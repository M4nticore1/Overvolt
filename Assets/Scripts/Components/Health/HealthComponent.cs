using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;

    [SerializeField] private float currentHealth = 0;
    public float CurrentHealth => currentHealth;

    public bool IsAlive => currentHealth > 0;

    public event Action OnHealthChanged;
    public event Action<float> OnHealed;
    public event Action<float> OnDamaged;
    public event Action OnDied;

    public void SetMaxHealh(float value)
    {
        maxHealth = value;
    }

    public void SetCurrentHealth(float value)
    {
        value = Mathf.Clamp(value, 0, maxHealth);
        if (value == currentHealth) return;

        currentHealth = value;
        OnHealthChanged?.Invoke();

        if (!IsAlive) {
            Die();
        }
    }

    public void AddHealth(float value)
    {
        if (CurrentHealth >= maxHealth) return;

        SetCurrentHealth(CurrentHealth + value);

        OnHealed?.Invoke(value);
    }

    public void TakeDamage(float value)
    {
        if (CurrentHealth < 0f) return;

        SetCurrentHealth(CurrentHealth - value);

        OnDamaged?.Invoke(value);
    }

    // ILocalizable
    public Dictionary<string, string> GetLocalization()
    {
        return new Dictionary<string, string>()
        {
            { "currentHealth", Mathf.Max(Mathf.CeilToInt(CurrentHealth), maxHealth).ToString() },
            { "maxHealth", Mathf.Max(Mathf.CeilToInt(MaxHealth), maxHealth).ToString() },
            { "currentHealthPercent", MaxHealth > 0 ? Mathf.CeilToInt(CurrentHealth / MaxHealth * 100).ToString(): "0" },
        };
    }

    // Die
    private void Die()
    {
        OnDied?.Invoke();
    }
}