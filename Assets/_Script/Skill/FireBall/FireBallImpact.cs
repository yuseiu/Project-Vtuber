using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallImpact : MonoBehaviour
{
    public SkillData skillData;
    public float timeOut = 2f;
    public float Dame;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        Dame = PlayerStat.Instance.PlayerStats.Damage + skillData.damage;
        Destroy(gameObject, timeOut);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null && !enemyHealth.IsDead)
            {
                enemyHealth.TakeDamage(Dame);
            }
            if (animator != null)
            {
                animator.SetTrigger("End");
            }
            Destroy(gameObject, 0.05f);
        }
    }
}
