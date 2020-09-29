using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class HorizontalItem : MonoBehaviour
{
    public int index;
    public Image selectionIndicator;
    public TMPro.TMP_Text indexLabel;
    public Image icon;
    public SeasonDownloader.Level level;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Select(bool selected)
    {
        selectionIndicator.enabled = selected;
        if (selected)
        {
            UIManager.Instance.SetItem(level);
            ChecklistManager.Instance.ClearItems();
            foreach (SeasonDownloader.Challenge challenge in level.challenges)
            {
                ChecklistManager.Instance.AddItem(challenge.text, challenge.isAchieved);
            }
        }
    }

    public void SetItem(SeasonDownloader.Level levelToSet, int index)
    {
        if (levelToSet == null)
        {
            indexLabel.text = "";
            level = null;
            icon.enabled = false;
            //button.interactable = false;
            return;
        }
        icon.enabled = true;
        indexLabel.text = index.ToString();
        level = levelToSet;
        //button.interactable = true;
        icon.sprite = level.icon;
    }

    public void OnClick()
    {
        HorizontalItemManager.Instance.SelectByIndex(index);
    }
}
