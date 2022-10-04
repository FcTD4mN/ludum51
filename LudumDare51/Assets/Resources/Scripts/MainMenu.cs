using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int mLevel;
    private List<string> mWeapons;
    private AudioManager mAudioManager;

    public void Start()
    {
        // Load Audio Manager
        mAudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Debug.Assert(mAudioManager != null);
        mAudioManager.Initialize();

        // Load Saved Data
        if (FileManager.LoadFromFile("SaveGlobalData.json", out var gameData))
        {
            GameStat gStat = new GameStat();
            gStat.LoadFromJson(gameData);
            mLevel = gStat.mLevel;
        }
        else
            mLevel = 1;

        // Load list of weapons
        // FileManager.LoadFromFile("Assets\\Resources\\PersitentData", "weapons.json", out var json);
        // Debug.Log(json);

    }

    public void PlayGame()
    {
        // Selection sound
        mAudioManager.Play("selection");

        // Make sure Scene are in the right order
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadNextScene();

        // Clear run data
        SaveDataManager.RemoveAllData("SaveGameData.json");
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void MenuSelectWeaponClicked_Rifle()
    {
        // Selection sound
        mAudioManager.Play("selection");
        GameManager.mWeaponChoice = 0;
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadNextScene();
    }

    public void MenuSelectWeaponClicked_Knife()
    {
        // Selection sound
        mAudioManager.Play("selection");
        GameManager.mWeaponChoice = 1;
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadNextScene();
    }

    public void MenuSelectWeaponClicked_Back()
    {
        // Selection sound
        mAudioManager.Play("selection");
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadPreviousScene();
    }
}
