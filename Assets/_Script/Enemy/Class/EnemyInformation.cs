using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInformation
{
    public string ID;
    public string Name;
    public Sprite Avatar;
    public Tag tag;
    public GameObject Prefab;
    [TextArea(6, 6)]
    public string Description;
}
