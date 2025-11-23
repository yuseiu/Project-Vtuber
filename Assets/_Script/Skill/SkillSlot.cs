using UnityEngine;

[System.Serializable]
public class SkillSlot
{
    public SkillData skillData;
    private float cooldownTimer;
    public bool IsReady => cooldownTimer <= 0f;
    public float RemainingCooldown => Mathf.Max(0f, cooldownTimer);
    public float RemainingCooldownPercent => skillData != null && skillData.cooldown > 0f
        ? cooldownTimer / skillData.cooldown
        : 0f;

    // Gọi trong Update
    public void TickCooldown(float deltaTime)
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= deltaTime;
    }

    public void ResetCooldown(GameObject caster)
    {
        float multiplier = 1f;
        PlayerStat stats = caster.GetComponent<PlayerStat>();
        if (stats != null)
        {
            multiplier = stats.GetCooldownMultiplier();
        }

        cooldownTimer = skillData.cooldown * multiplier;
    }


    // Kích hoạt kỹ năng
    public void Activate(GameObject caster, Vector2 aimDirection)
    {
        if (!IsReady || skillData == null || skillData.PrefabSkillBehaviour == null) return;

        // Tìm hoặc tạo container để chứa tất cả skill runtime (đặt tên duy nhất nếu chưa có)
        GameObject container = GameObject.Find("SkillBehaviourContainer");
        if (container == null)
        {
            container = new GameObject("SkillBehaviourContainer");
        }

        // Clone SkillBehaviour, đặt trong container
        SkillBehaviour behaviour = Object.Instantiate(skillData.PrefabSkillBehaviour, container.transform);

        if (behaviour != null)
        {
            behaviour.Activate(skillData, caster, aimDirection);

            // Xoá riêng SkillBehaviour sau x giây
            Object.Destroy(behaviour.gameObject, 2f);
        }

        ResetCooldown(caster);
    }

}
