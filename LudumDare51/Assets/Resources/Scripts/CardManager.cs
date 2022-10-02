using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
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
            mCurrentChoice[i] = new Card();
            Debug.Log(mCurrentChoice[i].ToString());
        }

        return mCurrentChoice;
    }

    public void EquipCard(int whichCard)
    {
        mDeckOfCards.Add(mCurrentChoice[whichCard]);
    }

    private void ReplaceCard(int whichCard)
    {

    }

    public List<Card> getDeckOfCards()
    {
        return mDeckOfCards;
    }
}
