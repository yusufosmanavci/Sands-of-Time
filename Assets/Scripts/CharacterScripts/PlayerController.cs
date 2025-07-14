using Assets.Scripts.BossScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerValues playerValues;
    PlayerAnimations playerAnimations;
    private PlayerInput playerInput;
    public EnemyHealth enemyHealth;
    public BossHealth bossHealth;
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();


    private void Awake()
    {
        playerValues = GetComponent<PlayerValues>();
        playerAnimations = GetComponent<PlayerAnimations>();
        playerInput = new PlayerInput();
        playerInput.PlayerInputs.Enable();
        playerInput.PlayerInputs.JumpInput.performed += Jump;
    }

    private void OnDestroy()
    {
        playerInput.PlayerInputs.JumpInput.performed -= Jump;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtbox"))
        {
            EnemyController enemyController = collision.gameObject.GetComponentInParent<EnemyController>();
            if (playerValues.IsAttacking && enemyController != null && !enemyController.enemyValues.IsInAttackAnimation)
            {
                EnemyHealth enemy = enemyController.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeEnemyDamage(playerValues.playerDamage, transform.position);
                    Debug.Log("Enemy took damage from player!");
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyHurtbox"))
        {
            BossController bossController = collision.gameObject.GetComponentInParent<BossController>();
            if (playerValues.IsAttacking && bossController != null && !bossController.bossValues.IsInAttackAnimation)
            {
                BossHealth boss = bossController.GetComponent<BossHealth>();
                if (boss != null)
                {
                    boss.TakeBossDamage(playerValues.playerDamage);
                    Debug.Log("Enemy took damage from player!");
                }
            }
        }
    }


    void Update()
    {
        HorizontalMove();
        Dash();
        playerValues.IsGrounded = Physics2D.OverlapCircle(playerValues.groundCheck.position, playerValues.groundCheckRadius, playerValues.groundLayer);
        BooleanControl();
    }
    //ana if bloðu ekleyip skillleri bool deðerleri ile kontrol edip animasyonlarý burada kontrol edebilirsin.
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
                playerValues.hitBox.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); // Flip hitbox scale for left-facing
                playerValues.IsfacingRight = false;
                playerValues.spriteRenderer.flipX = true;
            }
            else if (!playerValues.IsfacingRight && playerValues.InputX > 0)
            {
                playerValues.hitBox.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Reset hitbox scale for right-facing
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

            float actualDashSpeed = playerValues.IsKnockbacked ? playerValues.dashSpeed * 0.5f : playerValues.dashSpeed;
            playerValues.rb.AddForce(dashDirection * actualDashSpeed, ForceMode2D.Impulse);

            playerValues.currentDashCooldown = playerValues.dashCooldown;
            playerValues.dashDuration = 0.2f; // Dash duration can be set to a specific value
            //Spesifik deðer girdim o yüzden ilerideki durumlara göre bu deðeri deðiþtirebilirsin.
            playerValues.dashTime = playerValues.dashDuration * 1.8f;

        }
        if (playerValues.IsDashing)
        {
            DashAttack();
            DashBossAttack();
            gameObject.layer = LayerMask.NameToLayer(playerValues.dashLayer); // Set layer to PlayerDashing during dash
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer(playerValues.defaultLayer); // Reset layer to default when not dashing
            playerValues.hasDealtDashDamage = false; // Reset the dash damage flag when not dashing
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

    void DashAttack()
    {
        float direction = transform.localScale.x;
        Vector2 center = (Vector2)transform.position + new Vector2(direction * 0.5f, 0);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(center, playerValues.dashAttackRadius, playerValues.enemyLayerMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            GameObject enemyGO = enemy.gameObject;
            if (!damagedEnemies.Contains(enemyGO))
            {
                EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();
                if (enemyScript != null)
                {
                    enemyScript.TakeEnemyDashDamage(playerValues.playerDamage, transform.position);
                    damagedEnemies.Add(enemyGO); // tekrar vurulmasýný engelle
                }
            }
        }
    }
    void DashBossAttack()
    {
        if (playerValues.hasDealtDashDamage) return; // Daha önce hasar verdiyse çýk

        float direction = transform.localScale.x;
        Vector2 center = (Vector2)transform.position + new Vector2(direction * 1f, 0);
        Collider2D[] hitBosses = Physics2D.OverlapCircleAll(center, playerValues.dashAttackRadius, playerValues.enemyLayerMask);

        foreach (Collider2D boss in hitBosses)
        {
            BossHealth bossScript = boss.GetComponent<BossHealth>();
            if (bossScript != null)
            {
                bossScript.TakeBossDashDamage(playerValues.playerDamage);
                playerValues.hasDealtDashDamage = true; // Hasar verildiði iþaretleniyor
                break; // Ýlk düþmana vurunca çýk
            }
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
            damagedEnemies.Clear(); // Dash bitti, sonraki dash için sýfýrla
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
