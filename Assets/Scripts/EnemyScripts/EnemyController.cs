using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyValues enemyValues;
    Vector2 lastPosition;
    public PlayerHealth playerHealth; // Oyuncunun sa�l�k bile�eni
    
    private void Awake()
    {
        enemyValues = GetComponent<EnemyValues>();
        enemyValues.currentTarget = enemyValues.APoint; // Ba�lang�� hedefi A noktas� olarak ayarla
        enemyValues.enemyAnimator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
        enemyValues.player = FindFirstObjectByType<PlayerController>().transform; // Oyuncu referans�n� al
    }

    private void Update()
    {
        float rawSpeed = enemyValues.enemyRb.linearVelocity.magnitude;

        // �ok k���k de�erleri s�f�r kabul et (g�r�lt�y� engelle)
        float speed = rawSpeed < 0.05f ? 0f : rawSpeed;

        enemyValues.enemyAnimator.SetFloat("Speed", speed);

        PlayerDistanceControl();

        if (!enemyValues.IsPlayerInRange && enemyValues.wasInRangeLastFrame)
        {
            enemyValues.lastLocationOfThePlayer = true;
            enemyValues.lastLocationWaitTime = 2f;
            Debug.Log("Player disappeared! Waiting at last seen location.");
        }

        enemyValues.wasInRangeLastFrame = enemyValues.IsPlayerInRange;

        if (enemyValues.IsPlayerInRange)
        {
            PlayerTrackker();
        }
        else
        {
            if (enemyValues.playerNotFound)
            {
                if (!LastLocationWait())
                    PlayerNotFound();
            }
            else
            {
                EnemyPatrol();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtbox"))
        {
            // Sadece d��man sald�r�yorsa ve oyuncu sald�rm�yorsa
            PlayerController playerController = collision.gameObject.GetComponentInParent<PlayerController>();
            if (enemyValues.IsAttacking && playerController != null && !playerController.playerValues.IsAttacking)
            {
                PlayerHealth player = playerController.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakePLayerDamage(enemyValues.damage, transform.position);
                    Debug.Log("Player took damage from enemy!");
                }
            }
        }
    }


    private void EnemyPatrol()
    {
        // Noktaya ula��ld�ysa bekle
        if (Vector2.Distance(transform.position, enemyValues.currentTarget.position) < 0.2f)
        {
            // Bekleme s�resi ba�lat�lmam��sa ba�lat
            if (enemyValues.waitPatrolTime <= 0f)
            {
                enemyValues.waitPatrolTime = 2f;
            }

            // Bekleme s�resini azalt
            enemyValues.waitPatrolTime -= Time.deltaTime;

            // Bekleme s�resi bitince hedefi de�i�tir
            if (enemyValues.waitPatrolTime <= 0f)
            {
                enemyValues.currentTarget = enemyValues.currentTarget == enemyValues.APoint ? enemyValues.BPoint : enemyValues.APoint;

                // Y�n� g�ncelle
                if (transform.position.x < enemyValues.currentTarget.position.x)
                {
                    enemyValues.IsFacingRight = true;
                    enemyValues.enemySpriteRenderer.flipX = false;
                }
                else
                {
                    enemyValues.IsFacingRight = false;
                    enemyValues.enemySpriteRenderer.flipX = true;
                }
            }

            return; // Beklerken hareket etme
        }

        // Hareket et
        transform.position = Vector2.MoveTowards(transform.position, enemyValues.currentTarget.position, enemyValues.enemySpeed * Time.deltaTime);



        // Y�n� g�ncelle (hareket s�ras�nda)
        if (transform.position.x < enemyValues.currentTarget.position.x)
        {
            enemyValues.IsFacingRight = true;
            enemyValues.enemySpriteRenderer.flipX = false;
        }
        else
        {
            enemyValues.IsFacingRight = false;
            enemyValues.enemySpriteRenderer.flipX = true;
        }
    }


    private bool PlayerDistanceControl()
    {
        if (enemyValues.player == null)
        {
            enemyValues.IsPlayerInRange = false;
            enemyValues.playerNotFound = true;
            return false; // Player bulunamad�
        }
        float yOffset = Mathf.Abs(transform.position.y - enemyValues.player.transform.position.y);
        float xDistanceToPlayer = Mathf.Abs(transform.position.x - enemyValues.player.transform.position.x);

        if (xDistanceToPlayer > 10 || yOffset > 2.5f)
        {
            enemyValues.IsPlayerInRange = false;
            enemyValues.playerNotFound = true;
            enemyValues.IsAttacking = false;
        }
        else if (xDistanceToPlayer <= 10f && xDistanceToPlayer > 2 && yOffset <= 2.5f)
        {
            enemyValues.IsPlayerInRange = true;
            enemyValues.playerNotFound = false;
            return true;
        }
        else if (xDistanceToPlayer <= 1.5f && yOffset <= 2.5f)
        {
            enemyValues.IsPlayerInRange = true;
            enemyValues.playerNotFound = false;

            if (!enemyValues.attackInitialized)
            {
                StartCoroutine(AttackRoutine());
            }
        }

        return false;
    }



    private void PlayerTrackker()
    {
        if (enemyValues.IsEnemyKnockbacked)
            return;

        Vector2 enemyPos = transform.position;
        Vector2 playerPos = enemyValues.player.position;

        if (playerPos.x > enemyPos.x)
        {
            enemyValues.IsFacingRight = true;
            enemyValues.enemySpriteRenderer.flipX = false;
        }
        else
        {
            enemyValues.IsFacingRight = false;
            enemyValues.enemySpriteRenderer.flipX = true;
        }

        // Sald�r� animasyonu ve bekleme AttackRoutine'de y�netiliyor
        if (!enemyValues.IsAttacking)
        {
            if (IsOnSamePlatformLevel() && IsGroundAhead())
            {
                // Yerdeyse ve platform seviyesindeyse, d��man� hareket ettir
                float direction = enemyValues.IsFacingRight ? 1f : -1f;
                enemyValues.enemyRb.linearVelocity = new Vector2(direction * enemyValues.enemySpeed, enemyValues.enemyRb.linearVelocity.y);
            }
            else
            {
                // Yerde de�ilse veya platform seviyesinde de�ilse, d��man� durdur
                enemyValues.enemyRb.linearVelocity = new Vector2(0f, enemyValues.enemyRb.linearVelocity.y);
            }
        }
    }


    private void PlayerNotFound()
    {
        if (enemyValues.playerNotFound)
        {
            if (Vector2.Distance(transform.position, enemyValues.currentTarget.position) < 1f)
            {
                if (enemyValues.waitPatrolTime <= 0f)
                    enemyValues.waitPatrolTime = 2f;

                enemyValues.waitPatrolTime -= Time.deltaTime;

                if (enemyValues.waitPatrolTime <= 0f)
                {
                    enemyValues.currentTarget = enemyValues.currentTarget == enemyValues.APoint ? enemyValues.BPoint : enemyValues.APoint;

                    if (transform.position.x < enemyValues.currentTarget.position.x)
                    {
                        enemyValues.IsFacingRight = true;
                        enemyValues.enemySpriteRenderer.flipX = false;
                    }
                    else
                    {
                        enemyValues.IsFacingRight = false;
                        enemyValues.enemySpriteRenderer.flipX = true;
                    }
                }

                return;
            }

            // Hareket et
            Vector2 direction = (enemyValues.currentTarget.position - transform.position).normalized;
            enemyValues.enemyRb.linearVelocity = new Vector2(direction.x * enemyValues.enemySpeed, enemyValues.enemyRb.linearVelocity.y);



            // Y�n� g�ncelle
            if (transform.position.x < enemyValues.currentTarget.position.x)
            {
                enemyValues.IsFacingRight = true;
                enemyValues.enemySpriteRenderer.flipX = false;
            }
            else
            {
                enemyValues.IsFacingRight = false;
                enemyValues.enemySpriteRenderer.flipX = true;
            }
        }
    }

    private bool LastLocationWait()
    {
        if (enemyValues.lastLocationOfThePlayer)
        {
            if (enemyValues.lastLocationWaitTime > 0f)
            {
                enemyValues.lastLocationWaitTime -= Time.deltaTime;
                return true; // beklemeye devam et
            }

            // Bekleme s�resi bitti, art�k player yok ve patrol�a ge�ilebilir
            enemyValues.lastLocationOfThePlayer = false;
            enemyValues.playerNotFound = false; // Art�k "player not found" de�il

            // Yeni patrol hedefini ayarla
            enemyValues.currentTarget = enemyValues.currentTarget == enemyValues.APoint ? enemyValues.BPoint : enemyValues.APoint;

            // Y�n� g�ncelle
            if (transform.position.x < enemyValues.currentTarget.position.x)
            {
                enemyValues.IsFacingRight = true;
                enemyValues.enemySpriteRenderer.flipX = false;
            }
            else
            {
                enemyValues.IsFacingRight = false;
                enemyValues.enemySpriteRenderer.flipX = true;
            }

            return false; // bekleme bitti, devam et
        }

        return false; // Zaten last location yoktu
    }

    public IEnumerator EnemyKnockbackRoutine(Vector2 force)
    {
        enemyValues.IsEnemyKnockbacked = true;
        enemyValues.enemyRb.AddForce(force, ForceMode2D.Impulse);
        enemyValues.enemyRb.linearVelocity = Vector2.zero; // Reset velocity to prevent sliding
        yield return new WaitForSeconds(0.5f); // Adjust the duration of the knockback effect
        enemyValues.IsEnemyKnockbacked = false; // Reset the knockback state after the effect
    }

    private IEnumerator AttackRoutine()
    {
        enemyValues.attackInitialized = true;
        enemyValues.IsAttacking = true;

        // Sald�r�dan �nce bekleme s�resi (haz�rl�k s�resi)
        enemyValues.enemyRb.linearVelocity = Vector2.zero; // Sald�r� s�ras�nda hareketi durdur
        yield return new WaitForSeconds(enemyValues.attackWaitTime); // �rn. 1 saniye haz�rl�k

        // Animasyon ba�lat
        enemyValues.enemyAnimator.SetBool("IsAttacking", true);
        enemyValues.enemyHitbox.gameObject.SetActive(true); // Sald�r� hitbox'�n� aktif et
        enemyValues.IsInAttackAnimation = true;

        if (enemyValues.IsFacingRight)
        {
            enemyValues.enemyHitbox.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Sa� tarafa sald�r�rken
        }
        else
        {
            enemyValues.enemyHitbox.localScale = new Vector3(-0.5f, 0.5f, 0.5f); // Sol tarafa sald�r�rken
        }

        // Sald�r� animasyon s�resi boyunca bekle
        yield return new WaitForSeconds(0.5f); // animasyon s�resi kadar

        // Animasyon bitir
        enemyValues.enemyAnimator.SetBool("IsAttacking", false);
        enemyValues.enemyHitbox.gameObject.SetActive(false); // Sald�r� hitbox'�n� pasif et
        enemyValues.IsInAttackAnimation = false;

        // Sald�r�dan sonra tekrar bekleme (cooldown)
        yield return new WaitForSeconds(enemyValues.attackWaitTime);

        // Sald�r� tamamland�
        enemyValues.IsAttacking = false;
        enemyValues.attackInitialized = false;
    }

    private bool IsGroundAhead()
    {
        return Physics2D.OverlapCircle(enemyValues.groundCheck.position, enemyValues.groundCheckRadius, enemyValues.enemyLayer);
    }

    private bool IsOnSamePlatformLevel()
    {
        return Mathf.Abs(transform.position.y - enemyValues.player.position.y) <= enemyValues.platformTolerance;
    }

}
