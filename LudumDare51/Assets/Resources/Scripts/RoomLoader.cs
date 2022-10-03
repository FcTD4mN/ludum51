using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    // Use for IG transition
    public void FinishCurrentRoom()
    {
        // Play animation
        transition.SetTrigger("Start");
    }

    public void LoadNextRoom()
    {
        // Play animation
        transition.SetTrigger("End");
    }

    /*
    * Used for scene transition 
    * Using next Scene order
    */
    public void LoadNextScene()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public IEnumerator LoadScene(int whichScene)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load next scene
        SceneManager.LoadScene(whichScene);
    }

    /*
    * Used for scene transition 
    * Using next Scene order
    */
    public void LoadNextScene(string sName)
    {
        StartCoroutine(LoadScene(sName));
    }

    public IEnumerator LoadScene(string whichScene)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load next scene
        SceneManager.LoadScene(whichScene);
    }

}
