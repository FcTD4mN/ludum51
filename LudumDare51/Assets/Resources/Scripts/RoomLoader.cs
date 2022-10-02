using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public void LoadNextRoom()
    {
        StartCoroutine(LoadRoom());
    }

    public IEnumerator LoadRoom()
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load next room (move camera)
        transition.SetTrigger("End");
    }
}
