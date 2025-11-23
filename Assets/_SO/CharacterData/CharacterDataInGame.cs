using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/CharacterDataInGame")]
public class CharacterDataInGame : ScriptableObject
{
    public int LastSyncedVersion; // Ghi lại version của database lần sync cuối
    public List<Contents> Content;
}
[System.Serializable]
public class Contents
{
    public CharacterInformation CharacterInformation;
    public CharacterStat BaseStat;
    public CharacterStat BonusStat;
    public CharacterStat TotalStat;

    public void RecalculateTotalStat()
    {
        TotalStat = new CharacterStat
        {
            MaxHP = BaseStat.MaxHP + BonusStat.MaxHP,
            Damage = BaseStat.Damage + BonusStat.Damage,
            Speed = BaseStat.Speed + BonusStat.Speed,
            Luck = BaseStat.Luck + BonusStat.Luck
        };
    }
}
