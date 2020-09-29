using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecklistManager : MonoBehaviour
{
    public ChecklistItem itemPrototype;
    private List<ChecklistItem> items = new List<ChecklistItem>();

    public static ChecklistManager Instance;

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
    public void ClearItems()
    {
        foreach (ChecklistItem item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();
    }
    public void AddItem(string text, bool isCompleted)
    {
        ChecklistItem newItem = Instantiate(itemPrototype.gameObject, itemPrototype.transform.parent).GetComponent<ChecklistItem>();
        newItem.gameObject.SetActive(true);
        newItem.SetCompletionState(isCompleted);
        newItem.SetText(text);
        items.Add(newItem);
    }

}
