using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //========================================
    // Members
    //========================================
    public float mLifeTime = 2f;

    public Weapon mWeapon;


    //========================================
    // Unity Methods
    //========================================
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( CheckOutOfBounds() );
        StartCoroutine( Utilities.ExecuteAfter(mLifeTime, OnProjectileDeath) );
    }


    //========================================
    // Projectile methods
    //========================================
    public virtual void OnProjectileDeath()
    {
        GameObject.Destroy( gameObject );
    }


    private IEnumerator CheckOutOfBounds()
    {
        while( Utilities.IsObjectOnScreen( gameObject ) )
        { 
            yield return new WaitForSeconds( 3f );
        } 
        
        GameObject.Destroy( gameObject );
    }


    //========================================
    // Collision
    //========================================
    void OnTriggerEnter2D( Collider2D collider )
    {
        Enemy ennemy = collider.gameObject.GetComponent<Enemy>();

        if( ennemy != null )
        {
            Killable killable = ennemy.GetComponent<Killable>();
            killable.Hit( mWeapon.mBaseDamage * mWeapon.mShooter.mMultiplierDamage );

            if( killable.IsDead() )
            {
                killable.Die();
            }
            
            if( !mWeapon.mPierce )
            {
                GameObject.Destroy( gameObject );
            }
        }
    }
}
