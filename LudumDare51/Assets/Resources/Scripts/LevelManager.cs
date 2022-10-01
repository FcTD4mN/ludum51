using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    // Timer for every 10 seconds
    private float mGameTime = 10;
    // UI Elements
    private GameObject canvas;
    private GameObject gameOverPanel;
    private GameObject mTimer;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Retrieve Timer UI element
        gameOverPanel = GameObject.Find("GameOverPanel").gameObject;
        gameOverPanel.SetActive(false);
        canvas = canvas = GameObject.Find("Canvas");
        mTimer = canvas.transform.Find("Chrono").gameObject;
        mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);
        UpdateTimer();
    }

    void FixedUpdate()
    {
        // Timer
        mGameTime -= Time.fixedDeltaTime;
    }


    private void UpdateTimer()
    {
        StartCoroutine(Utilities.ExecuteAfter(1f, () =>
        {
            // Debug.Log(mGameTime);
            // Debug.Log(Utilities.FormatSecondsToMinuteAndSeconds(mGameTime));
            mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);
            if (mGameTime > 0)
                UpdateTimer();
            else
                Death();
        }));
    }

    // What happens if you run of out time (or die)
    private void Death()
    {
        // Pause le jeu
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        // Reset timer and hide death screen
        mGameTime = 10;
        mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        UpdateTimer();

        // Reset Character & Enemies : TO-DO
    }

}
