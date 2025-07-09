using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerValues playerValues;
    PlayerAnimations playerAnimations;
    private PlayerInput playerInput;
    public EnemyHealth enemyHealth;

    private void Awake()
    {
        playerValues = GetComponent<PlayerValues>();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerInput = new PlayerInput();
        playerInput.PlayerInputs.Enable();
        playerValues.hitBox = GetComponentInChildren<BoxCollider2D>();
        playerInput.PlayerInputs.JumpInput.performed += Jump;
    }

    private void OnDestroy()
    {
        playerInput.PlayerInputs.JumpInput.performed -= Jump;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerValues.IsAttacking || playerValues.IsDashing)
        {
            enemyHealth.TakeEnemyDamage(playerValues.playerDamage, transform.position, playerValues.IsDashing);
            Debug.Log("Enemy took damage from player!");
        }
    }

    void Update()
    {
        HorizontalMove();
        Dash();
        playerValues.IsGrounded = Physics2D.OverlapCircle(playerValues.groundCheck.position, playerValues.groundCheckRadius, playerValues.groundLayer);
        BooleanControl();
    }
    //ana if blo�u ekleyip skillleri bool de�erleri ile kontrol edip animasyonlar� burada kontrol edebilirsin.
    private void HorizontalMove()
    {
        if (playerValues.IsKnockbacked)
            return;
        if(playerValues.dashDuration > 0)
        {
            playerValues.dashDuration -= Time.deltaTime;
        }
        else
        {
            playerValues.InputX = playerInput.PlayerInputs.MoveInputs.ReadValue<Vector2>().x;
            playerValues.rb.linearVelocity = new Vector2(playerValues.InputX * playerValues.moveSpeed, playerValues.rb.linearVelocity.y);
            if (playerValues.IsfacingRight && playerValues.InputX < 0)
            {
                playerValues.IsfacingRight = false;
                playerValues.spriteRenderer.flipX = true;
            }
            else if (!playerValues.IsfacingRight && playerValues.InputX > 0)
            {
                playerValues.IsfacingRight = true;
                playerValues.spriteRenderer.flipX = false;

            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!playerValues.IsDashing)
        {
            if (context.ReadValueAsButton() && playerValues.IsGrounded)
            {
                playerValues.rb.AddForce(Vector2.up * playerValues.jumpForce, ForceMode2D.Impulse);
            }
        }
        else
            return;
    }

    private void Dash()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && playerValues.currentDashCooldown == 0 && !playerValues.IsAttacking)
        {
            playerValues.rb.linearVelocity = Vector2.zero; // Reset velocity to prevent sliding during dash
            Vector2 dashDirection = playerValues.IsfacingRight ? Vector2.right : Vector2.left;
            playerValues.rb.AddForce(dashDirection * playerValues.dashSpeed, ForceMode2D.Impulse);
            playerValues.currentDashCooldown = playerValues.dashCooldown;
            playerValues.dashDuration = 0.2f; // Dash duration can be set to a specific value
            //Spesifik de�er girdim o y�zden ilerideki durumlara g�re bu de�eri de�i�tirebilirsin.
            playerValues.dashTime = playerValues.dashDuration * 1.8f;

        }
        if (playerValues.currentDashCooldown > 0)
        {
            playerValues.currentDashCooldown -= Time.deltaTime;
            playerValues.dashTime -= Time.deltaTime;
        }
        else
        {
            playerValues.currentDashCooldown = 0;
            playerValues.dashTime = 0;
        }
    }

    private void BooleanControl()
    {
        if (!playerValues.IsGrounded)
        {
            playerValues.IsJumped = true;
        }
        else
            playerValues.IsJumped = false;

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && playerValues.dashTime >= 0)
        {
            playerValues.IsDashing = true;
        }
        else if (playerValues.dashTime <= 0)
        {
            playerValues.IsDashing = false;
        }
    }

    public IEnumerator KnockbackRoutine (Vector2 force)
    {
        playerValues.IsKnockbacked = true;
        playerValues.rb.AddForce(force, ForceMode2D.Impulse);
        playerValues.rb.linearVelocity = Vector2.zero; // Reset velocity to prevent sliding
        yield return new WaitForSeconds(0.5f); // Adjust the duration of the knockback effect
        playerValues.IsKnockbacked = false; // Reset the knockback state after the effect
    }
}
