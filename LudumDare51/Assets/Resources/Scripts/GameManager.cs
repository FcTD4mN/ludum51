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
    public LevelManager mLevelManager;
    public CardManager mCardManager;

    // Objects
    public PlayerController mThePlayer;
    public GameObject mPlayerObject;


    // Scenes Data
    // Rifle = 0
    // Knife = 1
    // Grenade = 2
    public static int mWeaponChoice = 0;


    // ======================================
    // Initialization
    // ======================================
    void OnEnable()
    {
        // If there is already an gamemanager instance
        if (GameManager.mInstance != null) { return; }
        mInstance = this;

        mPlayerObject = GameObject.Find("Player");
        mThePlayer = mPlayerObject.GetComponent<PlayerController>();
        Debug.Assert(mThePlayer && mPlayerObject, "Can't find player");
        mPlayerObject.GetComponent<Ludum51.Player.Player>().Initialize();
        mThePlayer.Initialize();
        mPlayerObject.GetComponent<Ludum51.Player.Player>().SyncStatsToShooter();


        // Get and initialize all managers
        mProjectileManager = transform.Find("ProjectileManager")?.gameObject.GetComponent<ProjectileManager>();
        Debug.Assert(mProjectileManager != null);
        mProjectileManager.Initialize();

        mEnnemyManager = transform.Find("EnnemyManager")?.gameObject.GetComponent<EnnemyManager>();
        Debug.Assert(mEnnemyManager != null);
        mEnnemyManager.Initialize();

        mCardManager = transform.Find("CardManager")?.gameObject.GetComponent<CardManager>();
        Debug.Assert(mCardManager != null);
        mCardManager.Initialize();

        mLevelManager = transform.Find("LevelManager")?.gameObject.GetComponent<LevelManager>();
        Debug.Assert(mLevelManager != null);
        mLevelManager.Initialize();

    }


    // ======================================
    // Mouse Events
    // ======================================
    void OnGUI()
    {
        Event m_Event = Event.current;

        if (!mLevelManager.mCardSelection && m_Event.type == EventType.MouseDown)
        {
            mThePlayer.MouseDown(m_Event);
        }

        if (!mLevelManager.mCardSelection && m_Event.type == EventType.MouseDrag)
        {
        }

        if (!mLevelManager.mCardSelection && m_Event.type == EventType.MouseUp)
        {
            mThePlayer.MouseUp(m_Event);
        }
    }

    // ======================================
    // Save Datas
    // ======================================
    public void SaveGame()
    {
        // Save whatever you want : add it to the list
        List<ISaveable> saveData = new List<ISaveable>();
        saveData.Add(mCardManager);

        SaveDataManager.SaveJsonData(saveData, "SaveGameData.json");
    }

}
