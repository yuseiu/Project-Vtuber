using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListUI : MonoBehaviour
{
    public static CharacterListUI Instance { get; private set; }

    public CharacterDataInGame characterDataSO;
    public Transform contentPanel; // Gán là Content của ScrollView
    public GameObject characterButtonPrefab; // Prefab chứa Button và Image

    public Transform characterDisplayParent; // nơi đặt nhân vật
    public GameObject currentCharacterGO; // nhân vật đang hiển thị
    public GameObject statUIController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Chỉ giữ lại 1 instance
            return;
        }
        Instance = this;
        // Optionally: DontDestroyOnLoad(gameObject); // nếu muốn giữ lại qua scene khác
    }

    void Start()
    {
        PopulateCharacterList();
    }

    public void PopulateCharacterList()
    {
        ClearContent(); // Xoá trước khi load mới

        foreach (var content in characterDataSO.Content)
        {
            GameObject buttonGO = Instantiate(characterButtonPrefab, contentPanel);

            // Đặt ảnh avatar vào nút
            Image avatarImage = buttonGO.GetComponentInChildren<Image>();
            if (avatarImage != null)
            {
                avatarImage.sprite = content.CharacterInformation.Avatar;
            }

            // Gán dữ liệu vào prefab controller
            CharacterUIController controller = buttonGO.GetComponent<CharacterUIController>();
            if (controller != null)
            {
                controller.SetData(content);
            }
        }
    }

    void ClearContent()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in characterDisplayParent)
        {
            Destroy(child.gameObject);
        }
    }
}
