using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private PlayerInputControls inputControls;
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    private PhysicsCheck physicsCheck => GetComponent<PhysicsCheck>();

    [Header("Settings")] 
    public float speed = 10;
    public float jumpForce = 10;
    public float canJumpCount = 2;
    
    [Header("Debug")]
    [SerializeField] private Vector2 inputDirection;

    [SerializeField] private bool isWalk;

    private int currentJumpCount = 0;

    private void Awake()
    {
        inputControls = new PlayerInputControls();
        inputControls.Gameplay.Jump.started += _ => Jump();
        inputControls.Gameplay.Walk.started += _ => ChangeToWalk();
    }

    #region Event

    private void OnEnable()
    {
        inputControls.Enable();
    }

    private void OnDisable()
    {
        inputControls.Disable();
    }

    #endregion

    private void FixedUpdate()
    {
        Movement();
    }
    
    /// <summary>
    /// Player move and flip
    /// </summary>
    private void Movement()
    {
        // Move
        inputDirection = inputControls.Gameplay.Move.ReadValue<Vector2>();
        rb.velocity = new Vector2(speed * inputDirection.x, rb.velocity.y);
        
        // Flip
        if (inputDirection.x > 0)
            spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }
    
    private void ChangeToWalk()
    {
        isWalk = !isWalk;
        speed = isWalk? speed / 2 : speed * 2;
    }

    /// <summary>
    /// Player check jump (Call from Awake)
    /// </summary>
    private void Jump()
    {
        if (physicsCheck.isGround)
            currentJumpCount = 0;

        // Check jump count
        if(currentJumpCount < canJumpCount)
        {
            currentJumpCount++;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
