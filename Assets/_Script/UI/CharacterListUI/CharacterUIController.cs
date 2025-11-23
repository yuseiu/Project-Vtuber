using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIController : MonoBehaviour
{
    private Contents characterContents;

    public void SetData(Contents contents)
    {
        this.characterContents = contents;
    }

    public void ShowCharacter()
    {
        GameManager.Instance.SelectedCharacter = characterContents;
        if (characterContents == null || characterContents.CharacterInformation == null)
        {
            Debug.LogWarning("Character contents is null or invalid.");
            return;
        }

        Debug.Log("Clicked on character: " + characterContents.CharacterInformation.Name);

        // Xoá nhân vật cũ nếu có
        if (CharacterListUI.Instance.currentCharacterGO != null)
        {
            Destroy(CharacterListUI.Instance.currentCharacterGO);
        }

        // Tạo nhân vật mới nếu có prefab
        if (characterContents.CharacterInformation.Prefab != null)
        {
            GameObject character = Instantiate(
                characterContents.CharacterInformation.Prefab,
                CharacterListUI.Instance.characterDisplayParent
            );
            CharacterListUI.Instance.currentCharacterGO = character;
            ScriptDisabler.Instance.DisableScriptsOn(character);
            // 👉 Gọi animation nếu có
            //Transform mainTransform = character.transform.Find("Main");
            if (character != null)
            {
                Animator animator = character.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Move");
                }
                else
                {
                    Debug.LogWarning("Animator không tìm thấy trong Main.");
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy GameObject con 'Main' trong prefab.");
            }

        }
        // 👉 Nếu bạn có UI tăng stat:
        if (CharacterListUI.Instance.statUIController != null)
        {
            CharacterListUI.Instance.statUIController.gameObject.SetActive(true);
            StatAdjustUIController adjustUI = CharacterListUI.Instance.statUIController.GetComponent<StatAdjustUIController>();
            if (adjustUI != null)
            {
                adjustUI.LoadCharacter(characterContents);
            }
        }
    }
}
