using UnityEngine;

/// <summary>
/// Dados de UM golpe — o "frame data". É um ScriptableObject: um asset que você cria e
/// edita no Inspector (sem recompilar código). Assim dá pra balancear o jogo mexendo em
/// números, e reutilizar o mesmo golpe em vários personagens.
///
/// Frame data (medido em FRAMES de física — no nosso setup, ~50/segundo):
///   - startup:  frames ANTES do golpe começar a machucar (quão rápido ele sai)
///   - active:   frames em que o golpe MACHUCA — a hitbox fica ligada (usado na Fase 3)
///   - recovery: frames de "trava" DEPOIS, em que você fica parado e vulnerável
///
/// Golpe leve/rápido = startup baixo, recovery baixo. Golpe forte = mais dano, porém
/// mais recovery (mais arriscado se errar). Esse trade-off é a alma de um jogo de luta.
/// </summary>
[CreateAssetMenu(fileName = "NewAttack", menuName = "CyberPunch/Attack Data")]
public class AttackData : ScriptableObject
{
    public string attackName = "Light Punch";

    [Header("Frame data (em frames)")]
    [Min(1)] public int startupFrames = 4;
    [Min(1)] public int activeFrames = 3;
    [Min(1)] public int recoveryFrames = 8;

    [Header("Efeito ao acertar (usado na Fase 3)")]
    public int damage = 8;
    public float knockback = 3f;

    /// <summary>Duração total do golpe, do botão até voltar a poder agir.</summary>
    public int TotalFrames => startupFrames + activeFrames + recoveryFrames;
}
