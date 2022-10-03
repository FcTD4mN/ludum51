using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int mLevel;
    private List<string> mWeapons;

    public void Start()
    {
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
        // Make sure Scene are in the right order
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadNextScene();
    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public void MenuSelectWeaponClicked_Rifle()
    {
        GameManager.mWeaponChoice = 0;
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadNextScene();
    }

    public void MenuSelectWeaponClicked_Knife()
    {
        GameManager.mWeaponChoice = 1;
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadNextScene();
    }

    public void MenuSelectWeaponClicked_Back()
    {
        GameObject.Find("RoomLoader").GetComponent<RoomLoader>().LoadPreviousScene();
    }
}
