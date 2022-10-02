using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludum51.Player.Stat;

public class Card
{
    // default ideas
    public StatModType mType;
    public PowerUpCategory mPowerUpCategory;
    public float mPoints;

    public Card()
    {
        // Card factory cheap
        int rnd = UnityEngine.Random.Range(1, Enum.GetNames(typeof(PowerUpCategory)).Length);
        mPowerUpCategory = (PowerUpCategory)rnd;

        rnd = UnityEngine.Random.Range(1, Enum.GetNames(typeof(StatModType)).Length);
        mType = (StatModType)rnd;

        // Get a random type
        int rndType = GetRandomType();

        mPoints = Mathf.Round(UnityEngine.Random.Range(0.05f, 0.2f) * 100f) / 100f;
        if (rndType == 1)
        {
            mType = StatModType.Flat;
            mPoints = Mathf.Round(UnityEngine.Random.Range(0, 20) * 100f) / 100f;
        }
        else if (rndType == 2)
            mType = StatModType.PercentAdd;
        else if (rndType == 3)
            mType = StatModType.PercentMult;

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
        return "Type : " + mPowerUpCategory + " / Points : " + mPoints;
    }

    public String ToCardText()
    {
        return mPowerUpCategory + "\r\n+" + mType + " / " + mPoints;
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
    Damage = 7
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