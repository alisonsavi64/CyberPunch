using System;
using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
public class FighterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 100;
    [SerializeField, Range(0f, 1f)] float blockedDamageMultiplier = 0.2f;

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsKnockedOut => CurrentHealth <= 0;

    public event Action<int, int> HealthChanged;
    public event Action KnockedOut;

    FighterStateMachine stateMachine;

    void Awake()
    {
        stateMachine = GetComponent<FighterStateMachine>();
        CurrentHealth = maxHealth;
    }

    public void ResetHealth()
    {
        CurrentHealth = maxHealth;
        HealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void ApplyHit(HitInfo hit)
    {
        if (IsKnockedOut) return;

        bool blocked = stateMachine.IsBlocking;
        int damage = blocked ? Mathf.Max(1, Mathf.RoundToInt(hit.Damage * blockedDamageMultiplier)) : hit.Damage;
        int minHealth = blocked ? 1 : 0;

        CurrentHealth = Mathf.Max(minHealth, CurrentHealth - damage);
        HealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (IsKnockedOut)
        {
            stateMachine.EnterKnockDown();
            KnockedOut?.Invoke();
            AudioManager.Instance?.PlayKnockOut();
        }
        else if (blocked)
        {
            AudioManager.Instance?.PlayBlock();
        }
        else
        {
            stateMachine.EnterHitStun(hit.HitstunFrames);
            AudioManager.Instance?.PlayHit();
        }
    }
}
