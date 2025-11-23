using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // Singleton instance
    public static PlayerStat Instance { get; private set; }

    [Header("Dữ liệu nhân vật")]
    public string Name;
    public CharacterDataInGame characterData;
    public CharacterStat PlayerStats;
    public CharacterInformation characterInformation;
    public HealthBarUI healthBarUI;
    [Header("Stat cộng thêm trong game")]
    public CharacterStat BonusStats = new CharacterStat();
    public CharacterStat TotalStats = new CharacterStat();
    [Header("Cấp độ")]
    public int level = 0;
    public float currentExp = 0f;
    public float maxExp = 100f;
    public float expGrowthRate = 0.5f; // 50% mỗi level
    public ExpBarUI expBarUI;

    void Awake()
    {
        Instance = this;
        healthBarUI = FindAnyObjectByType<HealthBarUI>();
        expBarUI = FindAnyObjectByType<ExpBarUI>();
        GetDataPlayer();
        TotalAllStats();
    }
    private void Start()
    {
        expBarUI.UpdateExpBar(level, currentExp, maxExp);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddExp(30f);
            Debug.Log("Đã cộng 30 EXP bằng phím Space");
        }
    }
    public void TotalAllStats()
    {
        TotalStats.MaxHP = PlayerStats.MaxHP + BonusStats.MaxHP;
        TotalStats.Damage = PlayerStats.Damage + BonusStats.Damage;
        TotalStats.Speed = PlayerStats.Speed + BonusStats.Speed;
        TotalStats.Luck = PlayerStats.Luck + BonusStats.Luck;
    }
    public void GetDataPlayer()
    {
        if (characterData == null || characterData.Content == null)
        {
            Debug.LogWarning("Chưa gán CharacterData hoặc Content rỗng.");
            return;
        }
        string objectName = Name;

        // Tìm nhân vật có tên trùng với tên GameObject
        Contents matchedCharacter = characterData.Content.Find(c => c.CharacterInformation.Name == objectName);

        if (matchedCharacter != null)
        {
            matchedCharacter.RecalculateTotalStat();
            PlayerStats = matchedCharacter.TotalStat;
            characterInformation = matchedCharacter.CharacterInformation;

            Debug.Log($"Tìm thấy nhân vật {objectName}");
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy nhân vật có tên: {objectName} trong CharacterData.");
        }
    }
    public void ModifyStats(StatType stat, float value)
    {
        switch (stat)
        {
            case StatType.MaxHP:
                BonusStats.MaxHP += value;
                TotalStats.MaxHP += value;

                var playerHealth = GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.maxHealth = TotalStats.MaxHP;
                    playerHealth.UpdateHealthUI();
                }
                break;

            case StatType.Damage:
                BonusStats.Damage += value;
                TotalStats.Damage += value;
                break;

            case StatType.Speed:
                BonusStats.Speed += value;
                TotalStats.Speed += value;
                break;

            case StatType.Luck:
                BonusStats.Luck += value;
                TotalStats.Luck += value;
                break;
        }

        Debug.Log($"Đã tăng {stat} thêm {value}");

        // Tự động cập nhật UI stat (nếu có)
        UIStatPlayer.Instance.UpdateUI();
    }


    //private float GetStatValue(StatType stat)
    //{
    //    return stat switch
    //    {
    //        StatType.MaxHP => PlayerStats.MaxHP,
    //        StatType.Damage => PlayerStats.Damage,
    //        StatType.Speed => PlayerStats.Speed,
    //        StatType.Luck => PlayerStats.Luck,
    //        _ => 0f
    //    };
    //}
    public void AddExp(float amount)
    {
        currentExp += amount;

        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
        }

        if (expBarUI != null)
        {
            expBarUI.UpdateExpBar(level,currentExp, maxExp);
        }
    }

    private void LevelUp()
    {
        level++;
        maxExp += maxExp * expGrowthRate;
        UpgradeManager.Instance.ShowUpgrade();
        Debug.Log($"🎉 Lên cấp {level}! Max Exp mới: {maxExp}");

        // (Tuỳ chọn) Tăng chỉ số:
        //ModifyStats(StatType.MaxHP, 10f);
        //ModifyStats(StatType.Damage, 1f);
    }
    public float GetCooldownMultiplier()
    {
        return Mathf.Clamp01(1f - PlayerStats.cooldownTime / 100f);
    }
}


