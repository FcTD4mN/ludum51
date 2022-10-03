using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Ludum51.Player;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum eDeathCause
    {
        kDed,
        kTimeOut
    }

    // Timer for every 10 seconds
    private float mGameTime = 10;

    // Current Room Number
    private int currentRoom = 1;

    // UI Elements
    private GameObject canvas;
    private GameObject cardCanvas;
    private GameObject gameOverPanel;
    private GameObject mIgPanel;
    private GameObject mTimer;

    // Reference to Room Loader (Animation)
    private GameObject mRoomLoader;
    public bool mCardSelection;

    // keep a copy of the executing coroutine (timer)
    private IEnumerator timerCoroutine;

    // Ref to CardManager
    private CardManager mCardManager;

    // Ref to main camera
    private GameObject mainCamera;
    //Player stuff
    private GameObject mPlayer;

    // Start is called before the first frame update
    public void Initialize()
    {
        // Load animation
        mRoomLoader = GameObject.Find("RoomLoader");
        RoomLoader rL = mRoomLoader.GetComponent<RoomLoader>();
        rL.LoadNextRoom();

        // Retrieve main camera
        mainCamera = GameObject.Find("Main Camera");
        // Retrieve main camera
        mPlayer = GameObject.Find("Player");

        // Retrieve Card reference
        mCardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        mCardSelection = false;

        // Retrieve UI Elements
        cardCanvas = GameObject.Find("CardCanvas");
        canvas = GameObject.Find("Canvas");
        gameOverPanel = GameObject.Find("GameOverPanel").gameObject;
        mIgPanel = GameObject.Find("InGamePanel").gameObject;
        cardCanvas.SetActive(false);
        gameOverPanel.SetActive(false);

        // Init Timer
        mTimer = mIgPanel.transform.Find("Chrono").gameObject;
        mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);

        // Start Timer & Build room - One second after loading scene
        StartCoroutine(Utilities.ExecuteAfter(1f, () =>
        {
            mGameTime = 10;
            mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(Mathf.Round(mGameTime));
            UpdateTimer();
            BuildRoom(currentRoom, true);
        }));

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
                Death(eDeathCause.kTimeOut);

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
    public void Death(eDeathCause cause)
    {
        // Pause le jeu
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }


    private void BuildRoom(int whichLevel, bool reset)
    {
        // int ennemyBasicCount = 4;
        // int ennemyShooterCount = 2;
        int ennemyBasicCount = Math.Max(whichLevel + 2, 0);
        int ennemyShooterCount = Math.Max(whichLevel - 4, 0);

        // Projectiles
        GameManager.mInstance.mProjectileManager.ClearAllProjectiles();

        // Player
        Weapon playerWeapon = GameManager.mInstance.mThePlayer.GetComponent<Shooter>().GetWeapon();
        playerWeapon.Reset(); // This will just reload the gun, not reset the stats

        if(reset)
            GameManager.mInstance.mThePlayer.Reset(); // This will reset all stats

        GameObject area = GameObject.Find("SpawnAreas/SpawnPlayer/" + whichLevel + "-Player");
        Debug.Assert(area != null);
        GameManager.mInstance.mThePlayer.transform.position = new Vector3(area.transform.position.x, area.transform.position.y, -1);

        // Ennemies
        GameManager.mInstance.mEnnemyManager.DestroyAllEnnemies();

        if( currentRoom % 10 == 0)
        {
            GameManager.mInstance.mEnnemyManager.SpawnBoss( currentRoom / 10 );
        }
        else
        {
            GameManager.mInstance.mEnnemyManager.SpawnEnnemies(whichLevel, ennemyBasicCount, ennemyShooterCount);
        }
    }

    public void Retry()
    {
        // Reset timer and hide death screen
        gameOverPanel.SetActive(false);
        ResetTimer();

        Time.timeScale = 1;
        BuildRoom(currentRoom, true);
    }

    // Current room finish
    public void FinishRoom()
    {
        // Stop Timer
        StopCoroutine(timerCoroutine);

        // Load Animation
        RoomLoader rL = mRoomLoader.GetComponent<RoomLoader>();
        rL.FinishCurrentRoom();

        // Load Room Features
        StartCoroutine(FinishRoomCoroutine(rL.transitionTime / 2));
    }

    // Show Card Panel
    private void ShowCardChoices()
    {
        // Show panel
        cardCanvas.SetActive(true);

        // Generate Cards
        mCardManager.CreateCardUI(cardCanvas, this);
    }

    public void ChooseCard(int whichCard)
    {
        // Equip the card
        mCardManager.EquipCard(whichCard, mPlayer.GetComponent<Player>());

        // Load + Set-up Next Room
        NextRoom();

        // Load Animation
        RoomLoader rL = mRoomLoader.GetComponent<RoomLoader>();
        rL.LoadNextRoom();

        // Resume timer etc...
        StartCoroutine(NextRoomCoroutine(rL.transitionTime / 2));
    }

    void NextRoom()
    {
        currentRoom += 1;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 28, 0f, -10f);
    }

    private void FinishChapter()
    {
        // Load menu scene - TODO
        // SceneManager.LoadScene("ChapterSelection");
    }

    public void FinishRun()
    {
        // Level +1
        GameStat gStat = new GameStat();
        gStat.mLevel = 1;
        if (FileManager.LoadFromFile("SaveGlobalData.json", out var gameData))
            gStat.LoadFromJson(gameData);

        gStat.mLevel += 1;
        if (FileManager.WriteToFile("SaveGlobalData.json", gStat.ToJson()))
            Debug.Log( "NoWarning" );
    }

    // Coroutines :
    public IEnumerator FinishRoomCoroutine(float waitingTime)
    {
        // Clean Monster & Projectiles + Hide IGCanvas
        canvas.SetActive(false);
        GameManager.mInstance.mEnnemyManager.DestroyAllEnnemies();
        GameManager.mInstance.mProjectileManager.ClearAllProjectiles();
        mCardSelection = true;

        // Wait
        yield return new WaitForSeconds(waitingTime);

        // Before resume the game : Show Card choice
        ShowCardChoices();
    }

    public IEnumerator NextRoomCoroutine(float waitingTime)
    {
        cardCanvas.SetActive(false);
        mGameTime = 10;
        mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);
        canvas.SetActive(true);
        mCardSelection = false;

        // Wait
        yield return new WaitForSeconds(waitingTime);

        // Before resume the game : Show Card choice
        ResetTimer();

        // Handle enemies spawn
        BuildRoom(currentRoom, false);
    }

}
