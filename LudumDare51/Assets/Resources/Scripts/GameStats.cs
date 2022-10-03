using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStat
{
    public int mLevel;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string jsonContent)
    {
        JsonUtility.FromJsonOverwrite(jsonContent, this);
    }
}
