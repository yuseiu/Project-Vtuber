using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Key
{
    public string action;
    public KeyCode key;
}

[CreateAssetMenu(fileName = "KeybindingsList", menuName = "Settings/KeyList")]
public class KeySO : ScriptableObject
{
    public List<Key> keyList = new List<Key>();

    public KeyCode GetKey(string action)
    {
        var binding = keyList.Find(k => k.action == action);
        return binding != null ? binding.key : KeyCode.None;
    }

    public void SetKey(string action, KeyCode newKey)
    {
        var binding = keyList.Find(k => k.action == action);
        if (binding != null)
        {
            binding.key = newKey;
        }
    }

    public void LoadFrom(KeySO source)
    {
        keyList.Clear();
        foreach (var binding in source.keyList)
        {
            keyList.Add(new Key { action = binding.action, key = binding.key });
        }
    }
}
