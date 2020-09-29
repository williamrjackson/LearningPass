using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class SeasonDownloader : MonoBehaviour
{
    public string pollUrl = "https://learningpass.000webhostapp.com/LearningPass.json";
    public UnityAction NewSeasonLoaded;
    public Season season;

    private SeasonRaw seasonRaw;

    public enum SeasonImageType { Icon, Hero }
    public static SeasonDownloader Instance;

    private int imageDownloadCounter = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multipble SeasonDownloaders instantiated.");
            Destroy(this);
        }
    }

    public Coroutine pollRoutine;
    public void StartPolling()
    {
        pollRoutine = StartCoroutine(Poll());
    }
    public void StopPolling()
    {
        if (pollRoutine != null)
        {
            StopCoroutine(pollRoutine);
            pollRoutine = null;
        }
    }

    public void LoadCurrentSeason(string url)
    {
        StartCoroutine(GetSeasonData(url));
    }

    private IEnumerator GetSprite(string url, Level level, SeasonImageType type) 
    {
        imageDownloadCounter++;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(url + ": " + www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (type == SeasonImageType.Hero)
            {
                level.heroImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
            else if (type == SeasonImageType.Icon)
            {
                level.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        }
        imageDownloadCounter--;
    }

    private IEnumerator GetSeasonData(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(url + ": " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            seasonRaw = JsonUtility.FromJson<SeasonRaw>(json);

            if (seasonRaw != null)
            {
                season = new Season();
                foreach (LevelRaw rawLevel in seasonRaw.levels)
                {
                    Level newLevel = new Level();
                    newLevel.title = rawLevel.title;
                    StartCoroutine(GetSprite(rawLevel.iconPath, newLevel, SeasonImageType.Icon));
                    StartCoroutine(GetSprite(rawLevel.heroPath, newLevel, SeasonImageType.Hero));

                    foreach (ChallengeRaw rawChallenge in rawLevel.challenges)
                    {
                        Challenge newChallenge = new Challenge();
                        newChallenge.text = rawChallenge.text;
                        newChallenge.isAchieved = rawChallenge.isAchieved;
                        newLevel.challenges.Add(newChallenge);
                    }

                    season.levels.Add(newLevel);
                }
                // Wait until all images are downloaded before alerting to the new season download.
                while (imageDownloadCounter > 0)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (NewSeasonLoaded != null)
                {
                    NewSeasonLoaded();
                }
            }
        }
    }

    private IEnumerator Poll()
    {
        yield return new WaitForSeconds(5f);
    }

    public class Season
    {
        public List<Level> levels = new List<Level>();
    }
    public class Level
    {
        public string title;
        public Sprite icon;
        public Sprite heroImage;
        public List<Challenge> challenges = new List<Challenge>();
    }
    public class Challenge
    {
        public string text;
        public bool isAchieved;
    }

    // JSON Serializable
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
}
