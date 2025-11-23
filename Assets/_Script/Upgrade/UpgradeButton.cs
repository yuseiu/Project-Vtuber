using UnityEngine;
using UnityEngine.UI;
using TMPro; // nếu bạn dùng TextMeshPro

public class UpgradeButton : MonoBehaviour
{
    [Header("UI Elements")]
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private UpgradeSO upgradeData;
    private UpgradeManager manager;

    // Gán dữ liệu hiển thị
    public void SetData(UpgradeSO data, UpgradeManager mgr)
    {
        if (data == null)
        {
            ResetButton();
            return;
        }
        upgradeData = data;
        manager = mgr;

        icon.sprite = data.icon;
        //titleText.text = data.upgradeName;
        TextFormat(data);
    }
    public void ResetButton()
    {
        upgradeData = null;
        icon.sprite = null;
        titleText.text = "";
        descriptionText.text = "";
    }
    public void TextFormat(UpgradeSO data)
    {
        string rarityColor = data.rarity switch
        {
            Rarity.Common => "#90EE90", 
            Rarity.Uncommon => "#2ECC71", 
            Rarity.Rare => "#0050FF",
            Rarity.Epic => "#B84DFF",
            Rarity.Legendary => "#FF9A00",
            Rarity.Mythical => "#FF3B3B", 
            _ => "#FFFFFF"
        };

        titleText.text = $"<color={rarityColor}>{data.upgradeName}\n({data.rarity})</color>";
        descriptionText.text = $"<color={rarityColor}>{data.description}</color>";
    }
    // Gọi khi click vào button
    public void OnClick()
    {
        ActivateStatsModify();
        if (manager != null && upgradeData != null)
        {
            manager.OnUpgradeSelected(upgradeData);
        }
    }
    public void ActivateStatsModify()
    {
        if(upgradeData.tag == UpgradeTag.Stats)
        {
            if(upgradeData.functionModify == FunctionModify.ModifyStatsPlayer)
            {
                StatType statname = upgradeData.modifyStatsPlayerInput.StatName;
                float value = upgradeData.modifyStatsPlayerInput.Value;
                Function.Instance.ModifyStatsPlayer(statname, value);
            }
        }
    }
}
