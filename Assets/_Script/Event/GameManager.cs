using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Contents SelectedCharacter; // nhân vật đã chọn

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi load scene mới
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
