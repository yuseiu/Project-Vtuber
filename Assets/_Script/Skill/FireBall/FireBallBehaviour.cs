using UnityEngine;

public class FireBallBehaviour : SkillBehaviour
{
    public float speed = 10f;
    public float spawnOffset = 2f; // khoảng cách spawn ra phía trước caster

    public override void Activate(SkillData data, GameObject caster, Vector2 aimDirection)
    {
        if (data.skillPrefab == null)
        {
            Debug.LogWarning("Fireball prefab is missing in SkillData!");
            return;
        }

        GameObject container = GameObject.Find("SkillRuntime");
        if (container == null)
        {
            container = new GameObject("SkillRuntime");
        }

        // Tính vị trí spawn hơi lệch ra theo hướng bắn
        Vector3 spawnPos = caster.transform.position + (Vector3)(aimDirection.normalized * spawnOffset);

        // Tạo fireball tại vị trí mới
        GameObject fireball = Instantiate(data.skillPrefab, spawnPos, Quaternion.identity, container.transform);

        // Xoay fireball theo hướng
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Gán vận tốc
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = aimDirection.normalized * speed;
        }
        else
        {
            Debug.LogWarning("Fireball prefab missing Rigidbody2D!");
        }
    }
}
