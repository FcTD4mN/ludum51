using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public void LoadNextRoom(GameObject canvas, GameObject cardCanvas, bool activate)
    {
        StartCoroutine(LoadRoom(canvas, cardCanvas, activate));
    }

    public IEnumerator LoadRoom(GameObject canvas, GameObject cardCanvas, bool activate)
    {
        // Play animation
        transition.SetTrigger("Start");

        // Wait
        yield return new WaitForSeconds(transitionTime);

        // Load next room (move camera)
        transition.SetTrigger("End");
        cardCanvas.SetActive(!activate);
        canvas.SetActive(activate);
    }
}
