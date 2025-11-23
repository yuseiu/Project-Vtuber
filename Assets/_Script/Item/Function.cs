using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public static Function Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    public void ModifyStatsPlayer(StatType stat, float value)
    {
        PlayerStat.Instance.ModifyStats(stat, value);
    }
    public void HealingPlayer(float value)
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        playerHealth.RecoverHealth(value);
    }

    public void AddExp(float value)
    {
        PlayerStat.Instance.AddExp(value);
    }
}
