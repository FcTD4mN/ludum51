using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public void FinishCurrentRoom()
    {
        StartCoroutine(FinishRoomAnimation());
    }

    public void LoadNextRoom()
    {
        StartCoroutine(NextRoomAnimation());
    }

    public IEnumerator FinishRoomAnimation()
    {
        // Pause game and Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load next room (move camera)
    }

    public IEnumerator NextRoomAnimation()
    {
        // Play animation
        transition.SetTrigger("End");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Resume game
    }
}
