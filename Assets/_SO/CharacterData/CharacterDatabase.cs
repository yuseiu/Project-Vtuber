using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    public int DataVersion; // Version toàn bộ database
    public List<Content> Content;
}

[System.Serializable]
public class Content
{
    public bool CreateData;
    public int DataVersion; // Version riêng từng nhân vật
    public CharacterInformation CharacterInformation;
    public bool UpdateInformation;
    public CharacterStat Stat;
    public bool UpdateStat;
}
