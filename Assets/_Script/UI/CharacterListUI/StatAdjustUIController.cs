using UnityEngine;
using UnityEngine.UI;
using TMPro; // ✅ Import TextMeshPro

public class StatAdjustUIController : MonoBehaviour
{
    [Header("HP")]
    public TextMeshProUGUI hpBaseText;
    public TextMeshProUGUI hpBonusText;
    public TextMeshProUGUI hpTotalText;
    public Button hpPlus, hpMinus;

    [Header("Dame")]
    public TextMeshProUGUI dameBaseText;
    public TextMeshProUGUI dameBonusText;
    public TextMeshProUGUI dameTotalText;
    public Button damePlus, dameMinus;

    [Header("Speed")]
    public TextMeshProUGUI speedBaseText;
    public TextMeshProUGUI speedBonusText;
    public TextMeshProUGUI speedTotalText;
    public Button speedPlus, speedMinus;

    [Header("Luck")]
    public TextMeshProUGUI luckBaseText;
    public TextMeshProUGUI luckBonusText;
    public TextMeshProUGUI luckTotalText;
    public Button luckPlus, luckMinus;

    [Header("Thông tin khác")]
    public TextMeshProUGUI pointsLeftText;
    public Button confirmButton;
    public Button cancelButton;
    public Button resetButton; 

    private Contents currentCharacter;
    private CharacterStat tempBonus;
    public float pointsLeft;

    public void LoadCharacter(Contents character)
    {
        currentCharacter = character;
        tempBonus = new CharacterStat();
        UpdateUI();
        EnableButtons(true);
    }

    void Start()
    {
        if (CharacterListUI.Instance.statUIController.gameObject.activeSelf)
        {
            CharacterListUI.Instance.statUIController.gameObject.SetActive(false);
        }
        hpPlus.onClick.AddListener(() => TryAdd(ref tempBonus.MaxHP));
        hpMinus.onClick.AddListener(() => TryRemove(ref tempBonus.MaxHP));
        damePlus.onClick.AddListener(() => TryAdd(ref tempBonus.Damage));
        dameMinus.onClick.AddListener(() => TryRemove(ref tempBonus.Damage));
        speedPlus.onClick.AddListener(() => TryAdd(ref tempBonus.Speed));
        speedMinus.onClick.AddListener(() => TryRemove(ref tempBonus.Speed));
        luckPlus.onClick.AddListener(() => TryAdd(ref tempBonus.Luck));
        luckMinus.onClick.AddListener(() => TryRemove(ref tempBonus.Luck));

        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
        resetButton.onClick.AddListener(OnReset);
    }

    void TryAdd(ref float stat)
    {
        if (pointsLeft > 0)
        {
            stat++;
            pointsLeft--;
            UpdateUI();
        }
    }

    void TryRemove(ref float stat)
    {
        if (stat > 0)
        {
            stat--;
            pointsLeft++;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        float baseHP = currentCharacter.BaseStat.MaxHP;
        float baseDame = currentCharacter.BaseStat.Damage;
        float baseSpeed = currentCharacter.BaseStat.Speed;
        float baseLuck = currentCharacter.BaseStat.Luck;

        float bonusHP = currentCharacter.BonusStat.MaxHP + tempBonus.MaxHP;
        float bonusDame = currentCharacter.BonusStat.Damage + tempBonus.Damage;
        float bonusSpeed = currentCharacter.BonusStat.Speed + tempBonus.Speed;
        float bonusLuck = currentCharacter.BonusStat.Luck + tempBonus.Luck;

        // HP
        hpBaseText.text = baseHP.ToString();
        hpBonusText.text = "+" + bonusHP;
        hpTotalText.text = "=" + (baseHP + bonusHP);

        // Dame
        dameBaseText.text = baseDame.ToString();
        dameBonusText.text = "+" + bonusDame;
        dameTotalText.text = "=" + (baseDame + bonusDame);

        // Speed
        speedBaseText.text = baseSpeed.ToString();
        speedBonusText.text = "+" + bonusSpeed;
        speedTotalText.text = "=" + (baseSpeed + bonusSpeed);

        // Luck
        luckBaseText.text = baseLuck.ToString();
        luckBonusText.text = "+" + bonusLuck;
        luckTotalText.text = "=" + (baseLuck + bonusLuck);

        // Điểm còn lại
        pointsLeftText.text = $"{pointsLeft}";
    }

    void OnConfirm()
    {
        currentCharacter.BonusStat.MaxHP += tempBonus.MaxHP;
        currentCharacter.BonusStat.Damage += tempBonus.Damage;
        currentCharacter.BonusStat.Speed += tempBonus.Speed;
        currentCharacter.BonusStat.Luck += tempBonus.Luck;

        currentCharacter.RecalculateTotalStat();
        tempBonus = new CharacterStat();
        UpdateUI();
    }

    void OnCancel()
    {
        // Tính tổng điểm đã cộng tạm
        float totalTempUsed = tempBonus.MaxHP + tempBonus.Damage + tempBonus.Speed + tempBonus.Luck;

        // Trả lại điểm
        pointsLeft += totalTempUsed;
        tempBonus = new CharacterStat();
        UpdateUI();
    }

    void OnReset()
    {
        // Tính tổng bonus đã cộng trước đó
        float totalBonusUsed = currentCharacter.BonusStat.MaxHP +
                             currentCharacter.BonusStat.Damage +
                             currentCharacter.BonusStat.Speed +
                             currentCharacter.BonusStat.Luck;

        // Trả điểm lại
        pointsLeft += totalBonusUsed;

        // Reset BonusStat về 0
        currentCharacter.BonusStat = new CharacterStat();

        // Reset bonus tạm nếu đang cộng thêm
        tempBonus = new CharacterStat();

        // Tính lại tổng stat và cập nhật giao diện
        currentCharacter.RecalculateTotalStat();
        UpdateUI();
    }

    void EnableButtons(bool state)
    {
        hpPlus.interactable = hpMinus.interactable = state;
        damePlus.interactable = dameMinus.interactable = state;
        speedPlus.interactable = speedMinus.interactable = state;
        luckPlus.interactable = luckMinus.interactable = state;
        confirmButton.interactable = cancelButton.interactable = state;
    }
}
