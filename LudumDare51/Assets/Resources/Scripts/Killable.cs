using UnityEngine;




public class Killable : MonoBehaviour
{
    public float mBaseLife = 100f;
    public float mLife = 100f;


    public void Hit( float damage )
    {
        mLife -= damage;
    }


    public bool IsDead()
    {
        return  mLife <= 0;
    }


    public void Die( )
    {
        GameObject.Destroy( gameObject );
    } 

}

 