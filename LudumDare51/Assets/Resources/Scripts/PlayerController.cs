using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, iShooter
{
    public float moveSpeed = 4f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeIShooter();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

            }
            if (!success)
            {
                success = TryMove(new Vector2(0, movementInput.y));
            }
            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        // Set sprite direction
        if (movementInput.x < 0)
        {

            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        UpdateIShooter();
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }



    //========================================
    // iShooter Members
    //========================================
    public Weapon mWeapon { get; set; }

    public float mMultiplierDamage      { get; set; }
    public float mMultiplierArea        { get; set; }
    public float mMultiplierReloadTime  { get; set; } 
    public float mMultiplierFireRate   { get; set; } 
    public float mMultiplierProjectileSpeed   { get; set; } 


    //========================================
    // iShooter Methods
    //========================================
    void InitializeIShooter()
    {
        // mWeapon = new Knife( this, this );
        mWeapon = new Rifle( this, this );

        mMultiplierDamage = 10f;
        mMultiplierArea = 1f;
        mMultiplierReloadTime = 1f;
        mMultiplierFireRate = 1f;
        mMultiplierProjectileSpeed = 1f;
    }

    void UpdateIShooter()
    {
        if( mIsMouseDown || mMouseWasDown )
        {
            mMouseWasDown = false;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            this.TryShoot( transform.position, mousePosition ); 
        } 
    }


    //========================================
    // Mouse
    //========================================
    private bool mIsMouseDown = false;
    private bool mMouseWasDown = false;
    public void MouseDown( Event mouseEvent )
    {
        mIsMouseDown = true;
        mMouseWasDown = true;
    }
    
    public void MouseUp( Event mouseEvent ) 
    {
        mIsMouseDown = false;
    }

}
