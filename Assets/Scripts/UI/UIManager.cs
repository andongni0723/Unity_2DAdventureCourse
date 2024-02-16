using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Component")] 
    public PlayerStatusBar playerStatusBar;
    
    [Header("Events")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO loadEventSO;
    //[Header("Settings")]
    //[Header("Debug")]

    #region Event

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSO sceneToGo, Vector3 pos, bool fadeScreen)
    {
        bool isMenu = sceneToGo.sceneType == SceneType.Menu;
       
        playerStatusBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHealth / character.maxHealth;
        playerStatusBar.OnHealthChange(percentage);
    }

    #endregion 
}
