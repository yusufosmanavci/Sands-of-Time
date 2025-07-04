using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 15f;
    public float InputX;
    public float dashCooldown;
    public float currentDashCooldown;
    public float groundCheckRadius = 0.2f;
    public float dashDuration = 0.2f;
    public float dashTime = 0f;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public bool IsGrounded;
    public bool IsfacingRight = true;
    public bool IsDashing = false;
    public bool IsRunning = false;
    public bool IsJumped = false;
    /*public bool IsAttacking = false;
    public bool IsUsingSkill = false;
    public bool IsUsingSkill2 = false;
    public bool IsUsingSkill3 = false;
    public bool IsUsingSkill4 = false;*/
}
