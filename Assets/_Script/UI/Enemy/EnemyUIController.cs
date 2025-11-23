using UnityEngine;

public class EnemyUIController : MonoBehaviour
{
    public static EnemyUIController instance;

    public DamageNumber damageNumber;
    public EnemyHealthBarUI enemyHealthBarUIPrefab;
    public Transform EnemyUICanvas;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnDamage(float damageAmount, Vector3 location)
    {
        Transform uiGroup = CreateOrGetUIContainer("DamageUI_Clone");
        int rounded = Mathf.RoundToInt(damageAmount);
        DamageNumber newDamage = Instantiate(damageNumber, location, Quaternion.identity, uiGroup);
        newDamage.Setup(rounded);
        newDamage.gameObject.SetActive(true);
    }

    public EnemyHealthBarUI SpawnHealthBar(Transform followTarget)
    {
        Transform uiGroup = CreateOrGetUIContainer("HealthBarUI_Clone");
        EnemyHealthBarUI bar = Instantiate(enemyHealthBarUIPrefab, uiGroup);
        HealthBarFollow follow = bar.GetComponent<HealthBarFollow>();
        follow.target = followTarget;
        follow.offset = new Vector3(0, -1, 0); // Tùy chỉnh khoảng cách
        return bar;
    }

    private Transform CreateOrGetUIContainer(string name)
    {
        // Kiểm tra trong EnemyUICanvas đã có container này chưa
        Transform existing = EnemyUICanvas.Find(name);
        if (existing != null)
            return existing;

        // Nếu chưa có thì tạo mới
        GameObject container = new GameObject(name);
        container.transform.SetParent(EnemyUICanvas);
        container.transform.localScale = Vector3.one;
        container.transform.localPosition = Vector3.zero;
        return container.transform;
    }
}
