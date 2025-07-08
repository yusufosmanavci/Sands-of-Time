using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerValues playerValues;
    private void Awake()
    {
        playerValues = GetComponent<PlayerValues>();
    }
    private void Update()
    {
        SetAnimation();
        AttackAimationController();
    }
    // rigidbody y deðerini kontrol edip duruma göre düþme ya da zýplama animasyonunu oynat.
    private void SetAnimation()
    {
        if (playerValues.IsAttacking)
        {
            playerValues.rb.linearVelocity = new Vector2(0f, playerValues.rb.linearVelocity.y); // Disable linear velocity during attack
            if (playerValues.attackCount == 1)
            {
                playerValues.animator.Play("1st Attack Animation");
            }
            else if (playerValues.attackCount == 2)
            {
                playerValues.animator.Play("2nd Attack Animation");
            }
            else if (playerValues.attackCount == 3)
            {
                Vector2 attack3Direction = playerValues.IsfacingRight ? Vector2.right : Vector2.left;
                float attack3Force = 5f;
                playerValues.rb.AddForce(attack3Direction * attack3Force, ForceMode2D.Impulse);
                playerValues.animator.Play("3rd Attack Animation");
            }
            return;
        }
        if (playerValues.IsDashing && !playerValues.IsAttacking)
        {
            playerValues.animator.Play("Dash Animation");
            return;
        }
        if (playerValues.rb.linearVelocity.y > 0)
        {
            playerValues.animator.Play("Jump Animation");
            return;

        }
        if (playerValues.rb.linearVelocity.y < 0)
        {
            playerValues.animator.Play("Fall Animation");
            return;
        }
        if (playerValues.InputX != 0)
        {
            playerValues.animator.Play("Run Animation");
            return;
        }
        else
        {
            playerValues.animator.Play("Idle Animation");
        }
    }

    private void AttackAimationController()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && playerValues.attackCooldown <= 0)
        {
            playerValues.attackCount++;
            playerValues.attackCooldown = 0.5f; // Reset the cooldown after an attack
            playerValues.attackDelay = 0.4f; // Delay before the next attack can be initiated
            playerValues.attackResetTime = 1.5f; // Time to reset attack count after the last attack
            playerValues.IsAttacking = true;
        }
        if (playerValues.attackDelay > 0)
        {
            playerValues.attackDelay -= Time.deltaTime;
            if (playerValues.attackDelay <= 0)
            {
                playerValues.IsAttacking = false; // Reset attacking state after delay
                playerValues.attackDelay = 0; // Reset attack delay
            }
        }
        if (playerValues.attackCooldown >= 0)
        {
            playerValues.attackCooldown -= Time.deltaTime;
        }
        if (playerValues.attackResetTime >= 0)
        {
            playerValues.attackResetTime -= Time.deltaTime;
            if (playerValues.attackResetTime <= 0)
            {
                playerValues.attackCount = 0; // Reset attack count after reset time
                playerValues.attackResetTime = 0; // Reset attack reset time
            }
        }
        if (playerValues.attackCount > 3)
        {
            playerValues.attackCount = 1; // Reset attack count after 3 attacks
        }
    }
}
