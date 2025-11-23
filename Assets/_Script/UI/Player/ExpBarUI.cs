using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Image fill;
    public TextMeshProUGUI percentText;
    public TextMeshProUGUI levelText;

    public void UpdateExpBar(int level,float currentExp, float maxExp)
    {
        float percent = (maxExp > 0f) ? currentExp / maxExp : 0f;
        percent = Mathf.Clamp01(percent);

        if (fill != null)
            fill.fillAmount = percent;
        if (levelText != null)
            levelText.text = $"Lv {level}";
        if (percentText != null)
        {
            float percentage = percent * 100f;
            bool isWholeNumber = Mathf.Abs(percentage - Mathf.Round(percentage)) < 0.001f;

            if (isWholeNumber)
            {
                percentText.text = $"{Mathf.RoundToInt(percentage)}%";
            }
            else
            {
                percentText.text = $"{percentage:F2}%";
            }
        }
    }
}
