using UnityEngine;
using System;

public class EnemyDeathTracker : MonoBehaviour
{
    public Action onDeath;

    void OnDestroy()
    {
        if (onDeath != null)
            onDeath.Invoke();
    }
}
