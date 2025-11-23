using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [Header("Item Collect Settings")]
    public float pickupRange = 5f;
    public LayerMask itemLayer;
    public float pickupSpeed = 7f;
    private HashSet<Transform> attractingItems = new HashSet<Transform>();

    void Update()
    {
        Vector2 boxSize = new Vector2(pickupRange * 2f, pickupRange * 1.1f);
        Collider2D[] itemsInRange = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, itemLayer);

        // Thêm item vào danh sách hút nếu phát hiện
        foreach (var itemCol in itemsInRange)
        {
            if (!attractingItems.Contains(itemCol.transform))
            {
                attractingItems.Add(itemCol.transform);
            }
        }

        // Di chuyển tất cả item đang được hút
        var toRemove = new List<Transform>();
        foreach (var item in attractingItems)
        {
            if (item == null)
            {
                toRemove.Add(item);
                continue;
            }

            item.position = Vector3.MoveTowards(item.position, transform.position, pickupSpeed * Time.deltaTime);

            // Khi item đủ gần, tự kích hoạt và xóa khỏi danh sách
            if (Vector2.Distance(item.position, transform.position) < 0.1f)
            {
                var activate = item.GetComponent<ItemActivate>();
                if (activate != null)
                {
                    activate.EffectActivate();
                }
                toRemove.Add(item);
            }
        }

        // Xóa item đã nhặt hoặc bị hủy
        foreach (var item in toRemove)
        {
            attractingItems.Remove(item);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector2 boxSize = new Vector2(pickupRange * 2f, pickupRange * 1.1f);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            var item = collision.GetComponent<ItemActivate>();
            if (item != null)
            {
                // Có thể sau này cho vào inventory thay vì dùng ngay
                item.EffectActivate();
            }
        }
    }
}
