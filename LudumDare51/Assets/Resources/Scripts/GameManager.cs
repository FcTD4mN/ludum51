using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private _TestEntity mTest;

    public ProjectileManager mProjectileManager;
    public static GameManager mInstance;

    void OnEnable()
    {
        // If there is already an gamemanager instance 
        if (GameManager.mInstance != null) { return; }
        mInstance = this;
        
        // Get and initialize all managers
        mProjectileManager = transform.Find("ProjectileManager")?.gameObject.GetComponent<ProjectileManager>();
        Debug.Assert( mProjectileManager != null );
        mProjectileManager.Initialize();






        // TMP ====
        mTest = GameObject.Find("Test").GetComponent<_TestEntity>();
        Debug.Assert( mTest != null );
    }


    // ======================================
    // Mouse Events
    // ======================================


    void OnGUI()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseDown)
        {
            mTest.MouseDown(m_Event);
        }

        if (m_Event.type == EventType.MouseDrag)
        {
        }

        if (m_Event.type == EventType.MouseUp)
        {
            mTest.MouseUp(m_Event);
        }
    }
}
