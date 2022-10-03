using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Start()
    {
        FileManager.LoadFromFile("Assets\\Resources\\PersitentData", "weapons.json", out var json);
        Debug.Log(json);
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
