using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludum51.Player.Stat;

public class Card : ISaveable
{
    // default ideas
    public int mId;
    public StatModType mType;
    public PowerUpCategory mPowerUpCategory;
    public float mPoints;

    public Card(int id)
    {
        mId = id;
        // Card factory cheap
        // int rnd = UnityEngine.Random.Range(1, Enum.GetNames(typeof(PowerUpCategory)).Length);
        // mPowerUpCategory = (PowerUpCategory)rnd;

        // rnd = UnityEngine.Random.Range(1, Enum.GetNames(typeof(StatModType)).Length);
        // mType = (StatModType)rnd;

        // // Get a random type
        // int rndType = GetRandomType();

        // mPoints = Mathf.Round(UnityEngine.Random.Range(0.05f, 0.2f) * 100f) / 100f;
        // if (rndType == 1)
        // {
        //     mType = StatModType.Flat;
        //     mPoints = Mathf.Round(UnityEngine.Random.Range(0, 20) * 100f) / 100f;
        // }
        // else if (rndType == 2)
        //     mType = StatModType.PercentAdd;
        // else if (rndType == 3)
        //     mType = StatModType.PercentMult;

        // flat 0 - 20
        // percentAdd 0.05 - 0.2
        // percentMulti 0.05 - 0.2
    }

    public int GetRandomType()
    {
        // 50% chance of 1, 30% - 2, 20% - 3
        List<Tuplez> probabilities = new List<Tuplez>();
        probabilities.Add(new Tuplez(0.20f, 3));
        probabilities.Add(new Tuplez(0.50f, 2));
        probabilities.Add(new Tuplez(1.00f, 1));

        double realRoll = UnityEngine.Random.Range(0f, 1f); // random number

        foreach (var proba in probabilities)
        {
            if (proba.probability > realRoll)
            {
                return proba.value;
            }
        }

        return 1;
    }

    public PowerUpCategory getType()
    {
        return mPowerUpCategory;
    }

    public float getPoints()
    {
        return mPoints;
    }

    public override String ToString()
    {
        return "Power Up : " + mPowerUpCategory + " / Type : " + mType + " / Points : " + mPoints;
    }

    public String ToCardText()
    {
        return mPowerUpCategory + "\r\n+" + mType + " / " + mPoints;
    }

    // SAVE DATA - TOJSON
    public void PopulateSaveData(SaveData dataSet)
    {
        // Saving Card Game Data
        SaveData.CardData cardData = new SaveData.CardData();
        cardData.id = mId;
        cardData.value = mPoints;
        cardData.powerUpCategory = (int)mPowerUpCategory;
        cardData.type = (int)mType;

        // Push to DeckOfCards
        dataSet.mDeckOfCards.Add(cardData);
    }

    public void LoadFromSaveData(SaveData dataSet)
    {
        // To Upgrade maybe
        foreach (SaveData.CardData cardData in dataSet.mDeckOfCards)
        {
            if (cardData.id == mId)
            {

                mPowerUpCategory = (PowerUpCategory)cardData.powerUpCategory;
                mType = (StatModType)cardData.type;
                mPoints = cardData.value;
                break;
            }
        }

        // Prévoir une fonction qui delete si il trouve pas l'id ?
    }

}

public enum PowerUpCategory
{
    Health = 1,
    Speed = 2,
    WeaponSpeed = 3,
    Projectile = 4,
    Cooldown = 5,
    Zone = 6,
    Damage = 7,
    Pierce = 8
}

class Tuplez
{

    public float probability;
    public int value;

    public Tuplez(float probability, int value)
    {
        this.probability = probability;
        this.value = value;
    }

    public override string ToString()
    {
        return probability + "/" + value;
    }
}





class CardList
{
    private List<Card> mCards;
    private List<Card> mCardsSpecial;
    private int mIDCounter = 1;

    private void CreateCard( bool normal, PowerUpCategory cat, StatModType type, float value )
    {
        Card card = new Card( mIDCounter );
        mIDCounter += 1;
        card.mPowerUpCategory = cat;
        card.mType = type;
        card.mPoints = value;

        if( normal )
            mCards.Add( card );
        else
            mCardsSpecial.Add( card );

    }

    public CardList()
    {
        mCards          = new List<Card>();
        mCardsSpecial   = new List<Card>();

        // NORMAL ===============================================
        CreateCard( true, PowerUpCategory.Health, StatModType.Flat, 50 );
        CreateCard( true, PowerUpCategory.Health, StatModType.PercentMult, 1.2f -1 );
        CreateCard( true, PowerUpCategory.Damage, StatModType.PercentMult, 1.2f -1 );
        CreateCard( true, PowerUpCategory.Damage, StatModType.Flat, 5 );
        CreateCard( true, PowerUpCategory.WeaponSpeed, StatModType.Flat, 1 );
        CreateCard( true, PowerUpCategory.WeaponSpeed, StatModType.PercentMult, 1.5f -1 );
        CreateCard( true, PowerUpCategory.Cooldown, StatModType.PercentMult, 0.5f -1 );
        CreateCard( true, PowerUpCategory.Cooldown, StatModType.Flat, -1 );
        CreateCard( true, PowerUpCategory.Speed, StatModType.Flat, 1 );
        CreateCard( true, PowerUpCategory.Speed, StatModType.PercentMult, 0.1f -1 );
        CreateCard( true, PowerUpCategory.Zone, StatModType.Flat, 1 );
        CreateCard( true, PowerUpCategory.Zone, StatModType.PercentMult, 1.2f -1 );



        // SPECIAL ===============================================
        CreateCard( false, PowerUpCategory.Health, StatModType.Flat, 500 );
        CreateCard( false, PowerUpCategory.Health, StatModType.PercentMult, 2 -1 );
        CreateCard( false, PowerUpCategory.Damage, StatModType.PercentMult, 2 -1 );
        CreateCard( false, PowerUpCategory.Damage, StatModType.Flat, 50 );
        CreateCard( false, PowerUpCategory.Projectile, StatModType.Flat, 10 );
        CreateCard( false, PowerUpCategory.Projectile, StatModType.PercentMult, 2 -1 );
        CreateCard( false, PowerUpCategory.WeaponSpeed, StatModType.Flat, 2 );
        CreateCard( false, PowerUpCategory.WeaponSpeed, StatModType.PercentMult, 2 -1 );
        CreateCard( false, PowerUpCategory.Zone, StatModType.Flat, 5 );
        CreateCard( false, PowerUpCategory.Zone, StatModType.PercentMult, 2f -1 );
        CreateCard( false, PowerUpCategory.Pierce, StatModType.Flat, 1 );
    }


    public Card GetRandomNormalCard()
    {
        int random = UnityEngine.Random.Range( 0, mCards.Count );
        return  mCards[random];
    }

    public Card GetRandomSpecialCard()
    {
        int random = UnityEngine.Random.Range( 0, mCardsSpecial.Count );
        return  mCardsSpecial[random];
    }
}