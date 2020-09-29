using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class SeasonRaw
{ 
    public List<LevelRaw> levels = new List<LevelRaw>();
}
[System.Serializable]
public class LevelRaw
{
    public string title;
    public string iconPath;
    public string heroPath;
    public List<ChallengeRaw> challenges = new List<ChallengeRaw>();
}
[System.Serializable]
public class ChallengeRaw
{
    public string text;
    public bool isAchieved;
}
