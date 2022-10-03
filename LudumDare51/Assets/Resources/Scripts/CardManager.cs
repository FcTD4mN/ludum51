using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ludum51.Player;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour, ISaveable
{
    // Contain current deck
    private List<Card> mDeckOfCards;

    // Contain room cards choice
    private Card[] mCurrentChoice;
    private List<GameObject> mCurrentDeckUI;
    private int mNumberOfCards = 3;

    void OnEnable()
    {
        mCurrentDeckUI = new List<GameObject>();
        // Should load from file
        mDeckOfCards = new List<Card>();
    }

    private Card[] SpawnCard()
    {
        // Generate new cards
        mCurrentChoice = new Card[mNumberOfCards];

        for (int i = 0; i < mNumberOfCards; i++)
        {
            mCurrentChoice[i] = new Card(i + 1);
        }

        return mCurrentChoice;
    }

    public void EquipCard(int whichCard, Player player)
    {
        mDeckOfCards.Add(mCurrentChoice[whichCard]);
        player.pushCard(mCurrentChoice[whichCard]);
    }

    // UI Side
    public void CreateCardUI( GameObject canvas, LevelManager levelManager, bool newChapter )
    {
        // Delete from UI first
        RemoveCardsUI();

        Card[] cCards = null;
        if( newChapter )
        {
            // TODO: Generate Special Cards
            cCards = SpawnCard();
        }
        else
        {
            // Generate Cards
            cCards = SpawnCard();
        }

        // Create as many Prefabas as there is Cards
        int posX = -250;
        for (int i = 0; i < cCards.Length; i++)
        {
            GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Cards/CardTemplate");
            GameObject card = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            card.transform.SetParent(canvas.transform);

            RectTransform uiTransform = card.GetComponent<RectTransform>();
            uiTransform.anchoredPosition = new Vector2(posX, 0);
            posX += 250;

            Button btn = card.GetComponent<Button>();
            // Text cText = btn.GetComponent<Text>();
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = cCards[i].ToCardText();

            int iCopy = i;
            btn.onClick.AddListener(() => levelManager.ChooseCard(iCopy));
            mCurrentDeckUI.Add(card);
        }
    }

    private void RemoveCardsUI()
    {
        foreach (GameObject item in mCurrentDeckUI)
        {
            GameObject.Destroy(item);
        }
    }

    public List<Card> getDeckOfCards()
    {
        return mDeckOfCards;
    }

    // SAVE DATA - JSON - Save current deck of cards
    public void PopulateSaveData(SaveData dataSet)
    {
        // Saving cards
        dataSet.mNumberOfCards = mDeckOfCards.Count;

        foreach (Card card in mDeckOfCards)
        {
            card.PopulateSaveData(dataSet);
        }
    }

    public void LoadFromSaveData(SaveData dataSet)
    {
        foreach (Card card in mDeckOfCards)
        {
            card.LoadFromSaveData(dataSet);
        }

        // for (int i = 0; i < dataSet.mNumberOfCards; i++)
        // {
        //     Card cCard = new Card(i + 1);
        //     cCard.LoadFromSaveData(dataSet);
        // }

    }
}
