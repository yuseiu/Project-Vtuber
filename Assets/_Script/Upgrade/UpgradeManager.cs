using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    [Header("UI References")]
    public GameObject upgradePanel;
    public Transform upgradeContainer;
    public UpgradeButton upgradeButtonPrefab;

    [Header("Upgrade Data")]
    public ListUpgrade upgradeLists;  // chứa 2 list upgrade
    public int numberOfChoices = 3;

    private bool hasClearedButtons = false;
    public List<UpgradeButton> currentButtons = new List<UpgradeButton>();

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        upgradePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowUpgrade();
        }
    }

    public void ShowUpgrade()
    {
        if (!hasClearedButtons)
        {
            foreach (Transform child in upgradeContainer)
            {
                Destroy(child.gameObject);
            }
            hasClearedButtons = true;
        }

        upgradePanel.SetActive(true);

        while (currentButtons.Count < numberOfChoices)
        {
            var btn = Instantiate(upgradeButtonPrefab, upgradeContainer);
            currentButtons.Add(btn);
        }

        var upgradesToShow = GetRandomUpgrades(numberOfChoices);

        for (int i = 0; i < upgradesToShow.Count; i++)
        {
            currentButtons[i].SetData(upgradesToShow[i], this);
        }
    }

    public void HideUpgrade()
    {
        upgradePanel.SetActive(false);
    }

    public void OnUpgradeSelected(UpgradeSO chosenUpgrade)
    {
        Debug.Log($"Player chose upgrade: {chosenUpgrade.upgradeName}");
        HideUpgrade();
    }

    /// <summary>
    /// Random ra danh sách UpgradeSO (tự chọn giữa Stats hoặc Skills)
    /// </summary>
    private List<UpgradeSO> GetRandomUpgrades(int numberOfChoices)
    {
        List<UpgradeSO> result = new List<UpgradeSO>();

        for (int i = 0; i < numberOfChoices; i++)
        {
            UpgradeTag tag = (Random.value < 0.5f) ? UpgradeTag.Stats : UpgradeTag.Skills;
            UpgradeSO chosen = null;

            if (tag == UpgradeTag.Stats)
            {
                chosen = upgradeLists.GetRandomStatUpgradeByRarity();
            }
            else if (tag == UpgradeTag.Skills)
            {
                chosen = upgradeLists.GetRandomSkillUpgrade(); // useRarity = false
            }
            result.Add(chosen);
        }

        return result;
    }
}
