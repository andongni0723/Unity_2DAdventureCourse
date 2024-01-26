using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private PlayerInputControls inputControls;
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    private Collider2D collider => GetComponent<Collider2D>();
    private PhysicsCheck physicsCheck => GetComponent<PhysicsCheck>();
    private PlayerAnimation playerAnimation => GetComponent<PlayerAnimation>();

    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("Settings")] 
    public float speed = 10;
    public float hurtForce = 5;
    public float jumpForce = 10;
    public float canJumpCount = 2;

    [Header("Debug")]
    [SerializeField] private Vector2 inputDirection;
    [SerializeField] private bool isWalk;
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    private int currentJumpCount = 0;

    private void Awake()
    {
        inputControls = new PlayerInputControls();
        inputControls.Gameplay.Jump.started += _ => Jump();
        inputControls.Gameplay.Walk.started += _ => ChangeToWalk();
        inputControls.Gameplay.Attack.started += _ => PlayerAttack();
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
        if(!isHurt && !isAttack)
            Movement();

        CheckState();
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
            transform.localScale = new Vector3(1, 1, 1);
        if (inputDirection.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

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
    
    private void PlayerAttack()
    {
        isAttack = true;
        playerAnimation.PlayAttack();
    }
    
    private void CheckState()
    {
        // Change physics material
        collider.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

    #region Unity Event
    
    /// <summary>
    /// Player Add hurt force (Call by Character.cs)
    /// </summary>
    /// <param name="attacker"></param>
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        
        Vector2 reboundDir = new Vector2(-(attacker.transform.position.x - transform.position.x), 0).normalized;
        rb.AddForce(reboundDir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControls.Gameplay.Disable();
    }
    
    #endregion

}
