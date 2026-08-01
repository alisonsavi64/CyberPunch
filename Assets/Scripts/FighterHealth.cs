using System;
using UnityEngine;

[RequireComponent(typeof(FighterStateMachine))]
public class FighterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 100;

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

    public void ApplyHit(HitInfo hit)
    {
        if (IsKnockedOut) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - hit.Damage);
        HealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (IsKnockedOut)
        {
            stateMachine.EnterKnockDown();
            KnockedOut?.Invoke();
        }
        else
        {
            stateMachine.EnterHitStun(hit.HitstunFrames);
        }
    }
}
