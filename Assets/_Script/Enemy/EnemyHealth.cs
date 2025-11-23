using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyHealth : MonoBehaviour
{
    public float CurrentHealth;
    private float maxHealth;
    public bool IsDead = false;

    private EnemyStats enemyStats;
    private EnemyMovement enemyMovement;
    private EnemyController enemyController;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Hit Flash Settings")]
    public float flashDuration = 0.1f;
    public Color damageColor = new Color(0xEF / 255f, 0x37 / 255f, 0x37 / 255f);

    [Header("Death Settings")]
    public float destroyDelay = 0.5f;
    public EnemyHealthBarUI enemyHealthBarUI;
    public Transform HealthbarTransform;

    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyController = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        ResetState();
    }

    //private void OnDisable()
    //{
    //    DisableState();
    //}

    private void Start()
    {
        if (enemyStats != null && enemyStats.enemyStat != null)
        {
            maxHealth = enemyStats.enemyStat.HP;
            CurrentHealth = maxHealth;
        }
        else
        {
            Debug.LogWarning("EnemyStat chưa được gán!");
            maxHealth = 10;
            CurrentHealth = maxHealth;
        }

        if (EnemyUIController.instance != null)
        {
            enemyHealthBarUI = EnemyUIController.instance.SpawnHealthBar(HealthbarTransform);
            enemyHealthBarUI.SetMaxHealth(maxHealth);
        }
        else
        {
            Debug.LogError("EnemyUIController.instance is null! Đảm bảo nó tồn tại trong scene.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        enemyHealthBarUI.SetHealth(CurrentHealth);

        if (spriteRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashRed());
        }

        EnemyUIController.instance.SpawnDamage(damage, transform.position);

        if (CurrentHealth <= 0)
        {
            Die();
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
        IsDead = true;
        DisableState();
        // Animation chết
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Hủy thanh máu
        if (enemyHealthBarUI != null)
            enemyHealthBarUI.Destroy();

        // Spawn exp
        ObjectPoolManager.Instance.GetPool("Exp", "ExpCommon", transform.position);

        // Trả về pool sau delay
        StartCoroutine(ReturnToPoolAfterDelay(destroyDelay));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPoolManager.Instance.ReturnPool(gameObject);
    }

    // ----------------------
    // RESET/TẮT TRẠNG THÁI
    // ----------------------
    private void ResetState()
    {
        IsDead = false;

        // Reset máu
        if (enemyStats != null && enemyStats.enemyStat != null)
            maxHealth = enemyStats.enemyStat.HP;
        CurrentHealth = maxHealth;

        // Reset UI máu
        if (EnemyUIController.instance != null)
        {
            enemyHealthBarUI = EnemyUIController.instance.SpawnHealthBar(HealthbarTransform);
            enemyHealthBarUI.SetMaxHealth(maxHealth);
        }

        // Reset EnemyMovement
        if (enemyMovement != null)
        {
            enemyMovement.isMoving = true;
            enemyMovement.ResetMovement(); // <--- gọi hàm reset bên EnemyMovement
        }

        // Reset EnemyController
        if (enemyController != null)
        {
            enemyController.enabled = true;
            enemyController.ResetController(); // <--- gọi hàm reset bên EnemyController
        }

        // Reset Rigidbody
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = true;
        }

        // Reset màu sprite
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    private void DisableState()
    {
        if (enemyMovement != null) enemyMovement.isMoving = false;
        if (enemyController != null) enemyController.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }
    }
}
