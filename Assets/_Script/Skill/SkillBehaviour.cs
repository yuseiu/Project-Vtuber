using UnityEngine;

public abstract class SkillBehaviour : MonoBehaviour
{
    // data: dữ liệu skill
    // caster: ai sử dụng (player hoặc enemy)
    // aimDirection: hướng cast skill (chuột, analog, v.v.)
    public abstract void Activate(SkillData data, GameObject caster, Vector2 aimDirection);
}
