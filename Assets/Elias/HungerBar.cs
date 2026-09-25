using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fill;

    [Range(0f, 1f)]
    public float progress = 1f;

    void Update()
    {
        fill.fillAmount = progress;
    }
}