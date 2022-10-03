using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ludum51.Player;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour, ISaveable
{
    private CardList mCardListGenerator;
    // Contain current deck
    private List<Card> mDeckOfCards;

    // Contain room cards choice
    private Card[] mCurrentChoice;
    private List<GameObject> mCurrentDeckUI;
    private int mNumberOfCards = 3;

    public void Initialize()
    {
        mCurrentDeckUI = new List<GameObject>();
        // Should load from file
        mDeckOfCards = new List<Card>();
        mCardListGenerator = new CardList();
    }

    private Card[] SpawnCard(bool special)
    {
        // Generate new cards
        mCurrentChoice = new Card[mNumberOfCards];

        for (int i = 0; i < mNumberOfCards; i++)
        {
            mCurrentChoice[i] = special ? mCardListGenerator.GetRandomSpecialCard() : mCardListGenerator.GetRandomNormalCard();
        }

        return mCurrentChoice;
    }

    public void EquipCard(int whichCard, Player player)
    {
        mDeckOfCards.Add(mCurrentChoice[whichCard]);
        player.pushCard(mCurrentChoice[whichCard]);
    }

    public void EquipDeckOfCards(List<Card> deckOfCards, Player player)
    {
        foreach (Card card in deckOfCards)
        {
            player.pushCard(card);
        }
    }

    // UI Side
    public void CreateCardUI(GameObject canvas, LevelManager levelManager, bool newChapter)
    {
        // Delete from UI first
        RemoveCardsUI();

        Card[] cCards = SpawnCard(newChapter);

        // Create as many Prefabas as there is Cards
        int posX = -325;
        for (int i = 0; i < cCards.Length; i++)
        {
            GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Cards/CardTemplate");
            GameObject card = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            card.transform.SetParent(canvas.transform);

            RectTransform uiTransform = card.GetComponent<RectTransform>();
            uiTransform.anchoredPosition = new Vector2(posX, 0);
            posX += 325;

            // Change color
            GameObject bgImg = card.transform.Find("Image").gameObject;
            Image img = bgImg.GetComponent<Image>();

            switch (cCards[i].mPowerUpCategory)
            {
                case (PowerUpCategory.Health):
                    img.color = new Color32(242, 0, 0, 100);
                    break;
                case (PowerUpCategory.Speed):
                    img.color = new Color32(90, 151, 250, 100);
                    break;
                case (PowerUpCategory.WeaponSpeed):
                    img.color = new Color32(196, 90, 250, 100);
                    break;
                case (PowerUpCategory.Projectile):
                    img.color = new Color32(250, 196, 90, 100);
                    break;
                case (PowerUpCategory.Cooldown):
                    img.color = new Color32(133, 133, 133, 100);
                    break;
                case (PowerUpCategory.Zone):
                    img.color = new Color32(36, 0, 218, 100);
                    break;
                case (PowerUpCategory.Damage):
                    img.color = new Color32(218, 127, 0, 100);
                    break;
                case (PowerUpCategory.Pierce):
                    img.color = new Color32(218, 15, 192, 100);
                    break;
            }


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
        foreach (SaveData.CardData item in dataSet.mDeckOfCards)
        {
            Card cCard = new Card();
            cCard.LoadFromSaveData(item);
            mDeckOfCards.Add(cCard);
        }

        // Equip the player after loading cards
        EquipDeckOfCards(mDeckOfCards, GameManager.mInstance.mPlayerObject.GetComponent<Player>());
    }
}
