using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ludum51.Player;
using UnityEngine;

public class CardManager : MonoBehaviour, ISaveable
{
    // Contain current deck
    private List<Card> mDeckOfCards;

    // Contain room cards choice
    private Card[] mCurrentChoice;
    private int mNumberOfCards = 3;

    void OnEnable()
    {
        // Should load from file 
        mDeckOfCards = new List<Card>();
    }

    public Card[] SpawnCard()
    {
        mCurrentChoice = new Card[mNumberOfCards];

        for (int i = 0; i < mNumberOfCards; i++)
        {
            mCurrentChoice[i] = new Card(i + 1);
            Debug.Log(mCurrentChoice[i].ToString());
        }

        return mCurrentChoice;
    }

    public void EquipCard(int whichCard, Player player)
    {
        mDeckOfCards.Add(mCurrentChoice[whichCard]);
        player.pushCard(mCurrentChoice[whichCard]);
    }

    private void ReplaceCard(int whichCard)
    {

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
