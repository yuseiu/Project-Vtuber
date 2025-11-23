using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitiationEvent : MonoBehaviour
{
    private void Awake()
    {
        CharacterDataHandle.Instance.InitializeCharacterData();
        SetTotalStat();

    }
    public void StartGame()
    {
        var selected = GameManager.Instance.SelectedCharacter;
        if (selected.CharacterInformation.Prefab != null)
        {
            SceneManager.LoadScene("InGamePlay"); // thay bằng tên scene thực tế
        }
        else { Debug.Log("Bạn chưa chọn nhân vật"); }
    }
    public void SetTotalStat()
    {
        foreach(var content in CharacterDataHandle.Instance.characterDataInGame.Content)
        {
            content.RecalculateTotalStat();
        }
    }
}
