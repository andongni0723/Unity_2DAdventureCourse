using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Component")] 
    public PlayerStatusBar playerStatusBar;
    
    [Header("Events")]
    public CharacterEventSO healthEvent;
    //[Header("Settings")]
    //[Header("Debug")]

    #region Event

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHealth / character.maxHealth;
        playerStatusBar.OnHealthChange(percentage);
    }

    #endregion 
}
