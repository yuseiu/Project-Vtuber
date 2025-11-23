using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInformation
{
    public string ID;
    public string Name;
    public Sprite Avatar;
    public GameObject Prefab;
    [TextArea(6,6)]
    public string Description;
}
