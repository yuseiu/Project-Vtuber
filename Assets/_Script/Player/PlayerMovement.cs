using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] KeySO keyBindings; // Thêm ScriptableObject Keybinding

    Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveSpeed = PlayerStat.Instance.TotalStats.Speed;
    }

    void Update()
    {
        movement = Vector2.zero; // Reset movement mỗi frame

        // Lấy phím di chuyển từ ScriptableObject
        if (Input.GetKey(keyBindings.GetKey("MoveUp")))
            movement.y = 1;
        if (Input.GetKey(keyBindings.GetKey("MoveDown")))
            movement.y = -1;
        if (Input.GetKey(keyBindings.GetKey("MoveLeft")))
            movement.x = -1;
        if (Input.GetKey(keyBindings.GetKey("MoveRight")))
            movement.x = 1;

        movement.Normalize(); // Để di chuyển đều khi đi chéo
        if(movement != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("InputX", movement.x);
            animator.SetFloat("InputY", movement.y);
            animator.SetFloat("LastInputX", movement.x);
            animator.SetFloat("LastInputY", movement.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.LogError("No Rigidbody2D found in PlayerMovement.cs");
        }
    }
}
