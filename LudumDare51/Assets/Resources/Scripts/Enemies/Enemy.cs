using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float mSpeed = 1f;
    public float mBaseDamage = 1f;
    public bool mFacesPlayer = false;


    private GameObject mHealthBar;
    private Killable mKillable;
    private float mHealthBarInitialScaleX = 1f;

    public void Initialize()
    {
        mHealthBar = transform.Find("HealthBar").gameObject;
        Debug.Assert(mHealthBar != null, "No health bar");
        mHealthBarInitialScaleX = mHealthBar.transform.localScale.x;
        mHealthBar.SetActive(false);

        mKillable = GetComponent<Killable>();
        Debug.Assert(mKillable != null, "Not killable");
    }


    void FixedUpdate()
    {
        PlayerController thePlayer = GameManager.mInstance.mThePlayer;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector3 direction = (thePlayer.transform.position - transform.position).normalized;
        float finalSpeed = Time.fixedDeltaTime * mSpeed;

        rb.velocity = new Vector2(direction.x * finalSpeed, direction.y * finalSpeed);

        if( mFacesPlayer )
        {
            Vector3 playerPos = thePlayer.transform.position;
            Vector3 dir = playerPos - transform.position;
            float angle = Mathf.Atan2( dir.y, dir.x );
            float prevAngle = transform.eulerAngles.z;

            transform.eulerAngles = new Vector3( 0, 0, 180 + Utilities.RadToDeg(angle) );
            float newAngle = transform.eulerAngles.z;
            mHealthBar.transform.RotateAround( transform.position, new Vector3( 0, 0, 1), prevAngle - newAngle );
        }

        UpdateHealthBar();
    }


    void UpdateHealthBar()
    {
        float ratio = mKillable.mLife / mKillable.mBaseLife;

        if (ratio < 1.0)
        {
            mHealthBar.SetActive(true);
            mHealthBar.transform.localScale = new Vector3(mHealthBarInitialScaleX * ratio, mHealthBar.transform.localScale.y, 1);
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        Ludum51.Player.Player thePlayer = collision.collider.gameObject.GetComponent<Ludum51.Player.Player>();

        if (thePlayer != null)
        {
            Killable killable = thePlayer.GetComponent<Killable>();
            PlayerController pController = thePlayer.GetComponent<PlayerController>();

            killable.Hit(mBaseDamage);
            // TODO: connect life
            pController.UpdateHealthBar();

            if (killable.IsDead())
            {
                pController.PlayDeathAnimation(() =>
                {
                    thePlayer.gameObject.SetActive(false);
                    GameManager.mInstance.mLevelManager.Death( LevelManager.eDeathCause.kDed );
                });
            }
        }
    }


    //========================================
    // Collision
    //========================================
    void OnTriggerEnter2D( Collider2D collider )
    {
        Projectile projectile = collider.gameObject.GetComponent<Projectile>();

        if( projectile != null )
        {
            Shooter shooter = projectile.mWeapon.mShooter;
            bool shooterIsEnnemy = shooter.gameObject.GetComponent<Enemy>() != null;

            if( !shooterIsEnnemy && shooter.tag != "BossGun" )
            {
                mKillable.Hit( projectile.mWeapon.mBaseDamage * projectile.mWeapon.mShooter.mMultiplierDamage );
                if( mKillable.IsDead() )
                {
                    mKillable.Die();
                }

                if( !projectile.mWeapon.mPierce )
                {
                    GameObject.Destroy( projectile.gameObject );
                }
            }
        }
    }
}
