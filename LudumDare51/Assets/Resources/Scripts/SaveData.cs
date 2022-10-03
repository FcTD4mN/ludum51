using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Card Structure - For Saving DeckOfCards
    [System.Serializable]
    public struct CardData
    {
        public string id;
        public int type;
        public int powerUpCategory;
        public float value;
    }

    public List<CardData> mDeckOfCards = new List<CardData>();

    public int mNumberOfCards;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string jsonContent)
    {
        JsonUtility.FromJsonOverwrite(jsonContent, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData dataSet);
    void LoadFromSaveData(SaveData dataSet);
}
