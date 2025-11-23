using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Image redBar;
    public TextMeshProUGUI healthText;

    private float maxHealth;

    public void SetMaxHealth(float max)
    {
        maxHealth = max;
        SetHealth(max);
    }

    public void SetHealth(float currentHealth)
    {
        if (redBar != null)
        {
            redBar.fillAmount = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.Ceil(currentHealth)} / {Mathf.Ceil(maxHealth)}";
        }
    }
}
