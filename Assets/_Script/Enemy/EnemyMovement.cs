using UnityEngine;
using Pathfinding; // Quan trọng: để dùng Seeker, Path

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Seeker))]
public class EnemyMovement : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    public float moveSpeed;

    private Rigidbody2D rb;
    private Animator animator;
    private EnemyStats enemyStats;
    private Transform player;
    private PlayerHealth playerHealth;
    private Seeker seeker;

    public bool isMoving = true;

    private float waveFrequency = 2f;
    private float waveMagnitude = 0.5f;
    private float waveOffset;

    private Path path;
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 0.5f;
    public float repeatTimeUpdatePath = 0.8f;

    void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("Player not found! Please tag the player as 'Player'");
        }
    }

    void Start()
    {
        // Random ban đầu
        minSpeed = enemyStats.enemyStat.minSpeed;
        maxSpeed = enemyStats.enemyStat.maxSpeed;
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        waveFrequency = Random.Range(1.8f, 2.8f);
        waveMagnitude = Random.Range(0.3f, 0.5f);
        waveOffset = Random.Range(0f, 2 * Mathf.PI);

        // Bắt đầu update path
        InvokeRepeating(nameof(UpdatePath), 0f, repeatTimeUpdatePath);
    }

    void Update()
    {
        // Nếu player chết hoặc không tồn tại, dừng di chuyển
        if (playerHealth == null || playerHealth.IsDeath)
        {
            isMoving = false;
        }

        animator.SetBool("Move", isMoving);

        // Flip sprite theo hướng player
        if (isMoving && player != null)
        {
            float directionX = player.position.x - transform.position.x;
            if (Mathf.Abs(directionX) > 0.01f)
            {
                Vector3 scale = transform.localScale;
                scale.x = directionX > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
                transform.localScale = scale;
            }
        }
    }

    void UpdatePath()
    {
        if (player != null && seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (!isMoving || path == null || currentWaypoint >= path.vectorPath.Count)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 targetWaypoint = path.vectorPath[currentWaypoint];
        Vector2 direction = (targetWaypoint - rb.position).normalized;

        // Zigzag movement
        Vector2 perpendicular = Vector2.Perpendicular(direction);
        float wave = Mathf.Sin(Time.time * waveFrequency + waveOffset) * waveMagnitude;
        Vector2 finalDir = (direction + perpendicular * wave).normalized;

        rb.velocity = finalDir * moveSpeed;

        // Đến gần waypoint thì chuyển sang waypoint tiếp theo
        float distance = Vector2.Distance(rb.position, targetWaypoint);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    public void ResetMovement()
    {
        isMoving = true;

        // Random lại tốc độ & zigzag
        minSpeed = enemyStats.enemyStat.minSpeed;
        maxSpeed = enemyStats.enemyStat.maxSpeed;
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        waveFrequency = Random.Range(1.8f, 2.8f);
        waveMagnitude = Random.Range(0.3f, 0.5f);
        waveOffset = Random.Range(0f, 2 * Mathf.PI);

        // Reset waypoint để tránh đi path cũ
        path = null;
        currentWaypoint = 0;

        // Gọi cập nhật đường đi ngay lập tức
        if (player != null && seeker != null && seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }

        // Reset animation
        if (animator != null)
            animator.SetBool("Move", true);

        // Reset velocity
        if (rb != null)
            rb.velocity = Vector2.zero;
    }

}
