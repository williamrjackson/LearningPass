using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Wrj;

public class FillBar : MonoBehaviour
{
    [Range(0, 99)]
    public int value = 50;
    public Image fill;
    public TMPro.TMP_Text percentageReadout;
    public Color activityColor;
    public Color restColor;

    private int cachedVal = 0;
    private Utils.MapToCurve.Manipulation manipulation;
    void Start()
    {
        cachedVal = value;
    }

    void Update()
    {
        if (value != cachedVal)
        {
            float fvalue = value.Remap(0, 100, 0f, 1f);
            float fcachedVal = cachedVal.Remap(0, 100, 0f, 1f);
            cachedVal = value;
            if (manipulation != null)
            {
                manipulation.Stop();
            }
            percentageReadout.text = value.ToString() + "%";
            manipulation = Utils.MapToCurve.Ease.ManipulateFloat(SetFill, fcachedVal, fvalue, 1f);
            Utils.MapToCurve.Linear.ChangeColor(fill.transform, activityColor, 1f);
            Utils.MapToCurve.Linear.ChangeColor(percentageReadout.transform, activityColor, 1f);
            Utils.Delay(1f, () => Utils.MapToCurve.Linear.ChangeColor(fill.transform, restColor, 1.5f));
            Utils.Delay(1f, () => Utils.MapToCurve.Linear.ChangeColor(percentageReadout.transform, Color.white, 1.5f));
        }
    }
    void SetFill(float fillAmount) { fill.fillAmount = fillAmount; }
}
