using UnityEngine;

[CreateAssetMenu(menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemTag tag;
    public Rarity rarity;
    [TextArea(6, 6)]
    public string Description;
    public FunctionModify effectType;
    public ModifyStatsPlayerInput modifyStatsPlayerInput;
    public HealingPlayerInput healingPlayerInput;
    public AddExp addExp;
}

[System.Serializable]
public class ModifyStatsPlayerInput
{
    public StatType StatName;
    public float Value;
}
[System.Serializable]
public class HealingPlayerInput
{
    public float Value;
}
[System.Serializable]
public class AddExp
{
    public float Value;
}
