using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public string xmlPath;
    public SeasonRaw season;
    public TMPro.TMP_Text titleText;
    public TMPro.TMP_Text levelLabel;
    public Image heroImage;
    public UnityAction PageChange;

    private int pages = 0;
    private int _currentPage = 0;
    public bool canPageUp
    {
        get
        {
            Debug.Log("Can Page = " + (currentPage < pages) + " || " + currentPage + " vs " + pages);
            return (currentPage < pages);
        }
    }
    public bool canPageDown
    {
        get
        {
            Debug.Log("Can Page = " + (currentPage > 0) + " || " + currentPage);
            return (currentPage > 0);
        }
    }
    public int currentPage
    {
        set
        {
            if (value <= pages && value > -1)
            {
                _currentPage = value;
                if (PageChange != null)
                {
                    PageChange();
                }
                PopulateLevels();
            }
        }
        get => _currentPage;
    }

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        SeasonDownloader.Instance.NewSeasonLoaded += PopulateLevels;
        SeasonDownloader.Instance.LoadCurrentSeason(SeasonDownloader.Instance.pollUrl);
    }

    public void PopulateLevels()
    {
        foreach (HorizontalItem slot in HorizontalItemManager.Instance.items)
        {
            slot.SetItem(null, 0);
        }

        pages = Mathf.CeilToInt(SeasonDownloader.Instance.season.levels.Count / 5);
        int offset = currentPage * 5;

        for (int i = currentPage * 5; (i < SeasonDownloader.Instance.season.levels.Count) && (i - offset < HorizontalItemManager.Instance.items.Length); i++)
        {
            HorizontalItemManager.Instance.items[i - offset].SetItem(SeasonDownloader.Instance.season.levels[i], i + 1);
        }
    }

    public void SetItem(SeasonDownloader.Level level)
    {
        heroImage.sprite = level.heroImage;
        titleText.text = level.title;
        levelLabel.text = "Level " + HorizontalItemManager.Instance.selectedItemIndex;
    }

}
