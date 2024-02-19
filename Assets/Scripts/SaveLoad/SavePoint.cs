using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SavePoint : MonoBehaviour, IInteractable, ISaveable
{
    [Header("Event")] 
    public VoidEventSO saveSceneEvent;
    
    [Header("Component")]
    public SpriteRenderer saveWordSpriteRenderer;

    public GameObject lightObj;
    
    [Header("Settings")]
    public Sprite lightSprite;
    public Sprite darkSprite;

    public bool isSave;
    //[Header("Debug")]

    private void OnEnable()
    { 
        UpdateSprite();
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    public void TriggerAction()
    {
        Debug.Log("Open Chest!");
        
        if(!isSave)
            OpenChest();
    }

    private void UpdateSprite()
    {
        saveWordSpriteRenderer.sprite = isSave ? lightSprite : darkSprite;
        lightObj.SetActive(isSave); 
    }
    
    private void OpenChest()
    {
        isSave = true;
        UpdateSprite();
        gameObject.tag = "Untagged";
        saveSceneEvent.RaiseEvent();
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.boolSaveDataDict.ContainsKey(GetDataID().ID))
        {
            data.boolSaveDataDict[GetDataID().ID] = isSave;
        }
        else
        {
            data.boolSaveDataDict.Add(GetDataID().ID, isSave);
        }
    }

    public void LoadData(Data data)
    {
        if (data.boolSaveDataDict.ContainsKey(GetDataID().ID))
        {
            isSave = data.boolSaveDataDict[GetDataID().ID];
            Debug.Log(isSave);
            UpdateSprite();
        }
    }
}
