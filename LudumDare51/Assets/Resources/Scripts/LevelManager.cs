using System;
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

    // Reference to Room Loader (Animation)
    private GameObject RoomLoader;

    // keep a copy of the executing coroutine (timer)
    private IEnumerator timerCoroutine;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Retrieve GameObjects reference
        RoomLoader = GameObject.Find("RoomLoader");

        // Retrieve Timer UI element
        gameOverPanel = GameObject.Find("GameOverPanel").gameObject;
        gameOverPanel.SetActive(false);
        canvas = canvas = GameObject.Find("Canvas");
        mTimer = canvas.transform.Find("Chrono").gameObject;
        mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);

        // Timer Coroutine
        UpdateTimer();
    }

    void FixedUpdate()
    {
        // Timer
        mGameTime -= Time.fixedDeltaTime;
        // use coroutine instead ou round
    }

    public IEnumerator getTimerCoroutine()
    {
        return timerCoroutine;
    }

    // Timer Coroutine
    private void UpdateTimer()
    {
        timerCoroutine = Utilities.ExecuteAfter(1f, () =>
        {
            mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(Mathf.Round(mGameTime));
            if (mGameTime > 0)
            {
                UpdateTimer();
            }
            else
                Death();

        });
        StartCoroutine(timerCoroutine);
    }

    private void ResetTimer()
    {
        mGameTime = 10;
        mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);
        UpdateTimer();
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
        gameOverPanel.SetActive(false);
        ResetTimer();

        Time.timeScale = 1;

        // Reset Character & Enemies
        GameManager.mInstance.mEnnemyManager.DestroyAllEnnemies();
        GameManager.mInstance.mEnnemyManager.SpawnEnnemies( 2, 4 );
    }

    // Set-up & Load Next Room
    public void NextRoom()
    {
        // Stop Timer
        StopCoroutine(timerCoroutine);
        mTimer.SetActive(false);

        // Load Animation
        RoomLoader rL = RoomLoader.GetComponent<RoomLoader>();
        rL.LoadNextRoom(1);

        // Load Room Features
        StartCoroutine(LoadRoom(rL.transitionTime));
    }

    public IEnumerator LoadRoom(float waitingTime)
    {
        // Wait
        yield return new WaitForSeconds(waitingTime);

        // Reset Timer
        ResetTimer();
        mTimer.SetActive(true);

        // Handle enemies spawn
        GameManager.mInstance.mEnnemyManager.DestroyAllEnnemies();
        GameManager.mInstance.mEnnemyManager.SpawnEnnemies( 2, 4 );
    }

}
