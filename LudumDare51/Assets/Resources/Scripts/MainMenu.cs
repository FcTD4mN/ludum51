using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Make sure Scene are in the right order
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void MenuSelectWeaponClicked_Rifle()
    {
        Debug.Log( "Rifle" );
        GameManager.mWeaponChoice = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MenuSelectWeaponClicked_Knife()
    {
        GameManager.mWeaponChoice = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MenuSelectWeaponClicked_Back()
    {
        Debug.Log( "Back" );
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
