using UnityEngine;
using Cinemachine;

public class CharacterSpawner : MonoBehaviour
{
    public Transform spawnPoint;         // Chỉ đến GameObject "Player"
    public GameObject arrowPrefab;       // Kéo Arrow prefab vào đây

    void Awake()
    {
        if (GameManager.Instance != null && GameManager.Instance.SelectedCharacter != null)
        {
            // Xoá tất cả con trong Player (spawnPoint)
            foreach (Transform child in spawnPoint)
            {
                Destroy(child.gameObject);
            }

            // Spawn Dakki làm con của Player
            GameObject newCharacter = Instantiate(
                GameManager.Instance.SelectedCharacter.CharacterInformation.Prefab,
                spawnPoint.position,
                Quaternion.identity,
                spawnPoint
            );

            // Spawn Arrow làm con của Player
            if (arrowPrefab != null)
            {
                GameObject arrow = Instantiate(arrowPrefab, spawnPoint);
                arrow.transform.localPosition = Vector3.zero;

                var arrowController = arrow.GetComponent<ArrowController>();
                if (arrowController != null)
                {
                    arrowController.player = newCharacter.transform;
                }
                // Tìm UIAutoAimConnector trong scene để kết nối Toggle
                UIAutoAimConnector uiConnector = FindObjectOfType<UIAutoAimConnector>();
                if (uiConnector != null)
                {
                    uiConnector.SetArrowController(arrowController);
                }
                var skillManager = newCharacter.GetComponent<SkillManager>();
                skillManager.arrowAimDirection = arrow.transform;
            }

            // Gán camera
            var vcam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = newCharacter.transform;
                vcam.LookAt = newCharacter.transform;
            }
        }
        else
        {
            Debug.Log("Không có nhân vật được chọn.Dùng dữ liệu trong scene");
        }
    }
}
