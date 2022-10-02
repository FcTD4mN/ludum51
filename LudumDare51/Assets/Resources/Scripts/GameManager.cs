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


    // ======================================
    // Initialization
    // ======================================
    void OnEnable()
    {
        // If there is already an gamemanager instance 
        if (GameManager.mInstance != null) { return; }
        mInstance = this;

        GameObject player = GameObject.Find("Player");
        mThePlayer = player.GetComponent<PlayerController>();
        Debug.Assert(mThePlayer && player, "Can't find player");
        player.GetComponent<Ludum51.Player.Player>().Initialize();
        mThePlayer.Initialize();


        // Get and initialize all managers
        mProjectileManager = transform.Find("ProjectileManager")?.gameObject.GetComponent<ProjectileManager>();
        Debug.Assert(mProjectileManager != null);
        mProjectileManager.Initialize();

        mEnnemyManager = transform.Find("EnnemyManager")?.gameObject.GetComponent<EnnemyManager>();
        Debug.Assert(mEnnemyManager != null);
        mEnnemyManager.Initialize();

        mLevelManager = transform.Find("LevelManager")?.gameObject.GetComponent<LevelManager>();
        Debug.Assert(mLevelManager != null);
        mLevelManager.Initialize();

        mCardManager = transform.Find("CardManager")?.gameObject.GetComponent<CardManager>();
        Debug.Assert(mCardManager != null);
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

        SaveDataManager.SaveJsonData(mCardManager, "SaveGameData.json");
    }
}
