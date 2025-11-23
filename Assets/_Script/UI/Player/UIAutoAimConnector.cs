using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIAutoAimConnector : MonoBehaviour
{
    [Header("UI")]
    public Toggle autoAimToggle;

    [Header("Arrow có sẵn (tuỳ chọn, để test)")]
    public GameObject arrow;

    private ArrowController currentArrowController;

    private bool hasConnected = false;

    void Start()
    {
        autoAimToggle.onValueChanged.AddListener(OnToggleChanged);

        // Nếu CharacterSpawner không gán gì, sau 1 frame sẽ tự tìm arrow test
        StartCoroutine(ConnectFallbackArrowAfterDelay());
    }

    // Được gọi từ CharacterSpawner
    public void SetArrowController(ArrowController arrowController)
    {
        if (arrowController == null) return;

        currentArrowController = arrowController;
        currentArrowController.SetAutoAim(autoAimToggle.isOn);
        hasConnected = true;

        Debug.Log("[UIAutoAimConnector] Arrow đã được gán từ CharacterSpawner");
    }

    private void OnToggleChanged(bool isOn)
    {
        if (currentArrowController != null)
        {
            currentArrowController.SetAutoAim(isOn);
        }
    }

    // Sau 1 frame, nếu chưa có arrow nào từ spawn, gán arrow có sẵn trong scene
    IEnumerator ConnectFallbackArrowAfterDelay()
    {
        yield return null; // chờ 1 frame

        if (!hasConnected && arrow != null)
        {
            ArrowController fallback = arrow.GetComponent<ArrowController>();
            if (fallback != null)
            {
                currentArrowController = fallback;
                currentArrowController.SetAutoAim(autoAimToggle.isOn);
                Debug.Log("[UIAutoAimConnector] Gán arrow test từ scene");
            }
        }
    }
}
