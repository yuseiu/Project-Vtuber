using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ItemActivate : MonoBehaviour
{
    public ItemData itemData;
    public void EffectActivate()
    {
        StatType statName;
        float value;
        if (itemData == null)
        {
            Debug.LogWarning("ItemData is null!");
            return;
        }
        switch (itemData.effectType)
        {
            case FunctionModify.ModifyStatsPlayer:
                statName = itemData.modifyStatsPlayerInput.StatName;
                value = itemData.modifyStatsPlayerInput.Value;
                Function.Instance.ModifyStatsPlayer(statName, value);
                ObjectPoolManager.Instance.ReturnPool(gameObject);
                break;
            case FunctionModify.HealingPlayer:
                value = itemData.healingPlayerInput.Value;
                Function.Instance.HealingPlayer(value);
                ObjectPoolManager.Instance.ReturnPool(gameObject);
                break;
            case FunctionModify.AddExp:
                value = itemData.addExp.Value;
                Function.Instance.AddExp(value);
                ObjectPoolManager.Instance.ReturnPool(gameObject);
                break;
            default:
                Debug.LogWarning($"No effect");
                break;
        }
    }
}
