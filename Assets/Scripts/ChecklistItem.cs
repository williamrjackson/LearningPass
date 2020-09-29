using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ChecklistItem : MonoBehaviour
{
    [SerializeField]
    private Image checkImage = null;
    [SerializeField]
    private TMP_Text challengeText = null;
    [SerializeField]
    private Color completedColor = Color.white;
    [SerializeField]
    private Color notCompletedColor = Color.gray;
    private string id;

    public void SetCompletionState(bool completed)
    {
        if (completed)
        {
            checkImage.enabled = true;
            challengeText.color = completedColor;
        }
        else
        {
            checkImage.enabled = false;
            challengeText.color = notCompletedColor;
        }
    }

    public void SetText(string text)
    {
        challengeText.text = text;
    }
}
