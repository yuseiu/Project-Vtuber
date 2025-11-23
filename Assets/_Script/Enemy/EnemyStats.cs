using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string Name;
    public EnemyData enemyData;
    public EnemyInformation enemyInformation;
    public EnemyStat enemyStat;

    void Awake()
    {
        if (enemyData == null || enemyData.enemyContent == null)
        {
            Debug.LogWarning("Chưa gán CharacterData hoặc Content rỗng.");
            return;
        }

        string objectName = Name;

        // Tìm nhân vật có tên trùng với tên GameObject
        EnemyContent matchedEnemy = enemyData.enemyContent.Find(c => c.enemyInformation.Name == objectName);

        if (matchedEnemy != null)
        {
            enemyInformation = matchedEnemy.enemyInformation;
            enemyStat = matchedEnemy.enemyStat;
        }
        //else
        //{
        //    Debug.LogWarning($"Không tìm thấy nhân vật có tên: {objectName} trong CharacterData.");
        //}
    }
}
