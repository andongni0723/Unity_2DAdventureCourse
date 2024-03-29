using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ISaveable
{
    [Header("Event")]
    public VoidEventSO newGameEventSO;
    
    [Header("Settings")] 
    public float maxHealth;
    public float invulnerableDuration;
    public bool isTakeDamagePermanentInvulnerable;

    [Header("Debug")]
    public float currentHealth;
    public bool isInvulnerable;
    
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;
    
    
    private Timer invulnerableTimer = new Timer();


    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void NewGame()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    } 
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // Die 
            if (currentHealth > 0)
            {
                currentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDead?.Invoke(); 
            }
        }
    }

    #region Event

    private void OnEnable()
    {
        invulnerableTimer.timerStartEvent += TimerStart;
        invulnerableTimer.timerFinishEvent += TimerFinish;
        newGameEventSO.OnEventRaised += NewGame;
        
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        invulnerableTimer.timerStartEvent -= TimerStart;
        invulnerableTimer.timerFinishEvent -= TimerFinish;
        newGameEventSO.OnEventRaised -= NewGame;
        
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    
    private void TimerStart()
    {
        isInvulnerable = true;
    }

    private void TimerFinish()
    {
        isInvulnerable = false;
    }

    #endregion 

    /// <summary>
    /// The character has attacked by attacker (Call by Attack.cs)
    /// </summary>
    /// <param name="attacker">object to attack character</param>
    /// <param name="isPermanentInvulnerable">the invulnerable will open permanent, until you manual close</param>(
    public void TakeDamage(Attack attacker)
    {
        // Check invulnerable
        if(isInvulnerable) return;
        
        // Damage Check
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;

            if(isTakeDamagePermanentInvulnerable)
                isInvulnerable = true;
            else
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
        
        OnHealthChange?.Invoke(this);
        
    }

    /// <summary>
    /// Change state to invulnerable
    /// </summary>
    private void TriggerInvulnerable()
    {
        if (!isInvulnerable)
        {
            invulnerableTimer.StartTimer(invulnerableDuration);
        }
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSaveDataDict[GetDataID().ID + "hp"] = currentHealth;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            data.floatSaveDataDict.Add(GetDataID().ID + "hp", currentHealth);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];
            currentHealth = data.floatSaveDataDict[GetDataID().ID + "hp"];
            OnHealthChange?.Invoke(this);
        }
    }
}