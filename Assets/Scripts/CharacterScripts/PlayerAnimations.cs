using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    private void Update()
    {
        SetAnimation();
        AttackAimationController();
    }
    // rigidbody y deðerini kontrol edip duruma göre düþme ya da zýplama animasyonunu oynat.
    private void SetAnimation()
    {
        if (PlayerManager.Instance.playerValues.IsKnockbacked)
        {
            if(PlayerManager.Instance.playerValues.rb.linearVelocity.y > 0)
            {
                PlayerManager.Instance.playerValues.animator.Play("Hit Animation");
            }
            if(PlayerManager.Instance.playerValues.rb.linearVelocity.y < 0)
            {
                PlayerManager.Instance.playerValues.animator.Play("Hit Animation Down");
            }
            return;
        }
        if (PlayerManager.Instance.playerValues.IsAttacking && PlayerManager.Instance.playerValues.IsGrounded)
        {
            if (PlayerManager.Instance.playerValues.attackCount == 1)
            {
                PlayerManager.Instance.playerValues.animator.Play("1st Attack Animation");
            }
            else if (PlayerManager.Instance.playerValues.attackCount == 2)
            {
                PlayerManager.Instance.playerValues.animator.Play("2nd Attack Animation");
            }
            else if (PlayerManager.Instance.playerValues.attackCount == 3)
            {
                PlayerManager.Instance.playerValues.animator.Play("3rd Attack Animation");
            }
            return;
        }
        if (PlayerManager.Instance.playerValues.IsDashing && !PlayerManager.Instance.playerValues.IsAttacking)
        {
            PlayerManager.Instance.playerValues.animator.Play("Dash Animation");
            return;
        }
        if (PlayerManager.Instance.playerValues.rb.linearVelocity.y > 0 && !PlayerManager.Instance.playerValues.IsGrounded)
        {
            PlayerManager.Instance.playerValues.animator.Play("Jump Animation");
            return;

        }
        if (PlayerManager.Instance.playerValues.rb.linearVelocity.y < 0 && !PlayerManager.Instance.playerValues.IsGrounded)
        {
            PlayerManager.Instance.playerValues.animator.Play("Fall Animation");
            return;
        }
        if (PlayerManager.Instance.playerValues.InputX != 0)
        {
            PlayerManager.Instance.playerValues.animator.Play("Run Animation");
            return;
        }
        else
        {
            PlayerManager.Instance.playerValues.animator.Play("Idle Animation");
        }
    }

    private void AttackAimationController()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && PlayerManager.Instance.playerValues.attackCooldown <= 0)
        {
            PlayerManager.Instance.playerValues.attackCount++;
            PlayerManager.Instance.playerValues.attackCooldown = 0.3f; // Reset the cooldown after an attack
            PlayerManager.Instance.playerValues.attackDelay = 0.3f; // Delay before the next attack can be initiated
            PlayerManager.Instance.playerValues.attackResetTime = 1.5f; // Time to reset attack count after the last attack
            PlayerManager.Instance.playerValues.IsAttacking = true;
        }
        if (PlayerManager.Instance.playerValues.attackDelay > 0)
        {
            PlayerManager.Instance.playerValues.attackDelay -= Time.deltaTime;
            if (PlayerManager.Instance.playerValues.attackDelay <= 0)
            {
                PlayerManager.Instance.playerValues.IsAttacking = false; // Reset attacking state after delay
                PlayerManager.Instance.playerValues.attackDelay = 0; // Reset attack delay
            }
        }
        if (PlayerManager.Instance.playerValues.attackCooldown >= 0)
        {
            PlayerManager.Instance.playerValues.attackCooldown -= Time.deltaTime;
        }
        if (PlayerManager.Instance.playerValues.attackResetTime >= 0)
        {
            PlayerManager.Instance.playerValues.attackResetTime -= Time.deltaTime;
            if (PlayerManager.Instance.playerValues.attackResetTime <= 0)
            {
                PlayerManager.Instance.playerValues.attackCount = 0; // Reset attack count after reset time
                PlayerManager.Instance.playerValues.attackResetTime = 0; // Reset attack reset time
            }
        }
        if (PlayerManager.Instance.playerValues.attackCount > 3)
        {
            PlayerManager.Instance.playerValues.attackCount = 1; // Reset attack count after 3 attacks
        }
    }
}
