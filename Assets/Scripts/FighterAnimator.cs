using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
public class FighterAnimator : MonoBehaviour
{
    [SerializeField] Color idleColor = new Color(0.7f, 0.7f, 0.8f);
    [SerializeField] Color attackColor = new Color(1f, 0.6f, 0.1f);
    [SerializeField] Color blockColor = new Color(0.2f, 0.6f, 1f);
    [SerializeField] Color hitstunColor = new Color(1f, 0.15f, 0.15f);
    [SerializeField] Color knockedOutColor = new Color(0.3f, 0.3f, 0.3f);

    FighterStateMachine stateMachine;
    Renderer fighterRenderer;
    MaterialPropertyBlock propertyBlock;

    void Awake()
    {
        stateMachine = GetComponent<FighterStateMachine>();
        fighterRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void LateUpdate()
    {
        Color targetColor = stateMachine.current switch
        {
            FighterStateMachine.State.LightAttack => attackColor,
            FighterStateMachine.State.HeavyAttack => attackColor,
            FighterStateMachine.State.Block => blockColor,
            FighterStateMachine.State.HitStun => hitstunColor,
            FighterStateMachine.State.KnockDown => knockedOutColor,
            _ => idleColor,
        };

        fighterRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", targetColor);
        fighterRenderer.SetPropertyBlock(propertyBlock);
    }
}
