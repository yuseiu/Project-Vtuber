using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float CurrentHealth;
    public float maxHealth;
    private HealthBarUI healthBarUI;
    public bool IsDeath = false;

    // Thêm phần mới:
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float flashDuration = 0.2f;
    public Color damageColor = new Color(0xEF / 255f, 0x37 / 255f, 0x37 / 255f); // #EF3737

    void Start()
    {
        // Lấy HP từ PlayerStat
        maxHealth = PlayerStat.Instance.TotalStats.MaxHP;
        CurrentHealth = maxHealth;

        // Health bar
        healthBarUI = FindObjectOfType<HealthBarUI>();
        if (healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(maxHealth);
        }

        // Lấy SpriteRenderer và màu gốc
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(CurrentHealth);
        }

        // Flash đỏ khi trúng đòn
        if (spriteRenderer != null)
        {
            StopAllCoroutines(); // Nếu đang flash thì dừng cái cũ
            StartCoroutine(FlashRed());
        }

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    public void RecoverHealth(float value)
    {
        CurrentHealth += value;
        if(CurrentHealth >= maxHealth)
        {
            CurrentHealth = maxHealth;
        }
        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(CurrentHealth);
        }
    }
    public void UpdateHealthUI()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetMaxHealth(maxHealth); // Cập nhật lại max
            healthBarUI.SetHealth(CurrentHealth); // Cập nhật lại current theo tỉ lệ mới
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        IsDeath = true;
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}
