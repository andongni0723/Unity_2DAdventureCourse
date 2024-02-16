using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SavePoint : MonoBehaviour, IInteractable
{
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
        saveWordSpriteRenderer.sprite = isSave ? lightSprite : darkSprite;
        lightObj.SetActive(isSave);
    }

    public void TriggerAction()
    {
        Debug.Log("Open Chest!");
        
        if(!isSave)
            OpenChest();
    }
    
    private void OpenChest()
    { 
        saveWordSpriteRenderer.sprite = lightSprite;
        isSave = true;
        lightObj.SetActive(true);
        gameObject.tag = "Untagged";
    }
}
