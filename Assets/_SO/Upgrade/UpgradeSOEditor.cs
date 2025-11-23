using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeSO))]
public class UpgradeSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Lấy các property
        SerializedProperty upgradeName = serializedObject.FindProperty("upgradeName");
        SerializedProperty icon = serializedObject.FindProperty("icon");
        SerializedProperty tag = serializedObject.FindProperty("tag");
        SerializedProperty rarity = serializedObject.FindProperty("rarity");
        SerializedProperty functionModify = serializedObject.FindProperty("functionModify");
        SerializedProperty skillUpgrade = serializedObject.FindProperty("skillUpgrade");
        SerializedProperty description = serializedObject.FindProperty("description");
        SerializedProperty modifyStatsPlayerInput = serializedObject.FindProperty("modifyStatsPlayerInput");

        // Hiển thị cơ bản
        EditorGUILayout.PropertyField(upgradeName);
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(tag);
        EditorGUILayout.PropertyField(rarity);
        EditorGUILayout.PropertyField(description);
        
        // Hiển thị tùy theo effect
        UpgradeTag Upgrade = (UpgradeTag)tag.enumValueIndex;
        if (Upgrade == UpgradeTag.Stats)
        {
            EditorGUILayout.PropertyField(functionModify, true);
            FunctionModify functionmodify = (FunctionModify)functionModify.enumValueIndex;
            if (functionmodify == FunctionModify.ModifyStatsPlayer)
            {
                EditorGUILayout.PropertyField(modifyStatsPlayerInput, true);
            }
        }
        if (Upgrade == UpgradeTag.Skills)
        {
            EditorGUILayout.PropertyField(skillUpgrade, true);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
