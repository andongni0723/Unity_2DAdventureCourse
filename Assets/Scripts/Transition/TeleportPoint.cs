using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    //[Header("Component")]
    [Header("Settings")] 
    public Vector3 positionToGo;
    //[Header("Debug")]
    public void TriggerAction()
    {
        Debug.Log("Teleport!");
    }
}
