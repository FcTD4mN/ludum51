using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{  
    public float mSpeed = 1f;
    public float mBaseDamage = 1f;

    void Start()
    { 
    } 


    void FixedUpdate()
    {
        PlayerController thePlayer = GameManager.mInstance.mThePlayer;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector3 direction = (thePlayer.transform.position - transform.position).normalized;
        float finalSpeed = Time.fixedDeltaTime * mSpeed;
        
        rb.velocity = new Vector2( direction.x * finalSpeed, direction.y * finalSpeed );
    }


    void OnCollisionStay2D( Collision2D collision )
    { 
        Ludum51.Player.Player thePlayer = collision.collider.gameObject.GetComponent<Ludum51.Player.Player>();

        if( thePlayer != null )
        {
            Killable killable = thePlayer.GetComponent<Killable>(); 
            PlayerController pController = thePlayer.GetComponent< PlayerController >();

            killable.Hit( mBaseDamage );
            // TODO: connect life

            if( killable.IsDead() )
            {
                pController.PlayDeathAnimation( ()=> {
                    thePlayer.gameObject.SetActive( false );
                });
            }
        }
    }
}
