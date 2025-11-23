using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Lấy các property
        SerializedProperty itemName = serializedObject.FindProperty("itemName");
        SerializedProperty icon = serializedObject.FindProperty("icon");
        SerializedProperty tag = serializedObject.FindProperty("tag");
        SerializedProperty rarity = serializedObject.FindProperty("rarity");
        SerializedProperty Description = serializedObject.FindProperty("Description");
        SerializedProperty effectType = serializedObject.FindProperty("effectType");
        SerializedProperty modifyStatsPlayerInput = serializedObject.FindProperty("modifyStatsPlayerInput");
        SerializedProperty healingPlayerInput = serializedObject.FindProperty("healingPlayerInput");
        SerializedProperty addExp = serializedObject.FindProperty("addExp");

        // Hiển thị cơ bản
        EditorGUILayout.PropertyField(itemName);
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(tag);
        EditorGUILayout.PropertyField(rarity);
        EditorGUILayout.PropertyField(Description);
        EditorGUILayout.PropertyField(effectType);

        // Hiển thị tùy theo effect
        FunctionModify effect = (FunctionModify)effectType.enumValueIndex;
        if (effect == FunctionModify.ModifyStatsPlayer)
        {
            EditorGUILayout.PropertyField(modifyStatsPlayerInput, true);
        }
        if (effect == FunctionModify.HealingPlayer)
        {
            EditorGUILayout.PropertyField(healingPlayerInput, true);
        }if (effect == FunctionModify.AddExp)
        {
            EditorGUILayout.PropertyField(addExp, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
