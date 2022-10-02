using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private List<GameObject> mAllProjectiles;

    public void Initialize()
    {
        mAllProjectiles = new List<GameObject>(); 
    } 


    public void AddProjectile( GameObject projectile )
    {
        mAllProjectiles.Add( projectile );
    }


    public void ClearAllProjectiles()
    {
        foreach ( GameObject item in mAllProjectiles )
        {
            GameObject.Destroy( item );
        }

        mAllProjectiles.Clear();
    }
}
