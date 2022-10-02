using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : MonoBehaviour
{
    private Shooter mShooter;
    private GameObject mGun1;
    private GameObject mGun2;
    private Shooter mGun1Shooter;
    private Shooter mGun2Shooter;

    public void Initialize()
    {
        mShooter = GetComponent<Shooter>();
        Debug.Assert( mShooter,"No Shooter" );
        Weapon mainWeapon = new EnnemyBasicRifle( this, mShooter );
        mainWeapon.mBaseBullets = 1;
        mainWeapon.mBullets = 1;
        mShooter.SetWeapon( mainWeapon );

        mGun1 = transform.Find( "Gun1" ).gameObject;
        Debug.Assert( mGun1,"No mGun1" );
        mGun1Shooter = mGun1.GetComponent<Shooter>();
        Debug.Assert( mGun1Shooter,"No mGun1Shooter" );
        Weapon gun1Weapon = new EnnemyBasicRifle( this, mGun1Shooter );
        gun1Weapon.mBaseBullets = 10;
        gun1Weapon.mBullets = 10;
        mGun1Shooter.SetWeapon( gun1Weapon );

        mGun2 = transform.Find( "Gun2" ).gameObject;
        Debug.Assert( mGun2,"No mGun2" ); 
        mGun2Shooter = mGun2.GetComponent<Shooter>();
        Debug.Assert( mGun2Shooter,"No mGun2Shooter" );
        Weapon gun2Weapon = new EnnemyBasicRifle( this, mGun2Shooter );
        gun2Weapon.mBaseBullets = 10;
        gun2Weapon.mBullets = 10;
        mGun2Shooter.SetWeapon( gun2Weapon );
    }


    // Update is called once per frame
    void Update()
    {  
        Vector3 playerPosition = GameManager.mInstance.mThePlayer.transform.position; 
        mShooter.TryShoot( transform.position, playerPosition ); 
        mGun1Shooter.TryShoot( mGun1.transform.position, playerPosition ); 
        mGun2Shooter.TryShoot( mGun2.transform.position, playerPosition ); 
    }
}
