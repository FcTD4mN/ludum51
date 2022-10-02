using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
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
    private GameObject RoomLoader;

    // keep a copy of the executing coroutine (timer)
    private IEnumerator timerCoroutine;

    // Ref to CardManager
    private CardManager mCardManager;

    // Ref to main camera
    private GameObject mainCamera;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Retrieve main camera
        mainCamera = GameObject.Find("Main Camera");

        // Retrieve GameObjects reference
        RoomLoader = GameObject.Find("RoomLoader");
        mCardManager = GameObject.Find("CardManager").GetComponent<CardManager>();

        // Retrieve UI Elements
        cardCanvas = GameObject.Find("CardCanvas");
        canvas = GameObject.Find("Canvas");
        cardCanvas.SetActive(false);
        gameOverPanel = GameObject.Find("GameOverPanel").gameObject;
        gameOverPanel.SetActive(false);
        mIgPanel = GameObject.Find("InGamePanel").gameObject;

        // Init Timer
        mTimer = mIgPanel.transform.Find("Chrono").gameObject;
        mTimer.GetComponent<TextMeshProUGUI>().text = Utilities.FormatSecondsToMinuteAndSeconds(mGameTime);

        // Start Timer Coroutine
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

        // Reset Character & Enemies : TO-DO
    }

    // Current room finish
    public void FinishRoom()
    {
        // Stop Timer
        StopCoroutine(timerCoroutine);

        // Load Animation
        RoomLoader rL = RoomLoader.GetComponent<RoomLoader>();
        rL.FinishCurrentRoom();

        // Load Room Features
        StartCoroutine(FinishRoomCoroutine(rL.transitionTime / 2));
    }

    // Show Card Panel
    private void ShowCardChoices()
    {
        // Generate Cards
        Card[] cCards = mCardManager.SpawnCard();

        // Show panel
        cardCanvas.SetActive(true);

        // Create as many Prefabas as there is Cards
        int posX = -250;
        for (int i = 0; i < cCards.Length; i++)
        {
            GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Cards/Card" + cCards[i].getType() + "Template");
            GameObject card = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            card.transform.SetParent(cardCanvas.transform);

            RectTransform uiTransform = card.GetComponent<RectTransform>();
            uiTransform.anchoredPosition = new Vector2(posX, 0);
            posX += 250;

            Button btn = card.GetComponent<Button>();
            // Text cText = btn.GetComponent<Text>();
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = cCards[i].ToCardText();

            int iCopy = i;
            btn.onClick.AddListener(() => ChooseCard(iCopy));
        }

    }

    void ChooseCard(int whichCard)
    {
        // Equip the card
        mCardManager.EquipCard(whichCard);

        // Load + Set-up Next Room
        NextRoom();

        // Load Animation
        RoomLoader rL = RoomLoader.GetComponent<RoomLoader>();
        rL.LoadNextRoom();

        // Resume timer etc...
        StartCoroutine(NextRoomCoroutine(rL.transitionTime / 2));
    }

    void NextRoom()
    {
        currentRoom += 1;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 28, 0f, -10f);
    }

    // Coroutines :
    public IEnumerator FinishRoomCoroutine(float waitingTime)
    {
        // Hide IGCanvas
        canvas.SetActive(false);

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

        // Wait
        yield return new WaitForSeconds(waitingTime);

        // Before resume the game : Show Card choice
        ResetTimer();
    }

}
