using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    // Reference to Camera
    public GameObject mainCamera;

    public void OnEnable()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    public void LoadNextRoom(int roomNumber)
    {
        StartCoroutine(LoadRoom(roomNumber));
    }

    public IEnumerator LoadRoom(int roomNumber)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load next room (move camera)
        transition.SetTrigger("End");
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 28, 0f, -10f);
    }
}
