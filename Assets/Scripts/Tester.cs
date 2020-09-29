using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public AudioClip audio;
    private Soundtrack soundtrack;
    private HorizontalItemManager horizontalItemManager;
    private ChecklistManager checklist;
   
    void Start()
    {
        soundtrack = FindObjectOfType<Soundtrack>();
        checklist = FindObjectOfType<ChecklistManager>();
        horizontalItemManager = FindObjectOfType<HorizontalItemManager>();
        soundtrack.Play(audio);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizontalItemManager.SelectUp();
            //horizontalItemManager.PageUpOut();
            //Wrj.Utils.Delay(.25f, () => horizontalItemManager.PageUpIn());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            horizontalItemManager.SelectDown();
            //horizontalItemManager.PageDownOut();
            //Wrj.Utils.Delay(.25f, () => horizontalItemManager.PageDownIn());
        }
    }
}
