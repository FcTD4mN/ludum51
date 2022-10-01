using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    //========================================
    // Members
    //========================================
    // PUBLIC
    public float mBaseDamage = 1f;
    public float mBaseArea = 1f;
    public float mBaseReloadTime = 1f;
    public float mBaseFireRatePerSec = 1f;
    public float mBaseProjectileSpeed = 1f;


    public int mBaseBullets = 1;
    public int mBullets = 1;

    public bool mIsShootAvailable = true;

    public MonoBehaviour mParent;
    public iShooter mShooter;

    // PRIVATE
    private bool mIsReloading = false;


    //========================================
    // Construction
    //========================================
    public Weapon( MonoBehaviour parent, iShooter shooter )
    {
        mParent = parent;
        mShooter = shooter;
    }


    //========================================
    // Methods
    //========================================
    public virtual void SpawnProjectile( Vector2 at, Vector2 to )
    {
        // Nothing here
    }


    public void Reload()
    {
        if( mIsReloading ) { return; }

        mIsReloading = true;
        mParent.StartCoroutine( Utilities.ExecuteAfter( mBaseReloadTime, ()=>{
            mBullets = mBaseBullets;
            mIsReloading = false;
            Debug.Log( "Reloaded" );
        }) );
    }
}
