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

    // Objects
    private PlayerController mThePlayer;


    // ======================================
    // Initialization
    // ======================================
    void OnEnable()
    {
        // If there is already an gamemanager instance 
        if (GameManager.mInstance != null) { return; }
        mInstance = this;
        
        // Get and initialize all managers
        mProjectileManager = transform.Find("ProjectileManager")?.gameObject.GetComponent<ProjectileManager>();
        Debug.Assert( mProjectileManager != null );
        mProjectileManager.Initialize();



        mThePlayer = GameObject.Find("Player").GetComponent<PlayerController>();
        Debug.Assert( mThePlayer != null );
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
