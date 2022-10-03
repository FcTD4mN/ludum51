using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private LevelManager levelManager;
    private GameObject mHealthBar;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    Killable mKillable;
    Shooter mShooter;

    // ======================================
    // Unity Methods
    // ======================================
    // Start is called before the first frame update
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        mKillable = GetComponent<Killable>();
        Debug.Assert(mKillable, "No mKillable");

        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        mShooter = GetComponent<Shooter>();
        Debug.Assert(mShooter, "No mShooter");

        Weapon theWeapon = new Rifle( this, mShooter );
        switch( GameManager.mWeaponChoice )
        {
            case 0:
                theWeapon = new Rifle( this, mShooter );
                break;
            case 1:
                theWeapon = new Knife( this, mShooter );
                break;
        }
        mShooter.SetWeapon( theWeapon );

        mHealthBar = GameObject.Find("Canvas/InGamePanel/HealthBar");
        InitGUIVariables();

        ResetShooter();
        Reset();
    }


    // ======================================
    // Input
    // ======================================
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

    private void OnTriggerEnter2D(Collider2D envCollision)
    {
        if (envCollision.name == "Tilemap_Door")
        {
            levelManager.FinishRoom();
        }
        else if (envCollision.gameObject.GetComponent<Projectile>())
        {
            OnProjectileHit(envCollision);
        }
    }

    // ======================================
    // Life cycle
    // ======================================
    public void Reset()
    {
        movementInput = Vector2.zero;
        gameObject.SetActive(true);
        mKillable.mLife = GetComponent<Ludum51.Player.Player>().Health.BaseValue;
        animator.Play("Player_Idle", 0);

        ResetShooter();
        UpdateHealthBar();
    }


    public void PlayDeathAnimation(Action onAnimationFinish)
    {
        Utilities.StartAnim( this, animator, "Die", "Player_Death", onAnimationFinish );
    }


    public void UpdateHealthBar()
    {
        float lifeRatio = mKillable.mLife / mKillable.mBaseLife;
        // GameObject healthBar = GameObject.Find("Canvas/InGamePanel/HealthBar");
        GameObject healthBarLabel = mHealthBar.transform.Find("label").gameObject;

        Debug.Assert(mHealthBar && mHealthBar, "UpdateHealthBar missing object");

        mHealthBar.transform.localScale = new Vector3(lifeRatio, 1, 1);

        if (lifeRatio == 0)
        { lifeRatio = 1; }
        healthBarLabel.transform.localScale = new Vector3(1 / lifeRatio, 1, 1);
        healthBarLabel.GetComponent<TextMeshProUGUI>().text = mKillable.mLife + "/" + mKillable.mBaseLife;
    }


    //========================================
    // iShooter Methods
    //========================================
    void ResetShooter()
    {
        mShooter.ResetShooter();

        mShooter.mMultiplierDamage = 1f;
        mShooter.mMultiplierArea = 1f;
        mShooter.mMultiplierReloadTime = 1f;
        mShooter.mMultiplierFireRate = 1f;
        mShooter.mMultiplierProjectileSpeed = 1f;
        mShooter.mMultiplierProjectileCount = 1;
    }

    void UpdateIShooter()
    {
        if (mIsMouseDown || mMouseWasDown)
        {
            mMouseWasDown = false;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mShooter.TryShoot(transform.position, mousePosition);
        }
    }


    //========================================
    // Mouse
    //========================================
    private bool mIsMouseDown = false;
    private bool mMouseWasDown = false;
    public void MouseDown(Event mouseEvent)
    {
        mIsMouseDown = true;
        mMouseWasDown = !GameObject.Find("Canvas").activeSelf; // To avoid shooting while in UI
    }

    public void MouseUp(Event mouseEvent)
    {
        mIsMouseDown = false;
    }


    //========================================
    // Collision
    //========================================
    private void OnProjectileHit(Collider2D collider)
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();

        if (projectile == null)
            return;

        Shooter shooter = projectile.mWeapon.mShooter;
        if( shooter.gameObject == null )
            return;

        bool shooterIsEnnemy = shooter.gameObject.GetComponent<Enemy>() != null;

        if( shooterIsEnnemy || shooter.tag == "BossGun" )
        {
            mKillable.Hit(projectile.mWeapon.mBaseDamage * projectile.mWeapon.mShooter.mMultiplierDamage);
            UpdateHealthBar();
            if ( mKillable.IsDead() )
            {
                PlayDeathAnimation( ()=>
                {
                    gameObject.SetActive(false);
                    GameManager.mInstance.mLevelManager.Death( LevelManager.eDeathCause.kDed );
                });
            }

            if (!projectile.mWeapon.mPierce)
            {
                GameObject.Destroy(projectile.gameObject);
            }
        }
    }


    //========================================
    // GUI
    //========================================
    private TextMeshProUGUI mLabelDamage;
    private TextMeshProUGUI mLabelArea;
    private TextMeshProUGUI mLabelWeaponSpeed;
    private TextMeshProUGUI mLabelProjectileSpeed;
    private TextMeshProUGUI mLabelReloadTime;

    private void InitGUIVariables()
    {
        mLabelDamage = GameObject.Find( "Canvas/InGamePanel/Stats/LabelDamage" ).GetComponent<TextMeshProUGUI>();
        mLabelArea = GameObject.Find( "Canvas/InGamePanel/Stats/LabelArea" ).GetComponent<TextMeshProUGUI>();
        mLabelWeaponSpeed = GameObject.Find( "Canvas/InGamePanel/Stats/LabelWeaponSpeed" ).GetComponent<TextMeshProUGUI>();
        mLabelProjectileSpeed = GameObject.Find( "Canvas/InGamePanel/Stats/LabelProjectileSpeed" ).GetComponent<TextMeshProUGUI>();
        mLabelReloadTime = GameObject.Find( "Canvas/InGamePanel/Stats/LabelReloadTime" ).GetComponent<TextMeshProUGUI>();

        Debug.Assert( mLabelDamage, "mLabelDamage" );
        Debug.Assert( mLabelArea, "mLabelArea" );
        Debug.Assert( mLabelWeaponSpeed, "mLabelWeaponSpeed" );
        Debug.Assert( mLabelProjectileSpeed, "mLabelProjectileSpeed" );
        Debug.Assert( mLabelReloadTime, "mLabelReloadTime" );
    }

    public void UpdateGUI()
    {
        mLabelDamage.text = "Damage: " + mShooter.GetWeapon().mBaseDamage * mShooter.mMultiplierDamage;
        mLabelArea.text = "Area: " + mShooter.GetWeapon().mBaseArea * mShooter.mMultiplierArea;
        mLabelWeaponSpeed.text = "FireRate: " + mShooter.GetWeapon().mBaseFireRatePerSec * mShooter.mMultiplierFireRate;
        mLabelProjectileSpeed.text = "ProjectileSpeed: " + mShooter.GetWeapon().mBaseProjectileSpeed * mShooter.mMultiplierProjectileSpeed;
        mLabelReloadTime.text = "ReloadTime: " + mShooter.GetWeapon().mBaseReloadTime * mShooter.mMultiplierReloadTime;

        UpdateHealthBar();
    }

}
