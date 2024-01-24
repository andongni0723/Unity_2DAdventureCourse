using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Settings")] 
    public float maxHealth;
    public float invulnerableDuration;
    
    [Header("Debug")]
    public float currentHealth;
    public bool isInvulnerable;

    private Timer invulnerableTimer = new Timer();
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    #region Event

    private void OnEnable()
    {
        invulnerableTimer.timerFinishedDelegate += TimerFinished;
    }

    private void OnDisable()
    {
        invulnerableTimer.timerFinishedDelegate -= TimerFinished;
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
        }
        else
        {
            currentHealth = 0;
            Debug.Log("Dead");
            //TODO: Dead 
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