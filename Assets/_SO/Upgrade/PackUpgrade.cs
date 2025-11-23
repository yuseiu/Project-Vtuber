using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PackUpgrade", menuName = "Upgrade/PackUpgrade")]
public class PackUpgrade : ScriptableObject
{
    public StatType upgradeName;
    public List<UpgradeSO> packUpgrade;
}
