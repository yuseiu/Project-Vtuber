using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Image redBar;

    private float maxHealth;
    private Coroutine hideCoroutine;
    public float hideDelay = 2f; // thời gian trước khi ẩn

    public void SetMaxHealth(float max)
    {
        maxHealth = max;
    }

    public void SetHealth(float currentHealth)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true); // Hiện ra nếu đang ẩn

        if (redBar != null)
        {
            redBar.fillAmount = currentHealth / maxHealth;
        }

        Show();
    }

    public void Show()
    {
        // Reset lại coroutine ẩn
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        gameObject.SetActive(false);
    }

    public void HideImmediate()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);
        gameObject.SetActive(false);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
