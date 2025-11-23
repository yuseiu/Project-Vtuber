using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillBoxUI : MonoBehaviour
{
    public Image icon;
    public Image backgroundCooldown;
    public TextMeshProUGUI cooldownText;

    private SkillSlot slot;

    public void SetSlot(SkillSlot skillSlot)
    {
        slot = skillSlot;

        if (slot.skillData != null && icon != null)
            icon.sprite = slot.skillData.icon;
    }

    void Update()
    {
        if (slot == null || slot.skillData == null) return;

        float percent = slot.RemainingCooldownPercent;

        // Fill cooldown background
        if (backgroundCooldown != null)
        {
            backgroundCooldown.fillAmount = percent;
            backgroundCooldown.gameObject.SetActive(!slot.IsReady);
        }

        // Show cooldown text
        if (cooldownText != null)
        {
            if (slot.IsReady)
            {
                cooldownText.gameObject.SetActive(false);
            }
            else
            {
                cooldownText.text = $"{slot.RemainingCooldown:F1}s";
                cooldownText.gameObject.SetActive(true);
            }
        }
    }
}
