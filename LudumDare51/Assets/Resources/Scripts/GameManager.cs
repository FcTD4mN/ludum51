using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ======================================
    // Members
    // ======================================
    public static GameManager mInstance;

    // Managers
    public ProjectileManager mProjectileManager;
    public EnnemyManager mEnnemyManager;

    // Objects
    public PlayerController mThePlayer;


    // ======================================
    // Initialization
    // ======================================
    void OnEnable()
    {
        // If there is already an gamemanager instance 
        if (GameManager.mInstance != null) { return; }
        mInstance = this;
        
        mThePlayer = GameObject.Find("Player").GetComponent<PlayerController>();
        Debug.Assert( mThePlayer != null );


        // Get and initialize all managers
        mProjectileManager = transform.Find("ProjectileManager")?.gameObject.GetComponent<ProjectileManager>();
        Debug.Assert( mProjectileManager != null );
        mProjectileManager.Initialize();

        mEnnemyManager = transform.Find("EnnemyManager")?.gameObject.GetComponent<EnnemyManager>();
        Debug.Assert( mEnnemyManager != null );
        mEnnemyManager.Initialize(); 
    }


    // ======================================
    // Mouse Events
    // ======================================
    void OnGUI()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseDown)
        {
            mThePlayer.MouseDown(m_Event);
        }

        if (m_Event.type == EventType.MouseDrag)
        {
        }

        if (m_Event.type == EventType.MouseUp)
        {
            mThePlayer.MouseUp(m_Event);
        }
    }
}
