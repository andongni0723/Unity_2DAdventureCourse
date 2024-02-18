using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();

    private Data saveData;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        saveData = new Data();
    }
    
    [Header("Event")]
    public VoidEventSO saveEvent;
    //[Header("Component")]
    //[Header("Settings")]
    //[Header("Debug")]

    private void OnEnable()
    {
        saveEvent.OnEventRaised += Save;
    }
    private void OnDisable()
    {
        saveEvent.OnEventRaised -= Save;
    }

    private void Update()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
            Load();
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
            saveableList.Add(saveable);
    }
    
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + " : " + item.Value);
        }
    }

    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadDate(saveData);
        } 
    }
}
