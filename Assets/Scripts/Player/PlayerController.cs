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
    public Vector2 inputDirection;

    private int currentJumpCount = 0;
    
    private void Awake()
    {
        inputControls = new PlayerInputControls();
        inputControls.Gameplay.Jump.started += _ => Jump();
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
        inputDirection = inputControls.Gameplay.Move.ReadValue<Vector2>().normalized;
        rb.velocity = new Vector2(speed * inputDirection.x, rb.velocity.y);
        
        // Flip
        spriteRenderer.flipX = inputDirection.x < 0;
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
