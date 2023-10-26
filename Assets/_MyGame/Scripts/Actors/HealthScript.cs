using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    [System.Serializable]
    public class HealthScript 
    {
        private int maxHealth;
        private int currentHealth;

        private float waitTimerBeforeRegenerationStart = 5;
        private float rateOfHealthRegeneration = 0.5f; //every half second we regenerate health
        private int regenerationAmountPercent = 2; //will regenerate 2% of total health;

        private float currentWaitTime;
        private float currentRegenerationTime;

        public float healthRatio => ((1f * currentHealth) / maxHealth);
        public bool canPickUpHealth => currentHealth < maxHealth;
        public bool IsDead => currentHealth <= 0;


        public UnityEvent<float> OnHealthRatioChangeEvent;

        public HealthScript()
        {
            OnHealthRatioChangeEvent = new UnityEvent<float>();
        }

        public void SetHealth(int value)
        {
            maxHealth = value;
            currentHealth = value;
        }

        public void IncreaseHealth(int value)
        {
            currentHealth += value;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            OnHealthRatioChangeEvent?.Invoke(healthRatio);
        }

        public int Damage(int damage)
        {
            currentHealth -= damage;

            OnHealthRatioChangeEvent?.Invoke(healthRatio);

            if (currentHealth < maxHealth)
            {
                currentWaitTime = waitTimerBeforeRegenerationStart;
                currentRegenerationTime = rateOfHealthRegeneration;
            }

            return currentHealth;
        }

        public void HealthRegenerationProcess()
        {
            if (currentHealth < maxHealth)
            {
                if (currentWaitTime > 0)
                {
                    currentWaitTime -= Time.deltaTime;
                }
                else
                {
                    currentRegenerationTime -= Time.deltaTime;
                    if (currentRegenerationTime <= 0)
                    {
                        currentRegenerationTime = rateOfHealthRegeneration;
                        currentHealth += Mathf.FloorToInt(maxHealth * (regenerationAmountPercent / 100f));
                        OnHealthRatioChangeEvent?.Invoke(healthRatio);
                        if (currentHealth > maxHealth)
                        {
                            currentHealth = maxHealth;
                        }
                    }
                }
            }
        }

    }
}