using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 15f;
    public float InputX;
    public float dashCooldown;
    public float currentDashCooldown;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public bool IsJumpPressed => Input.GetKeyDown(KeyCode.Space);
    public bool IsGrounded => Physics2D.Raycast(rb.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

    public bool IsfacingRight = true;
}
