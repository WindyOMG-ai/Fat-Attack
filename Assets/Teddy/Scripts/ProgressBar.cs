using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image bar;

    public FatTracker fatTracker;

    private void Start()
    {

        if (bar == null)
        {
            bar = GetComponent<Image>();
        }
    }

    void Update()
    {
        bar.fillAmount = fatTracker.CurrentCalories / 100f;
    }
}