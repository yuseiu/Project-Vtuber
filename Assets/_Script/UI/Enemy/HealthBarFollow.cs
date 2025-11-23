using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform target;     // Enemy to follow
    public Vector3 offset;       // Offset from enemy position

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
