public enum Tag
{
    None,
    Item,
    Enemy
}
public enum ItemTag
{
    None,
    Health,
    Exp
}
public enum Rarity
{
    None,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythical
}
[System.Serializable]
public enum FunctionModify
{
    None,
    ModifyStatsPlayer,
    HealingPlayer,
    AddExp
}
public enum UpgradeTag
{
    None,
    Stats,
    Skills
}
public enum StatType
{
    MaxHP,
    Damage,
    Speed,
    Luck
}
public enum SkillUpgrade
{
    None,
    FireBall
}