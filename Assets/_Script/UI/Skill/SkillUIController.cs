using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    public SkillBoxUI[] skillBoxes; // size = 7

    void Start()
    {
        if (SkillManager.Instance == null)
        {
            Debug.LogWarning("SkillManager.Instance is null. UI won't be initialized.");
            return;
        }

        var slots = SkillManager.Instance.skillSlots;

        for (int i = 0; i < skillBoxes.Length; i++)
        {
            if (i < slots.Count)
            {
                skillBoxes[i].SetSlot(slots[i]);
                skillBoxes[i].gameObject.SetActive(true);
            }
            else
            {
                skillBoxes[i].gameObject.SetActive(false);
            }
        }
    }
}
