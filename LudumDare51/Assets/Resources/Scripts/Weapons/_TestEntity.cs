using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestEntity : MonoBehaviour, iShooter
{
    //========================================
    // iShooter Members
    //========================================
    public Weapon mWeapon { get; set; }

    public float mMultiplierDamage      { get; set; }
    public float mMultiplierArea        { get; set; }
    public float mMultiplierReloadTime  { get; set; } 
    public float mMultiplierFireRate   { get; set; } 
    public float mMultiplierProjectileSpeed   { get; set; } 


    //========================================
    // iShooter Methods
    //========================================
    void InitializeIShooter()
    {
        mWeapon = new Knife( this, this );
        // mWeapon = new Rifle( this, this );

        mMultiplierDamage = 1f;
        mMultiplierArea = 1f;
        mMultiplierReloadTime = 1f;
        mMultiplierFireRate = 1f;
        mMultiplierProjectileSpeed = 1f;
    }

    void UpdateIShooter()
    {
        if( mIsMouseDown || mMouseWasDown )
        {
            mMouseWasDown = false;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            this.TryShoot( transform.position, mousePosition ); 
        } 
    }


    //========================================
    // Unity Methods
    //========================================
    // Start is called before the first frame update
    void Start()
    {
        InitializeIShooter();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIShooter();
    }


    //========================================
    // Mouse
    //========================================
    private bool mIsMouseDown = false;
    private bool mMouseWasDown = false;
    public void MouseDown( Event mouseEvent )
    {
        mIsMouseDown = true;
        mMouseWasDown = true;
    }
    
    public void MouseUp( Event mouseEvent ) 
    {
        mIsMouseDown = false;
    }

}
