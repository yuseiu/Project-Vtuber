using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("Cấu hình")]
    public Transform player;
    public float distanceFromPlayer = 2.0f;

    [Header("Auto Aim")]
    public bool autoAimEnabled = false;      // Gán qua Toggle
    public string enemyTag = "Enemy";        // Tag của Enemy

    void Update()
    {
        if (player == null) return;

        Vector3 direction;

        if (autoAimEnabled)
        {
            GameObject nearestEnemy = FindClosestEnemy();
            if (nearestEnemy != null)
            {
                direction = (nearestEnemy.transform.position - player.position).normalized;
            }
            else
            {
                direction = Vector3.right; // Nếu không có enemy, mặc định hướng phải
            }
        }
        else
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = player.position.z;
            direction = (mouseWorldPos - player.position).normalized;
        }

        // Luôn đặt vị trí Arrow theo vị trí player
        transform.position = player.position + direction * distanceFromPlayer;

        // Quay mũi tên theo hướng đó
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void SetAutoAim(bool isOn)
    {
        autoAimEnabled = isOn;
        Debug.Log("Auto Aim: " + autoAimEnabled);
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(player.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }
}
