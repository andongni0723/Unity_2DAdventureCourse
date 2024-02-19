using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Event")] 
    public VoidEventSO pauseEvent;
    
    [Header("Component")] 
    public PlayerStatusBar playerStatusBar;

    public GameObject gameOverPanel;
    public GameObject restartButton;
    public GameObject mobileTouch;
    public Button settingButton;
    public GameObject pausePanel;
    public Slider volumeSlider;
    
    [Header("Events")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO unloadSceneEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO syncVolumeEvent;
    //[Header("Settings")]
    //[Header("Debug")]

    private void Awake()
    {
        mobileTouch.SetActive(false);
        
        
        settingButton.onClick.AddListener(TogglePausePanel);
    }

    

    #region Event

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        unloadSceneEvent.LoadRequestEvent += OnUnLoadSceneEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        unloadSceneEvent.LoadRequestEvent -= OnUnLoadSceneEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    private void OnSyncVolumeEvent(float amount)
    {
        volumeSlider.value = amount;
    }

    private void TogglePausePanel()
    {
        if(!pausePanel.activeInHierarchy)  // Prepare to Pause
            pauseEvent.RaiseEvent();
        
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
        Time.timeScale = pausePanel.activeInHierarchy ? 0 : 1;
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnUnLoadSceneEvent(GameSceneSO sceneToGo, Vector3 pos, bool fadeScreen)
    {
        bool isMenu = sceneToGo.sceneType == SceneType.Menu;
       
        playerStatusBar.gameObject.SetActive(!isMenu);
        
        #if !UNITY_STANDALONE
        mobileTouch.SetActive(!isMenu);
        #endif
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.currentHealth / character.maxHealth;
        playerStatusBar.OnHealthChange(percentage);
    }

    #endregion 
}
