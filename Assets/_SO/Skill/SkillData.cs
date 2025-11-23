using UnityEngine;

[CreateAssetMenu(menuName = "Skill System/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("Information")]
    public string skillName;
    public Sprite icon;
    public SkillType skillType;

    [Header("References")]
    public GameObject skillPrefab;              // Prefab của kỹ năng (đạn, vùng nổ, summon...)
    public SkillBehaviour PrefabSkillBehaviour;

    [Header("Stat")]
    public float damage;
    public float cooldown;
    public float duration;
    public float range;
    public float areaRadius;
    public float summonLifetime;
}

public enum SkillType
{
    Projectile,
    Area,
    Buff,
    Summon,
    Aura,
    Dash,
    Shield,
    Passive
}
