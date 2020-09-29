using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class HorizontalItemManager : MonoBehaviour
{
    public HorizontalItem[] items;
    private int selectedItem;
    private int previouslySelectedItem;

    private HorizontalLayoutGroup xLayoutGroup;
    private RectTransform rect;
    public static HorizontalItemManager Instance;

    public string selectedItemIndex
    {
        get
        {
            return items[selectedItem].indexLabel.text;
        }
    }

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
        rect = GetComponent<RectTransform>();
        xLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        selectedItem = 0;
        previouslySelectedItem = 0;
        SeasonDownloader.Instance.NewSeasonLoaded += OnSeasonUpdated;
    }


    void OnSeasonUpdated()
    {
        UpdateSelection();
    }

    void UpdateSelection()
    {
        float delay = 0f;
        bool pageUp = false;
        bool pageDn = false;

        if (previouslySelectedItem % 5 == 4 && selectedItem % 5 == 0)
        {

            if (!UIManager.Instance.canPageUp)
            {
                return;
            }
            PageUpOut();
            selectedItem = 0;
            pageUp = true;
            delay = .25f;
        }
        else if (previouslySelectedItem == 0 && selectedItem == -1)
        {
            if (!UIManager.Instance.canPageDown)
            {
                return;
            }
            PageDownOut();
            selectedItem = 4;
            pageDn = true;
            delay = .25f;
        }
        Wrj.Utils.Delay(delay, () =>
        {
            if (pageUp)
            {
                UIManager.Instance.currentPage = UIManager.Instance.currentPage + 1;
                Select();
                PageUpIn();
            }
            else if (pageDn)
            {
                UIManager.Instance.currentPage = UIManager.Instance.currentPage - 1;
                Select();
                PageDownIn();
            }
            else
            {
                Select();
            }
        });
    }

    private void Select()
    {
        selectedItem = Mathf.Clamp(selectedItem, 0, items.Length - 1);
        Debug.Log(selectedItem);
        for (int i = 0; i < items.Length; i++)
        {
            if (i == selectedItem)
            {
                items[i].Select(true);
            }
            else
            {
                items[i].Select(false);
            }
        }
    }

    public void ForcePageUp()
    {
        if (!UIManager.Instance.canPageUp)
        {
            return;
        }
        PageUpOut();
        Wrj.Utils.Delay(.25f, () =>
        {
            UIManager.Instance.currentPage = UIManager.Instance.currentPage + 1;
            selectedItem = 0;
            Select();
            PageUpIn();
        });
    }
    public void ForcePageDown()
    {
        if (!UIManager.Instance.canPageDown)
        {
            return;
        }
        PageDownOut();
        Wrj.Utils.Delay(.25f, () =>
        {
            UIManager.Instance.currentPage = UIManager.Instance.currentPage - 1;
            selectedItem = 4;
            Select();
            PageDownIn();
        });
    }

    public void SelectUp()
    {
        if (selectedItem + 1 < items.Length && items[selectedItem + 1].level == null)
        {
            return;
        }
        previouslySelectedItem = selectedItem;
        selectedItem++;
        UpdateSelection();
    }
    public void SelectDown()
    {
        if (selectedItem == 0 && !UIManager.Instance.canPageDown) return;
        previouslySelectedItem = selectedItem;
        selectedItem--;
        UpdateSelection();
    }
    public void SelectByIndex(int index)
    {
        previouslySelectedItem = selectedItem;
        selectedItem = index;
        Select();
    }

    public void PageUpOut()
    {
        Wrj.Utils.MapToCurve.EaseIn.ManipulateFloat(ModifyLayoutGroupPosition, 0f, -3100f, .25f);
    }
    public void PageUpIn()
    {
        Wrj.Utils.MapToCurve.EaseIn.ManipulateFloat(ModifyLayoutGroupPosition, 3100f, 0f, .25f);
    }
    public void PageDownOut()
    {
        Wrj.Utils.MapToCurve.EaseIn.ManipulateFloat(ModifyLayoutGroupPosition, 0, 3100f, .25f);
    }
    public void PageDownIn()
    {
        Wrj.Utils.MapToCurve.EaseIn.ManipulateFloat(ModifyLayoutGroupPosition, -3100f, 0f, .25f);
    }

    private void ModifyLayoutGroupPosition(float val)
    {
        xLayoutGroup.padding.left = Mathf.RoundToInt(val);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }
}
