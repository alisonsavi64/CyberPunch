using UnityEngine;

public struct HitInfo
{
    public readonly GameObject Attacker;
    public readonly int Damage;
    public readonly float Knockback;
    public readonly int HitstunFrames;
    public readonly bool FromRight;

    public HitInfo(GameObject attacker, int damage, float knockback, int hitstunFrames, bool fromRight)
    {
        Attacker = attacker;
        Damage = damage;
        Knockback = knockback;
        HitstunFrames = hitstunFrames;
        FromRight = fromRight;
    }
}

public interface IDamageable
{
    void ApplyHit(HitInfo hit);
}
