using UnityEngine;

public struct HitInfo
{
    public readonly GameObject Attacker;
    public readonly int Damage;
    public readonly float Knockback;
    public readonly bool FromRight;

    public HitInfo(GameObject attacker, int damage, float knockback, bool fromRight)
    {
        Attacker = attacker;
        Damage = damage;
        Knockback = knockback;
        FromRight = fromRight;
    }
}

public interface IDamageable
{
    void ApplyHit(HitInfo hit);
}
