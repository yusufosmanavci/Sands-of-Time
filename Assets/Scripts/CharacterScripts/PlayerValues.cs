using NUnit.Framework;
using UnityEngine;
using Unity.Cinemachine;

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
    public int attackCount = 0;
    public float attackDelay = 0; // Adjust the delay as needed
    public float attackCooldown = 0; // Cooldown time for attacks
    public float attackResetTime = 0; // Time to reset attack count after the last attack
    public float playerDamage = 30f; // Damage dealt by the player
    public float dashAttackRadius = 1.5f;
    public int sandsOfTime = 0; // Represents the number of "sand of time" items collected
    public float hitboxRadius = 1f; // Radius for the hitbox detection
    public int jumpCount = 0; // Number of jumps performed
    public static Vector2 lastCheckpointPosition; // Last checkpoint position for respawning
    public static CinemachineConfiner2D cameraConfiner; // Reference to the Cinemachine confiner for camera boundaries

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public LayerMask enemyLayerMask;
    public Collider2D playerCollider;
    public Transform player;

    public string defaultLayer = "PlayerHurtbox";
    public string dashLayer = "DashingPlayer";

    public bool IsGrounded;
    public bool IsfacingRight = true;
    public bool IsDashing = false;
    public bool IsRunning = false;
    public bool IsJumped = false;
    public bool IsAttacking = false;
    public bool IsKnockbacked = false;
    public bool hasDealtDashDamage = false;
    public bool IsSwordAttacking = false;
    public bool IsDead = false;

    /*public bool IsUsingSkill = false;
    public bool IsUsingSkill2 = false;
    public bool IsUsingSkill3 = false;
    public bool IsUsingSkill4 = false;*/
}
