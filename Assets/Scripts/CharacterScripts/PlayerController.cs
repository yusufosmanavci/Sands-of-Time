using Assets.Scripts.BossScripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    public BossHealth bossHealth;
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();
    private HashSet<GameObject> damagedEnemies2 = new HashSet<GameObject>();

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerInputs.Enable();
        playerInput.PlayerInputs.JumpInput.performed += Jump;
    }


    private void OnDestroy()
    {
        playerInput.PlayerInputs.JumpInput.performed -= Jump;
    }



    void Update()
    {
        HorizontalMove();
        Dash();
        PlayerManager.Instance.playerValues.IsGrounded = Physics2D.OverlapCircle(PlayerManager.Instance.playerValues.groundCheck.position, PlayerManager.Instance.playerValues.groundCheckRadius, PlayerManager.Instance.playerValues.groundLayer);
        if (PlayerManager.Instance.playerValues.IsAttacking && PlayerManager.Instance.playerValues.IsGrounded && !PlayerManager.Instance.playerValues.IsKnockbacked && !PlayerManager.Instance.playerValues.IsSwordAttacking)
        {
            PlayerManager.Instance.playerValues.rb.linearVelocity = Vector2.zero;
            StartCoroutine(SwordAttack());
        }
        BooleanControl();

    }
    //ana if bloðu ekleyip skillleri bool deðerleri ile kontrol edip animasyonlarý burada kontrol edebilirsin.
    private void HorizontalMove()
    {
        if (PlayerManager.Instance.playerValues.IsKnockbacked || PlayerManager.Instance.playerValues.IsAttacking)
            return;
        if (PlayerManager.Instance.playerValues.dashDuration > 0)
        {
            PlayerManager.Instance.playerValues.dashDuration -= Time.deltaTime;
        }
        else
        {
            PlayerManager.Instance.playerValues.InputX = playerInput.PlayerInputs.MoveInputs.ReadValue<Vector2>().x;
            PlayerManager.Instance.playerValues.rb.linearVelocity = new Vector2(PlayerManager.Instance.playerValues.InputX * PlayerManager.Instance.playerValues.moveSpeed, PlayerManager.Instance.playerValues.rb.linearVelocity.y);
            if (PlayerManager.Instance.playerValues.IsfacingRight && PlayerManager.Instance.playerValues.InputX < 0)
            {
                PlayerManager.Instance.playerValues.IsfacingRight = false;
                PlayerManager.Instance.playerValues.spriteRenderer.flipX = true;
            }
            else if (!PlayerManager.Instance.playerValues.IsfacingRight && PlayerManager.Instance.playerValues.InputX > 0)
            {
                PlayerManager.Instance.playerValues.IsfacingRight = true;
                PlayerManager.Instance.playerValues.spriteRenderer.flipX = false;

            }
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!PlayerManager.Instance.playerValues.IsDashing)
        {
            if (context.ReadValueAsButton() && PlayerManager.Instance.playerValues.jumpCount <= 1 && PlayerManager.Instance.playerValues.jumpCount > 0)
            {
                PlayerManager.Instance.playerValues.jumpCount--;
                PlayerManager.Instance.playerValues.rb.AddForce(Vector2.up * PlayerManager.Instance.playerValues.jumpForce, ForceMode2D.Impulse);
            }
        }
        else
            return;
    }

    private void Dash()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && PlayerManager.Instance.playerValues.currentDashCooldown == 0 && !PlayerManager.Instance.playerValues.IsAttacking && !PlayerManager.Instance.playerValues.IsKnockbacked)
        {
            PlayerManager.Instance.playerValues.rb.linearVelocity = Vector2.zero; // Reset velocity to prevent sliding during dash
            Vector2 dashDirection = PlayerManager.Instance.playerValues.IsfacingRight ? Vector2.right : Vector2.left;

            float actualDashSpeed = PlayerManager.Instance.playerValues.IsKnockbacked ? PlayerManager.Instance.playerValues.dashSpeed * 0.5f : PlayerManager.Instance.playerValues.dashSpeed;
            PlayerManager.Instance.playerValues.rb.AddForce(dashDirection * actualDashSpeed, ForceMode2D.Impulse);

            PlayerManager.Instance.playerValues.currentDashCooldown = PlayerManager.Instance.playerValues.dashCooldown;
            PlayerManager.Instance.playerValues.dashDuration = 0.2f; // Dash duration can be set to a specific value
            //Spesifik deðer girdim o yüzden ilerideki durumlara göre bu deðeri deðiþtirebilirsin.
            PlayerManager.Instance.playerValues.dashTime = PlayerManager.Instance.playerValues.dashDuration * 1.8f;

        }
        if (PlayerManager.Instance.playerValues.IsDashing)
        {
            DashAttack();
            DashBossAttack();
            gameObject.layer = LayerMask.NameToLayer(PlayerManager.Instance.playerValues.dashLayer); // Set layer to PlayerDashing during dash
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer(PlayerManager.Instance.playerValues.defaultLayer); // Reset layer to default when not dashing
            PlayerManager.Instance.playerValues.hasDealtDashDamage = false; // Reset the dash damage flag when not dashing
        }
        if (PlayerManager.Instance.playerValues.currentDashCooldown > 0)
        {
            PlayerManager.Instance.playerValues.currentDashCooldown -= Time.deltaTime;
            PlayerManager.Instance.playerValues.dashTime -= Time.deltaTime;
        }
        else
        {
            PlayerManager.Instance.playerValues.currentDashCooldown = 0;
            PlayerManager.Instance.playerValues.dashTime = 0;
        }
    }

    void DashAttack()
    {
        float direction = transform.localScale.x;
        Vector2 center = (Vector2)transform.position + new Vector2(direction * 0.5f, 0);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(center, PlayerManager.Instance.playerValues.dashAttackRadius, PlayerManager.Instance.playerValues.enemyLayerMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            GameObject enemyGO = enemy.gameObject;
            if (!damagedEnemies.Contains(enemyGO))
            {
                EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();
                if (enemyScript != null)
                {
                    StartCoroutine(enemyScript.TakeEnemyDashDamage(PlayerManager.Instance.playerValues.playerDamage));
                    damagedEnemies.Add(enemyGO); // tekrar vurulmasýný engelle
                }
            }
        }
    }
    void DashBossAttack()
    {
        if (PlayerManager.Instance.playerValues.hasDealtDashDamage) return; // Daha önce hasar verdiyse çýk

        float direction = transform.localScale.x;
        Vector2 center = (Vector2)transform.position + new Vector2(direction * 1f, 0);
        Collider2D[] hitBosses = Physics2D.OverlapCircleAll(center, PlayerManager.Instance.playerValues.dashAttackRadius, PlayerManager.Instance.playerValues.enemyLayerMask);

        foreach (Collider2D boss in hitBosses)
        {
            BossHealth bossScript = boss.GetComponent<BossHealth>();
            if (bossScript != null)
            {
                bossScript.TakeBossDashDamage(PlayerManager.Instance.playerValues.playerDamage);
                PlayerManager.Instance.playerValues.hasDealtDashDamage = true; // Hasar verildiði iþaretleniyor
                break; // Ýlk düþmana vurunca çýk
            }
        }
    }


    private void BooleanControl()
    {
        if (!PlayerManager.Instance.playerValues.IsGrounded)
        {
            PlayerManager.Instance.playerValues.IsJumped = true;
        }
        else
            PlayerManager.Instance.playerValues.IsJumped = false;

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && PlayerManager.Instance.playerValues.dashTime >= 0)
        {
            PlayerManager.Instance.playerValues.IsDashing = true;
        }
        else if (PlayerManager.Instance.playerValues.dashTime <= 0)
        {
            PlayerManager.Instance.playerValues.IsDashing = false;
            damagedEnemies.Clear(); // Dash bitti, sonraki dash için sýfýrla
        }
        if(PlayerManager.Instance.playerValues.IsGrounded)
        {
            PlayerManager.Instance.playerValues.jumpCount = 1; // Zeminle temas edince zýplama sayýsýný artýr.
        }
            
    }

    public IEnumerator KnockbackRoutine(Vector2 force)
    {
        PlayerManager.Instance.playerValues.IsKnockbacked = true;
        PlayerManager.Instance.playerValues.rb.AddForce(force, ForceMode2D.Impulse);
        PlayerManager.Instance.playerValues.rb.linearVelocity = Vector2.zero; // Reset velocity to prevent sliding
        yield return new WaitForSeconds(0.5f); // Adjust the duration of the knockback effect
        PlayerManager.Instance.playerValues.IsKnockbacked = false; // Reset the knockback state after the effect
    }

    public IEnumerator SwordAttack()
    {
        PlayerManager.Instance.playerValues.IsSwordAttacking = true;

        Vector2 position = PlayerManager.Instance.playerValues.IsfacingRight ? new Vector2(PlayerManager.Instance.playerValues.playerCollider.bounds.max.x + 1f, PlayerManager.Instance.playerValues.playerCollider.bounds.center.y) : new Vector2(PlayerManager.Instance.playerValues.playerCollider.bounds.min.x - 1f, PlayerManager.Instance.playerValues.playerCollider.bounds.center.y);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(position, PlayerManager.Instance.playerValues.hitboxRadius, PlayerManager.Instance.playerValues.enemyLayerMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            GameObject enemyGO = enemy.gameObject;
            if (!damagedEnemies2.Contains(enemyGO))
            {
                EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();
                if (enemyScript != null)
                {
                    StartCoroutine(enemyScript.TakeEnemyDamage(PlayerManager.Instance.playerValues.playerDamage));
                    Debug.Log("Enemy took damage from player!");
                    damagedEnemies2.Add(enemyGO); // tekrar vurulmasýný engelle
                }
            }
        }

        yield return new WaitForSeconds(PlayerManager.Instance.playerValues.attackDelay); // Bekleme süresi
        damagedEnemies2.Clear(); // Attack bittiðinde hasar verilen düþmanlarý temizle

        PlayerManager.Instance.playerValues.IsSwordAttacking = false; // Saldýrý bittiðinde durumu sýfýrla
    }

}
