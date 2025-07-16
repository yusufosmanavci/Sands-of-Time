using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    private void Update()
    {
        SetAnimation();
        AttackAimationController();
    }
    // rigidbody y de�erini kontrol edip duruma g�re d��me ya da z�plama animasyonunu oynat.
    private void SetAnimation()
    {
        if (PlayerManager.Instance.playerValues.IsAttacking)
        {
            PlayerManager.Instance.playerValues.hitBox.gameObject.SetActive(true); // Activate hitbox during attack
            PlayerManager.Instance.playerValues.rb.linearVelocity = new Vector2(0f, PlayerManager.Instance.playerValues.rb.linearVelocity.y); // Disable linear velocity during attack
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
                Vector2 attack3Direction = PlayerManager.Instance.playerValues.IsfacingRight ? Vector2.right : Vector2.left;
                float attack3Force = 5f;
                PlayerManager.Instance.playerValues.rb.AddForce(attack3Direction * attack3Force, ForceMode2D.Impulse);
                PlayerManager.Instance.playerValues.animator.Play("3rd Attack Animation");
            }
            return;
        }
        if (PlayerManager.Instance.playerValues.IsDashing && !PlayerManager.Instance.playerValues.IsAttacking)
        {
            PlayerManager.Instance.playerValues.hitBox.gameObject.SetActive(false); 
            PlayerManager.Instance.playerValues.animator.Play("Dash Animation");
            return;
        }
        if (PlayerManager.Instance.playerValues.rb.linearVelocity.y > 0)
        {
            PlayerManager.Instance.playerValues.hitBox.gameObject.SetActive(false); 
            PlayerManager.Instance.playerValues.animator.Play("Jump Animation");
            return;

        }
        if (PlayerManager.Instance.playerValues.rb.linearVelocity.y < 0)
        {
            PlayerManager.Instance.playerValues.hitBox.gameObject.SetActive(false);
            PlayerManager.Instance.playerValues.animator.Play("Fall Animation");
            return;
        }
        if (PlayerManager.Instance.playerValues.InputX != 0)
        {
            PlayerManager.Instance.playerValues.hitBox.gameObject.SetActive(false); 
            PlayerManager.Instance.playerValues.animator.Play("Run Animation");
            return;
        }
        else
        {
            PlayerManager.Instance.playerValues.hitBox.gameObject.SetActive(false); 
            PlayerManager.Instance.playerValues.animator.Play("Idle Animation");
        }
    }

    private void AttackAimationController()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && PlayerManager.Instance.playerValues.attackCooldown <= 0)
        {
            PlayerManager.Instance.playerValues.attackCount++;
            PlayerManager.Instance.playerValues.attackCooldown = 0.5f; // Reset the cooldown after an attack
            PlayerManager.Instance.playerValues.attackDelay = 0.6f; // Delay before the next attack can be initiated
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
