using System.Collections.Generic;
using UnityEngine;

public class ScriptDisabler : MonoBehaviour
{
    public static ScriptDisabler Instance { get; private set; }

    [Tooltip("Tên các script muốn tắt hoặc bật.")]
    public List<string> scriptNamesToToggle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Tắt script trên GameObject được chỉ định.
    /// </summary>
    public void DisableScriptsOn(GameObject target, bool includeChildren = true)
    {
        ToggleScriptsOn(target, false, includeChildren);
    }

    /// <summary>
    /// Bật script trên GameObject được chỉ định.
    /// </summary>
    public void EnableScriptsOn(GameObject target, bool includeChildren = true)
    {
        ToggleScriptsOn(target, true, includeChildren);
    }

    private void ToggleScriptsOn(GameObject target, bool state, bool includeChildren)
    {
        if (target == null) return;

        var targets = includeChildren
            ? target.GetComponentsInChildren<MonoBehaviour>(true)
            : target.GetComponents<MonoBehaviour>();

        foreach (var mono in targets)
        {
            if (mono == null) continue;

            string typeName = mono.GetType().Name;
            if (scriptNamesToToggle.Contains(typeName))
            {
                mono.enabled = state;
            }
        }
    }
}
