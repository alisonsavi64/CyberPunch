using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A máquina de estados (FSM) do lutador — o "cérebro" que decide o que ele pode fazer.
///
/// A cada instante o lutador está em EXATAMENTE UM estado. Cada estado define as ações
/// permitidas. É isso que impõe, de forma limpa, regras como "não pode andar no meio de
/// um soco" (o FighterMovement pergunta 'IsBusy' antes de deixar andar).
///
/// Nesta fase implementamos: Idle + LightAttack + HeavyAttack, com o ataque passando pelas
/// fases do frame data (Startup → Active → Recovery → Idle). HitStun/KnockDown/Block entram
/// nas próximas fases.
/// </summary>
[RequireComponent(typeof(FighterMovement))]
[RequireComponent(typeof(Hitbox))]
[RequireComponent(typeof(Hurtbox))]
public class FighterStateMachine : MonoBehaviour
{
    public enum State { Idle, LightAttack, HeavyAttack, HitStun, KnockDown }

    /// Onde um ataque está dentro do seu frame data.
    public enum AttackPhase { None, Startup, Active, Recovery }

    [Header("Golpes (arraste os assets AttackData aqui)")]
    public AttackData lightAttack;
    public AttackData heavyAttack;

    [Header("Debug — observe estes campos mudarem no Play mode")]
    public State current = State.Idle;
    public AttackPhase phase = AttackPhase.None;
    [Tooltip("Há quantos frames o lutador está no estado atual.")]
    public int frameInState;

    FighterMovement movement;
    Hitbox hitbox;
    AttackData activeAttackData;
    int hitstunFramesTarget;

    void Awake()
    {
        movement = GetComponent<FighterMovement>();
        hitbox = GetComponent<Hitbox>();
    }

    void Update()
    {
        // Botões de ataque lidos no Update (pra nunca perder um toque).
        // P1: J = leve, K = forte.   P2: , (vírgula) = leve, . (ponto) = forte.
        var kb = Keyboard.current;
        if (kb == null) return;

        bool lightPressed, heavyPressed;
        if (movement.player == FighterMovement.PlayerId.One)
        {
            lightPressed = kb.jKey.wasPressedThisFrame;
            heavyPressed = kb.kKey.wasPressedThisFrame;
        }
        else
        {
            lightPressed = kb.commaKey.wasPressedThisFrame;
            heavyPressed = kb.periodKey.wasPressedThisFrame;
        }

        // Só dá pra INICIAR um ataque a partir de Idle (não durante outro ataque).
        if (current == State.Idle)
        {
            if (lightPressed && lightAttack != null) StartAttack(State.LightAttack, lightAttack);
            else if (heavyPressed && heavyAttack != null) StartAttack(State.HeavyAttack, heavyAttack);
        }
    }

    void FixedUpdate()
    {
        // O tempo avança em FRAMES aqui (passo fixo = determinismo).
        frameInState++;

        if (current == State.LightAttack || current == State.HeavyAttack)
            TickAttack();
        else if (current == State.HitStun)
            TickHitStun();
    }

    void StartAttack(State attackState, AttackData data)
    {
        current = attackState;
        activeAttackData = data;
        phase = AttackPhase.Startup;
        frameInState = 0;
        hitbox.Deactivate();
    }

    void TickAttack()
    {
        int s = activeAttackData.startupFrames;
        int a = activeAttackData.activeFrames;
        int r = activeAttackData.recoveryFrames;

        AttackPhase previousPhase = phase;

        if (frameInState <= s)                phase = AttackPhase.Startup;
        else if (frameInState <= s + a)       phase = AttackPhase.Active;
        else if (frameInState <= s + a + r)   phase = AttackPhase.Recovery;
        else                                  { ReturnToIdle(); return; }

        if (phase == AttackPhase.Active && previousPhase != AttackPhase.Active)
            hitbox.Activate(activeAttackData);
        else if (previousPhase == AttackPhase.Active && phase != AttackPhase.Active)
            hitbox.Deactivate();
    }

    void ReturnToIdle()
    {
        current = State.Idle;
        phase = AttackPhase.None;
        frameInState = 0;
        activeAttackData = null;
        hitbox.Deactivate();
    }

    public void EnterHitStun(int hitstunFrames)
    {
        current = State.HitStun;
        phase = AttackPhase.None;
        frameInState = 0;
        hitstunFramesTarget = hitstunFrames;
        activeAttackData = null;
        hitbox.Deactivate();
    }

    void TickHitStun()
    {
        if (frameInState >= hitstunFramesTarget)
            ReturnToIdle();
    }

    public void EnterKnockDown()
    {
        current = State.KnockDown;
        phase = AttackPhase.None;
        frameInState = 0;
        activeAttackData = null;
        hitbox.Deactivate();
    }

    /// <summary>Está "ocupado" (não pode andar/pular)? O FighterMovement usa isto.</summary>
    public bool IsBusy => current != State.Idle;

    /// <summary>A hitbox do golpe está ativa AGORA? A Fase 3 vai usar isto pra causar dano.</summary>
    public bool IsAttackActive => phase == AttackPhase.Active;
}
