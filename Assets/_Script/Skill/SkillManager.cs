using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    [Header("Skills")]
    public List<SkillSlot> skillSlots = new List<SkillSlot>();

    [Header("References")]
    public Transform arrowAimDirection; // hướng cast, thường là arrow xoay quanh player

    [Header("Auto Attack Settings")]
    public float rangeAttack = 5f;
    public LayerMask enemyLayer;

    public GameObject Caster => this.gameObject;

    void Awake()
    {
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        Instance = this;
    }

    void Update()
    {
        Vector2 aimDirection = (arrowAimDirection != null)
            ? (arrowAimDirection.position - Caster.transform.position).normalized
            : Vector2.right;

        foreach (var slot in skillSlots)
        {
            slot.TickCooldown(Time.deltaTime);
        }

        // Auto attack nếu có enemy trong range => nhưng vẫn dùng arrowAimDirection
        if (skillSlots.Count > 0 && skillSlots[0].IsReady && EnemyInRange())
        {
            skillSlots[0].Activate(Caster, aimDirection);
        }

        // TEST bằng phím nếu muốn
        if (Input.GetKeyDown(KeyCode.Alpha1) && skillSlots.Count > 0)
        {
            skillSlots[0].Activate(Caster, aimDirection);
        }
    }

    private bool EnemyInRange()
    {
        Vector2 center = Caster.transform.position;
        Vector2 boxSize = new Vector2(rangeAttack * 2f, rangeAttack * 1.1f);
        Collider2D col = Physics2D.OverlapBox(center, boxSize, 0f, enemyLayer);
        return col != null;
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 boxSize = new Vector2(rangeAttack * 2f, rangeAttack * 1.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    // Gán kỹ năng vào slot
    public void EquipSkill(SkillData newSkill, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= skillSlots.Count) return;

        skillSlots[slotIndex].skillData = newSkill;
    }
}
