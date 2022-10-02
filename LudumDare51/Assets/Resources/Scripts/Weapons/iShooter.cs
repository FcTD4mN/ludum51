using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iShooter 
{
    Weapon mWeapon { get; set; }

    float mMultiplierDamage     { get; set; }
    float mMultiplierArea       { get; set; }
    float mMultiplierReloadTime { get; set; } 
    float mMultiplierFireRate   { get; set; } 
    float mMultiplierProjectileSpeed   { get; set; } 
}





public static class ExtensionShooter
{
    public static void TryShoot( this iShooter shooter, Vector2 from, Vector2 to )
    {
        Weapon theWeapon = shooter.mWeapon;

        if( theWeapon.mIsShootAvailable )
        {
            shooter.Shoot( from, to );
            theWeapon.mIsShootAvailable = false; 

            float timeToWait = 1 / (theWeapon.mBaseFireRatePerSec * shooter.mMultiplierFireRate);

            theWeapon.mParent.StartCoroutine( Utilities.ExecuteAfter( timeToWait,()=>{ 
                theWeapon.mIsShootAvailable = true;
            }));
        }

    }


    private static void Shoot( this iShooter shooter, Vector2 from, Vector2 to )
    {
        Weapon theWeapon = shooter.mWeapon;
        
        if( theWeapon.mBullets > 0 )
        { 
            theWeapon.SpawnProjectile( from, to );
            theWeapon.mBullets -= 1;
        }
        else
        { 
            theWeapon.Reload();
        }
    }

    
    public static void Reload( this iShooter shooter )
    {
        Weapon theWeapon = shooter.mWeapon;
        theWeapon.Reload();
    }

    
    public static void ResetShooter( this iShooter shooter )
    {
        Weapon theWeapon = shooter.mWeapon;
        theWeapon.Reset();
    }
}