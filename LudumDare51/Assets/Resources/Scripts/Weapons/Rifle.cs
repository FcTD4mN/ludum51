using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public Rifle( MonoBehaviour parent, iShooter shooter ) : base( parent, shooter )
    { 
        mBaseDamage = 2f;
        mBaseArea = 1f;
        mBaseReloadTime = 2f;
        mBaseFireRatePerSec = 3f;
        mBaseProjectileSpeed = 3f;
        
        mBaseBullets = 30;
        mBullets = 30;
    }

    
    override public void SpawnProjectile( Vector2 at, Vector2 to )
    {
        Vector2 direction = to - at;
        float angle = Mathf.Atan2( direction.y, direction.x );

        GameObject projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile-Rifle");
        GameObject projectile = GameObject.Instantiate( projectilePrefab, mParent.transform.position, Quaternion.Euler(0, 0, 90 + Utilities.RadToDeg(angle)));

        // Sending toward direction
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        float projectileSpeed = mBaseProjectileSpeed * mShooter.mMultiplierProjectileSpeed; 
        Vector2 directionNorm = direction.normalized;

        rb.velocity = new Vector2( directionNorm.x * projectileSpeed, directionNorm.y * projectileSpeed );

        GameManager.mInstance.mProjectileManager.AddProjectile( projectile );
    }
}
