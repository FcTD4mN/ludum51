using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // default ideas
    private Categories mCategorie;
    private Type mType;
    private float mPoints;

    public Card()
    {
        // Card factory cheap
        int rnd = UnityEngine.Random.Range(1, Enum.GetNames(typeof(Type)).Length);
        mType = (Type)rnd;
        mCategorie = (Categories)UnityEngine.Random.Range(1, 2);

        switch (mType)
        {
            case Type.Life:
                // Points between 10 and 100
                mPoints = Mathf.Round(UnityEngine.Random.Range(10, 100) * 100f) / 100f;
                break;
            case Type.Projectile:
                // Points between 1 and 5
                mPoints = Mathf.Round(UnityEngine.Random.Range(1, 5) * 100f) / 100f;
                break;
            default:
                // Points between 0.1 and 10%
                mPoints = Mathf.Round(UnityEngine.Random.Range(0.1f, 10f) * 100f) / 100f;
                break;
        }
    }

    public Type getType()
    {
        return mType;
    }

    public float getPoints()
    {
        return mPoints;
    }

    public override String ToString()
    {
        return "Categories : " + mCategorie + " / Type : " + mType + " / Points : " + mPoints;
    }

    public String ToCardText()
    {
        if (mType == Type.Life)
            return mType + "\r\n+" + mPoints;
        else if (mType == Type.Projectile)
            return mType + "\r\n+" + mPoints;
        else
            return mType + "\r\n+" + mPoints + "%";
    }

    // ENUMS
    public enum Categories
    {
        Weapon = 1,
        Character = 2
    }

    public enum Type
    {
        Life = 1,
        Speed = 2,
        Projectile = 3,
        Cooldown = 4,
        Zone = 5,
        Damage = 6
    }
}

