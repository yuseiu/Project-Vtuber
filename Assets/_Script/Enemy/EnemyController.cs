using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private PlayerHealth playerHealth;
    private Animator animator;
    private EnemyStats enemyStats;

    public float attackCooldown = 5f;
    private float lastAttackTime = -Mathf.Infinity;

    private bool playerInRange = false;

    void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
        enemyStats = GetComponent<EnemyStats>();
    }
    private void Start()
    {
        attackCooldown = enemyStats.enemyStat.AttackCooldown;
    }
    void Update()
    {
        if (playerInRange && playerHealth != null && !playerHealth.IsDeath)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth == null || playerHealth.IsDeath) return;

        enemyMovement.isMoving = false;
        playerInRange = true;

        // Attack ngay lập tức khi vừa chạm
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            enemyMovement.isMoving = true;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        playerHealth.TakeDamage(2);
    }

    public void ResetController()
    {
        // Reset cooldown tấn công
        attackCooldown = 0f;

        // Nếu có animation idle/ready thì reset ở đây
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

}
