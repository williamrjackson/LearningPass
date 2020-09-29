using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PageButtonManager : MonoBehaviour
{
    public enum Side { Left, Right };
    public Side side;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.PageChange += ManageState;
        ManageState();
    }

    // Update is called once per frame
    void ManageState()
    {
        if (side == Side.Left)
        {
            if (UIManager.Instance.canPageDown)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
        else
        { 
            if (UIManager.Instance.canPageUp)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}
