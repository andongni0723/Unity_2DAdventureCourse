using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Settings")] 
    public float maxHealth;
    public float invulnerableDuration;

    [Header("Debug")]
    public float currentHealth;
    public bool isInvulnerable;
    
    [Space(15)]
    [Header("Event")]
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;
    
    
    private Timer invulnerableTimer = new Timer();


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    #region Event

    private void OnEnable()
    {
        invulnerableTimer.timerFinishedEvent += TimerFinished;
    }

    private void OnDisable()
    {
        invulnerableTimer.timerFinishedEvent -= TimerFinished;
    }

    private void TimerFinished()
    {
        isInvulnerable = false;
    }

    #endregion 

    /// <summary>
    /// The character has attacked by attacker (Call by Attack.cs)
    /// </summary>
    /// <param name="attacker">object to attack character</param>
    public void TakeDamage(Attack attacker)
    {
        // Check invulnerable
        if(isInvulnerable) return;
        
        // Damage Check
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            
            // Hurt Action
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            Debug.Log("Dead");
            
            // Dead Action
            OnDead?.Invoke();
        }
        
    }

    /// <summary>
    /// Change state to invulnerable
    /// </summary>
    private void TriggerInvulnerable()
    {
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            invulnerableTimer.StartTimer(invulnerableDuration);
        }
    }
}