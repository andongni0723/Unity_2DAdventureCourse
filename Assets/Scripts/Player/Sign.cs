using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class Sign : MonoBehaviour
{
    [Header("Component")]
    public Transform playerTransform;
    public GameObject signSprite;
    private Animator animator => signSprite.GetComponent<Animator>();
    private PlayerInputControls playerInput;

    //[Header("Settings")]
    //[Header("Debug")]

    private IInteractable targetItem;
    private GameObject targetItemObj;
    private bool canPress;

    private void Awake()
    {
        playerInput = new PlayerInputControls();
        playerInput.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.Gameplay.Confirm.started += _ => OnConfirm(); // Start interactive item action
    }

    private void OnDisable()
    {
        canPress = false;
    }

    private void OnConfirm()
    {
        if (canPress)
        {
            targetItem.TriggerAction();
            targetItemObj.GetComponent<AudioDefination>()?.PlayAudioClip();
        }
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
                case Gamepad:
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
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
            targetItemObj = other.gameObject;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
