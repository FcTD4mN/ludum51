using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyShooterIA : MonoBehaviour
{
    private Shooter mShooter;



    public void Initialize()
    {
        mShooter = GetComponent<Shooter>();
        Debug.Assert( mShooter,"No Shooter" );
        mShooter.SetWeapon( new EnnemyBasicRifle( this, mShooter ) );
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = GameManager.mInstance.mThePlayer.transform.position; 
        mShooter.TryShoot( transform.position, playerPosition ); 
    }
}
