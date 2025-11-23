using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrade/UpgradeSO")]
public class UpgradeSO : ScriptableObject
{
    [Header("UI Hiển thị")]
    public string upgradeName;
    public Sprite icon;
    public UpgradeTag tag;
    public Rarity rarity;
    [TextArea(3, 3)]
    public string description;
    public SkillUpgrade skillUpgrade;
    public FunctionModify functionModify;
    public ModifyStatsPlayerInput modifyStatsPlayerInput;
}


