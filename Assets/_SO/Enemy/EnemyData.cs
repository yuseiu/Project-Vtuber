using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public List<EnemyContent> enemyContent;
}
[System.Serializable]
public class EnemyContent
{
    public EnemyInformation enemyInformation;
    public EnemyStat enemyStat;
}
