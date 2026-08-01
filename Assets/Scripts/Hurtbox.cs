using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
public class Hurtbox : MonoBehaviour
{
    [SerializeField] Vector3 center = new Vector3(0f, 1f, 0f);
    [SerializeField] Vector3 size = new Vector3(0.6f, 1.8f, 0.6f);

    public FighterStateMachine Owner { get; private set; }

    void Awake()
    {
        Owner = GetComponent<FighterStateMachine>();

        var hurtboxCollider = gameObject.AddComponent<BoxCollider>();
        hurtboxCollider.isTrigger = true;
        hurtboxCollider.center = center;
        hurtboxCollider.size = size;
    }
}
