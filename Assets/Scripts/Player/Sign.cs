using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    [Header("Component")]
    public Transform playerTransform;
    public GameObject signSprite;
    private Animator animator => signSprite.GetComponent<Animator>();
    private PlayerInputControls playerInput;

    //[Header("Settings")]
    //[Header("Debug")]

    private bool canPress;

    private void Awake()
    {
        playerInput = new PlayerInputControls();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    /// <summary>
    /// According the device, play the animation 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="actionChange"></param>
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    animator.Play("Keyboard");
                    break;
                default:
                    animator.Play("PS");
                    break;
            }
        }
    }

    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playerTransform.localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Interactable"))
            canPress = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}