using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public float startTime;
    public bool started = false;
    public bool finished = false;
    public List<EnemySpawn> spawns;

    public bool AllFinished()
    {
        return spawns.TrueForAll(s => s.finished);
    }
}
