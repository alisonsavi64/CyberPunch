using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
public class Hitbox : MonoBehaviour
{
    [SerializeField] Vector3 center = new Vector3(0.6f, 1f, 0f);
    [SerializeField] Vector3 size = new Vector3(0.5f, 0.8f, 0.6f);

    FighterStateMachine owner;
    BoxCollider hitboxCollider;
    AttackData activeAttack;
    readonly HashSet<FighterStateMachine> hitTargetsThisSwing = new HashSet<FighterStateMachine>();

    void Awake()
    {
        owner = GetComponent<FighterStateMachine>();

        hitboxCollider = gameObject.AddComponent<BoxCollider>();
        hitboxCollider.isTrigger = true;
        hitboxCollider.enabled = false;
        hitboxCollider.center = center;
        hitboxCollider.size = size;
    }

    public void Activate(AttackData attackData)
    {
        activeAttack = attackData;
        hitTargetsThisSwing.Clear();
        hitboxCollider.enabled = true;
    }

    public void Deactivate()
    {
        hitboxCollider.enabled = false;
        activeAttack = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (activeAttack == null) return;

        var hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox == null || hurtbox.Owner == owner) return;
        if (!hitTargetsThisSwing.Add(hurtbox.Owner)) return;

        bool fromRight = owner.transform.position.x > hurtbox.Owner.transform.position.x;
        var hit = new HitInfo(owner.gameObject, activeAttack.damage, activeAttack.knockback, fromRight);
        hurtbox.Owner.GetComponent<IDamageable>()?.ApplyHit(hit);
    }
}
