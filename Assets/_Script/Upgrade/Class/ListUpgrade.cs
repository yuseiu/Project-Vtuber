using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ListUpgrade
{
    public List<PackUpgrade> statUpgrades;  // danh sách theo StatType
    public List<PackUpgrade> skillUpgrades; // danh sách skill bình thường

    // -----------------------------
    // Random Stat Upgrade theo StatType + Rarity
    // -----------------------------
    public UpgradeSO GetRandomStatUpgradeByRarity()
    {
        if (statUpgrades == null || statUpgrades.Count == 0)
        {
            Debug.LogWarning("⚠️ Chưa có StatUpgrade nào!");
            return null;
        }

        // 1️⃣ Random StatType
        StatType statType = (StatType)Random.Range(0, System.Enum.GetValues(typeof(StatType)).Length);

        //// 2️⃣ Lấy PackUpgrade tương ứng
        //var pack = statUpgrades.FirstOrDefault(p => p.upgradeName == statType);
        //if (pack == null || pack.packUpgrade == null || pack.packUpgrade.Count == 0)
        //{
        //    Debug.LogWarning($"⚠️ Không tìm thấy PackUpgrade cho StatType {statType}");
        //    return null;
        //}
        var pack = statUpgrades[Random.Range(0, statUpgrades.Count)];

        if (pack.packUpgrade == null || pack.packUpgrade.Count == 0)
        {
            Debug.LogWarning($"⚠️ PackUpgrade '{pack.upgradeName}' rỗng!");
            return null;
        }

        // 3️⃣ Random Rarity
        Rarity rarity = GetRandomRarity();
        var filtered = pack.packUpgrade.Where(u => u.rarity == rarity).ToList();

        // Fallback về Common nếu không có
        if (filtered.Count == 0)
        {
            Debug.LogWarning($"⚠️ Trong Pack {statType} không có UpgradeSO với rarity {rarity}. Trả về null.");
            return null;
        }

        if (filtered.Count > 0)
        {
            int index = Random.Range(0, filtered.Count);
            return filtered[index];
        }

        return null;
    }

    // -----------------------------
    // Random Skill Upgrade bình thường
    // -----------------------------
    public UpgradeSO GetRandomSkillUpgrade()
    {
        if (skillUpgrades == null || skillUpgrades.Count == 0)
        {
            Debug.LogWarning("⚠️ Chưa có SkillUpgrade nào!");
            return null;
        }

        // Random 1 PackUpgrade
        int packIndex = Random.Range(0, skillUpgrades.Count);
        var pack = skillUpgrades[packIndex];

        if (pack.packUpgrade == null || pack.packUpgrade.Count == 0)
            return null;

        int index = Random.Range(0, pack.packUpgrade.Count);
        return pack.packUpgrade[index];
    }

    // -----------------------------
    // Random Rarity
    // -----------------------------
    private Rarity GetRandomRarity()
    {
        float roll = Random.value;

        if (roll < 0.40f) return Rarity.Common;
        else if (roll < 0.65f) return Rarity.Uncommon;
        else if (roll < 0.80f) return Rarity.Rare;
        else if (roll < 0.90f) return Rarity.Epic;
        else if (roll < 0.97f) return Rarity.Legendary;
        else return Rarity.Mythical;
    }
}
