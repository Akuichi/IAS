using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    public event Action OnHealthZero;
    public event Action OnPoiseZero;

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private float maxPoise = 50f;
    private float currentPoise;

    [SerializeField] private float poiseRegenAmount = 1f;
    [SerializeField] private float poiseRegenCooldown = 3f;
    private float lastPoiseDamagedTime;
    private bool canRegenPoise;
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public float GetPoisePercentage() => currentPoise / maxPoise;

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
        currentPoise = maxPoise;
        canRegenPoise = true;
    }

    private void Update()
    {
        if (Time.time >= lastPoiseDamagedTime + poiseRegenCooldown)
        {
            canRegenPoise = true;
        }

        if (canRegenPoise && currentPoise < maxPoise)
        {
           IncreasePoise(poiseRegenAmount * Time.deltaTime);
        }
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        if (currentHealth == 0)
        {
            OnHealthZero?.Invoke();
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void DecreasePoise(float amount)
    {
        canRegenPoise = false;
        lastPoiseDamagedTime = Time.time;
        currentPoise = Mathf.Clamp(currentPoise - amount, 0, maxPoise);
        if (currentPoise == 0)
        {
            OnPoiseZero?.Invoke();
        }
    }

    public void IncreasePoise(float amount)
    {
        currentPoise = Mathf.Clamp(currentPoise + amount, 0, maxPoise);
    }

}
