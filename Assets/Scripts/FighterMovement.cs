using UnityEngine;
using UnityEngine.InputSystem; // Input System novo — usamos Keyboard.current direto (sem asset de InputActions)

/// <summary>
/// Movimento de um lutador num jogo 2.5D estilo Mortal Kombat.
///
/// Conceitos de jogo de luta embutidos aqui (é o que você está aprendendo):
///  - O lutador anda só no eixo X (frente/trás) e pula no eixo Y. O eixo Z (profundidade)
///    fica TRAVADO — é isso que faz um jogo "2.5D": visual 3D, jogabilidade num plano 2D.
///  - Toda a física roda no FixedUpdate (passo de tempo fixo). Jogos de luta dependem de
///    timing preciso e previsível ("determinístico"); FixedUpdate é a base disso.
///  - Auto-facing: o lutador SEMPRE vira de frente pro oponente. Isso é padrão do gênero
///    (você nunca luta "de costas") e vai ser essencial quando os golpes forem direcionais.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FighterMovement : MonoBehaviour
{
    public enum PlayerId { One, Two }

    [Header("Identidade")]
    [Tooltip("Player One usa A/D + W. Player Two usa as setas.")]
    public PlayerId player = PlayerId.One;

    [Tooltip("O outro lutador. Usado pro auto-facing (virar de frente pro oponente). Arraste aqui no Inspector.")]
    public Transform opponent;

    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;

    [Header("Checagem de chão")]
    [Tooltip("Camadas consideradas 'chão'. Por padrão, tudo. Refinamos com Layers numa fase futura.")]
    public LayerMask groundLayer = ~0;

    Rigidbody rb;
    Collider col;
    FighterStateMachine sm; // opcional: se existir, decide quando o lutador pode se mover

    float moveInput;   // -1 (esquerda), 0 (parado) ou +1 (direita) no eixo X
    bool jumpQueued;   // pulo pedido neste frame de input; consumido no próximo FixedUpdate
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        sm = GetComponent<FighterStateMachine>(); // pode ser null se ainda não foi adicionada

        // 2.5D: trava a profundidade (Z) para o corpo nunca sair do plano de luta,
        // e trava as rotações da física pra cápsula não tombar ao esbarrar em algo.
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // INPUT é lido no Update (roda a cada frame de render) para nunca perder um toque de tecla.
        var kb = Keyboard.current;
        if (kb == null) return; // nenhum teclado conectado

        if (player == PlayerId.One)
        {
            // Truque: (direita?1:0) - (esquerda?1:0) resulta em -1, 0 ou +1.
            moveInput = (kb.dKey.isPressed ? 1f : 0f) - (kb.aKey.isPressed ? 1f : 0f);
            if (kb.wKey.wasPressedThisFrame) jumpQueued = true;
        }
        else // PlayerId.Two
        {
            moveInput = (kb.rightArrowKey.isPressed ? 1f : 0f) - (kb.leftArrowKey.isPressed ? 1f : 0f);
            if (kb.upArrowKey.wasPressedThisFrame) jumpQueued = true;
        }
    }

    void FixedUpdate()
    {
        // FÍSICA/LÓGICA no passo fixo (determinismo).
        isGrounded = CheckGrounded();

        // A FSM manda: durante um ataque (ou hitstun) o lutador fica "ocupado" e não anda/pula.
        bool busy = sm != null && sm.IsBusy;

        // Controlamos a velocidade horizontal diretamente e preservamos a vertical
        // (pra gravidade/queda continuarem funcionando naturalmente).
        Vector3 v = rb.linearVelocity; // Unity 6: 'linearVelocity' (o antigo 'velocity' está deprecado)
        v.x = busy ? 0f : moveInput * moveSpeed;

        // Só pula se NÃO estiver ocupado, pediu pulo E está no chão.
        if (!busy && jumpQueued && isGrounded)
            v.y = jumpSpeed;
        jumpQueued = false; // consome o pedido de pulo

        rb.linearVelocity = v;

        FaceOpponent();
    }

    /// <summary>Um raio curto pra baixo detecta se há chão sob os pés.</summary>
    bool CheckGrounded()
    {
        Bounds b = col.bounds;
        // O raio parte do CENTRO da cápsula. Como a origem está DENTRO do próprio collider,
        // o raycast ignora o próprio corpo por padrão e só enxerga o que está abaixo.
        float distance = b.extents.y + 0.15f; // meia-altura + uma pequena folga
        return Physics.Raycast(b.center, Vector3.down, distance, groundLayer, QueryTriggerInteraction.Ignore);
    }

    /// <summary>Gira o lutador pra encarar o oponente (auto-facing).</summary>
    void FaceOpponent()
    {
        if (opponent == null) return;
        bool opponentIsRight = opponent.position.x > transform.position.x;
        transform.rotation = Quaternion.Euler(0f, opponentIsRight ? 0f : 180f, 0f);
    }

    /// <summary>Direção que o lutador encara. Golpes direcionais (fases futuras) vão usar isto.</summary>
    public bool FacingRight => opponent == null || opponent.position.x >= transform.position.x;

    // Desenha o raio de chão no editor quando o objeto está selecionado — ajuda a visualizar/depurar.
    void OnDrawGizmosSelected()
    {
        var c = GetComponent<Collider>();
        if (c == null) return;
        Bounds b = c.bounds;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(b.center, b.center + Vector3.down * (b.extents.y + 0.15f));
    }
}
