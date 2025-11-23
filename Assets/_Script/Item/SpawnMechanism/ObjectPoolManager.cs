using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CreateObject
{
    public GameObject gameObject;
    public int quantity;
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [Header("Danh sách prefab cần đưa vào pool")]
    public List<CreateObject> createObjects;

    private Transform rootContainer;
    private Dictionary<string, Transform> containers = new Dictionary<string, Transform>();
    private Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();

    void Awake()
    {
        Instance = this;
        InitContainers();
        CreateObjects(createObjects);
    }

    void Start()
    {
        
    }

    // B1: Container gốc
    void InitContainers()
    {
        rootContainer = CreateContainer("ObjectPool", null);
    }

    Transform CreateContainer(string name, Transform parent)
    {
        GameObject container = new GameObject(name);
        if (parent != null) container.transform.SetParent(parent);
        containers[name] = container.transform;
        return container.transform;
    }

    // B2: Tạo object và phân loại theo Tag -> objectName
    public void CreateObjects(List<CreateObject> objectList)
    {
        foreach (var obj in objectList)
        {
            for (int i = 0; i < obj.quantity; i++)
            {
                GameObject newObj = Instantiate(obj.gameObject);
                newObj.SetActive(false);

                string tagName = null;
                string objectName = null;

                // Nếu là Item
                if (newObj.CompareTag("Item"))
                {
                    ItemActivate itemAct = newObj.GetComponent<ItemActivate>();
                    if (itemAct != null && itemAct.itemData != null)
                    {
                        tagName = itemAct.itemData.tag.ToString();
                        objectName = itemAct.itemData.itemName;
                    }
                }
                // Nếu là Enemy
                else if (newObj.CompareTag("Enemy"))
                {
                    EnemyStats enemyStats = newObj.GetComponent<EnemyStats>();
                    if (enemyStats != null && enemyStats.enemyInformation != null)
                    {
                        tagName = enemyStats.enemyInformation.tag.ToString();
                        objectName = enemyStats.enemyInformation.Name;
                    }
                }
                // Có thể mở rộng thêm Projectile, Effect...
                // else if (newObj.CompareTag("Projectile")) { ... }

                if (string.IsNullOrEmpty(tagName) || string.IsNullOrEmpty(objectName))
                {
                    Debug.LogWarning($"{obj.gameObject.name} không có dữ liệu hợp lệ để đưa vào pool!");
                    Destroy(newObj);
                    continue;
                }

                string itemKey = tagName + "_" + objectName;

                // Container Tag
                if (!containers.ContainsKey(tagName))
                {
                    CreateContainer(tagName, rootContainer);
                }

                // Container objectName
                if (!containers.ContainsKey(itemKey))
                {
                    Transform itemContainer = CreateContainer(objectName, containers[tagName]);
                    containers[itemKey] = itemContainer;
                }

                // Lưu prefab gốc (chỉ 1 lần)
                if (!prefabLookup.ContainsKey(itemKey))
                {
                    prefabLookup[itemKey] = obj.gameObject;
                }

                // Đưa object vào container
                newObj.transform.SetParent(containers[itemKey]);
            }
        }
    }

    // B3: Lấy object từ pool
    public GameObject GetPool(string tagName, string objectName, Vector3 position)
    {
        string itemKey = tagName + "_" + objectName;

        if (!containers.ContainsKey(itemKey))
        {
            Debug.LogError($"[ObjectPool] Không tìm thấy container: {itemKey}");
            return null;
        }

        Transform parent = containers[itemKey];

        // Tìm object chưa active
        foreach (Transform child in parent)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
                child.position = position;
                return child.gameObject;
            }
        }

        // Không còn object rảnh -> tạo mới từ prefabLookup
        if (prefabLookup.TryGetValue(itemKey, out GameObject prefab))
        {
            GameObject newObj = Instantiate(prefab, position, Quaternion.identity, parent);
            newObj.SetActive(true);
            return newObj;
        }

        Debug.LogError($"[ObjectPool] Không tìm thấy prefab cho {itemKey}");
        return null;
    }

    // B4: Trả object về pool
    public void ReturnPool(GameObject obj)
    {
        obj.SetActive(false);
        // Parent giữ nguyên vì đã nằm trong container
    }

    // ==========================
    // Bổ sung: tiện ích cho Wave
    // ==========================

    // Kiểm tra container có tồn tại không
    public bool HasContainer(string key)
    {
        return containers.ContainsKey(key);
    }

    // Lấy container theo key
    public Transform GetContainer(string key)
    {
        if (containers.TryGetValue(key, out Transform container))
            return container;
        return null;
    }

    // Đăng ký container thủ công (ít khi cần)
    public void RegisterContainer(string key, Transform container)
    {
        if (!containers.ContainsKey(key))
        {
            containers[key] = container;
        }
    }
}
