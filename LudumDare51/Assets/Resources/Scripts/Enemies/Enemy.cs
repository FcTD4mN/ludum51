using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{  
    public float mSpeed = 1f;

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
}
