using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIStatPlayer : MonoBehaviour
{
    public static UIStatPlayer Instance { get; private set; }

    public GameObject panelRoot;
    [Header("UI Elements")]
    public Image avatarImage;
    public TMP_Text statText;

    [Header("Reference")]
    public PlayerStat playerStat;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (playerStat == null)
            playerStat = PlayerStat.Instance;

        // Tắt panel UI lúc bắt đầu
        if (panelRoot.activeSelf)
            panelRoot.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleUI();
        }
    }

    public void ToggleUI()
    {
        bool willShow = !panelRoot.activeSelf;
        panelRoot.SetActive(willShow);

        if (willShow)
            UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerStat == null) return;

        // Cập nhật avatar
        if (avatarImage != null && playerStat.characterInformation != null)
            avatarImage.sprite = playerStat.characterInformation.Avatar;

        CharacterStat baseStat = playerStat.PlayerStats;
        CharacterStat bonus = playerStat.BonusStats;
        CharacterStat Total = playerStat.TotalStats;
        string green = "#32CD32";

        statText.text =
            $"MaxHP:   {baseStat.MaxHP}  <color={green}>+ {bonus.MaxHP}</color>  = {Total.MaxHP}\n" +
            $"Damage:  {baseStat.Damage} <color={green}>+ {bonus.Damage}</color> = {Total.Damage}\n" +
            $"Speed:   {baseStat.Speed}  <color={green}>+ {bonus.Speed}</color>  = {Total.Speed}\n" +
            $"Luck:    {baseStat.Luck}   <color={green}>+ {bonus.Luck}</color>   = {Total.Luck}";
    }
}
