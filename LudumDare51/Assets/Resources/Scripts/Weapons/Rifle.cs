using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public Rifle( MonoBehaviour parent, Shooter shooter ) : base( parent, shooter )
    { 
        mBaseDamage = 2f;
        mBaseArea = 1f;
        mBaseReloadTime = 2f;
        mBaseFireRatePerSec = 3f;
        mBaseProjectileSpeed = 3f;
        
        mPierce = false;

        mBaseBullets = 30;
        mBullets = 30;
    }

    
    override public void SpawnProjectile( Vector2 at, Vector2 to )
    {
        Vector2 direction = to - at;
        float angle = Mathf.Atan2( direction.y, direction.x );

        float projectileSpeed = mBaseProjectileSpeed * mShooter.mMultiplierProjectileSpeed; 
        float projectileSize = mBaseArea * mShooter.mMultiplierArea; 

        GameObject projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile-Rifle");
        GameObject projectile = GameObject.Instantiate( projectilePrefab, mParent.transform.position, Quaternion.Euler(0, 0, 90 + Utilities.RadToDeg(angle)));
        projectile.transform.localScale = new Vector3( projectileSize * projectile.transform.localScale.x, 
                                                       projectileSize * projectile.transform.localScale.y,
                                                       projectile.transform.localScale.z );
        Projectile projectileTyped = projectile.GetComponent<Projectile>();
        projectileTyped.mWeapon = this;

        // Sending toward direction
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Vector2 directionNorm = direction.normalized;

        rb.velocity = new Vector2( directionNorm.x * projectileSpeed, directionNorm.y * projectileSpeed );

        GameManager.mInstance.mProjectileManager.AddProjectile( projectile );
    }
}
