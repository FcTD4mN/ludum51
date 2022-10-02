using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{ 
    public Knife( MonoBehaviour parent, Shooter shooter ) : base( parent, shooter )
    { 
        mBaseDamage = 4f;
        mBaseArea = 1f;
        mBaseReloadTime = 0.3f;
        mBaseFireRatePerSec = 3f;
        mBaseProjectileSpeed = 90f;
        
        mPierce = true;
        
        mBaseBullets = 1;
        mBullets = 1;
    }

    
    override public void SpawnProjectile( Vector2 at, Vector2 to )
    {
        Vector2 direction = to - at;
        Vector2 directionNorm = direction.normalized;
        float angle = Mathf.Atan2( direction.y, direction.x );

        float projectileSize = mBaseArea * mShooter.mMultiplierArea; 
        float projectileSpeed = mBaseProjectileSpeed * mShooter.mMultiplierProjectileSpeed; 
        Vector3 location = new Vector3( at.x, at.y, -1 ) + new Vector3( directionNorm.x * projectileSize, directionNorm.y * projectileSize, -1 );

        GameObject projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/Projectile-Knife");
        GameObject projectile = GameObject.Instantiate( projectilePrefab, location, Quaternion.Euler(0, 0, 90 + Utilities.RadToDeg(angle)));
        projectile.transform.localScale = new Vector3( projectileSize * projectile.transform.localScale.x, 
                                                       projectileSize, // y is 1 by default
                                                       projectile.transform.localScale.z );

        Projectile projectileTyped = projectile.GetComponent<Projectile>();
        projectileTyped.mWeapon = this;

        // Sending toward direction
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        rb.centerOfMass = new Vector2( 0f, projectileSize );
        rb.angularVelocity = projectileSpeed;

        GameManager.mInstance.mProjectileManager.AddProjectile( projectile );
    }
}
