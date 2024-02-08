using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    // [Header("Component")]
    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    
    [Header("Settings")]
    public Sprite openSprite;
    public Sprite closeSprite;

    public bool isDone;
    //[Header("Debug")]

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }

    public void TriggerAction()
    {
        Debug.Log("Open Chest!");
        
        if(!isDone)
            OpenChest();
    }
    
    private void OpenChest()
    { 
        spriteRenderer.sprite = openSprite;
        isDone = true;
        gameObject.tag = "Untagged";
    }
}
