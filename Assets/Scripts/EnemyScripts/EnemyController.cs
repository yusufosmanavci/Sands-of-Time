using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyValues enemyValues;
    Vector2 lastPosition;
    public PlayerHealth playerHealth; // Oyuncunun saðlýk bileþeni
    private HashSet<GameObject> damagedPlayer = new HashSet<GameObject>(); // Hasar verilen oyuncu listesi

    private void Awake()
    {
        enemyValues = GetComponent<EnemyValues>();
        enemyValues.currentTarget = enemyValues.APoint; // Baþlangýç hedefi A noktasý olarak ayarla
        enemyValues.enemyAnimator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
        enemyValues.player = FindFirstObjectByType<PlayerController>().transform; // Oyuncu referansýný al
    }

    private void Update()
    {
        float rawSpeed = enemyValues.enemyRb.linearVelocity.magnitude;

        // Çok küçük deðerleri sýfýr kabul et (gürültüyü engelle)
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


    private void EnemyPatrol()
    {
        // Noktaya ulaþýldýysa bekle
        if (Vector2.Distance(transform.position, enemyValues.currentTarget.position) < 0.2f)
        {
            // Bekleme süresi baþlatýlmamýþsa baþlat
            if (enemyValues.waitPatrolTime <= 0f)
            {
                enemyValues.waitPatrolTime = 2f;
            }

            // Bekleme süresini azalt
            enemyValues.waitPatrolTime -= Time.deltaTime;

            // Bekleme süresi bitince hedefi deðiþtir
            if (enemyValues.waitPatrolTime <= 0f)
            {
                enemyValues.currentTarget = enemyValues.currentTarget == enemyValues.APoint ? enemyValues.BPoint : enemyValues.APoint;

                // Yönü güncelle
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



        // Yönü güncelle (hareket sýrasýnda)
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
            return false; // Player bulunamadý
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

        // Saldýrý animasyonu ve bekleme AttackRoutine'de yönetiliyor
        if (!enemyValues.IsAttacking)
        {
            if (IsOnSamePlatformLevel() && IsGroundAhead())
            {
                // Yerdeyse ve platform seviyesindeyse, düþmaný hareket ettir
                float direction = enemyValues.IsFacingRight ? 1f : -1f;
                enemyValues.enemyRb.linearVelocity = new Vector2(direction * enemyValues.enemySpeed, enemyValues.enemyRb.linearVelocity.y);
            }
            else
            {
                // Yerde deðilse veya platform seviyesinde deðilse, düþmaný durdur
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



            // Yönü güncelle
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

            // Bekleme süresi bitti, artýk player yok ve patrol’a geçilebilir
            enemyValues.lastLocationOfThePlayer = false;
            enemyValues.playerNotFound = false; // Artýk "player not found" deðil

            // Yeni patrol hedefini ayarla
            enemyValues.currentTarget = enemyValues.currentTarget == enemyValues.APoint ? enemyValues.BPoint : enemyValues.APoint;

            // Yönü güncelle
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


    private IEnumerator AttackRoutine()
    {
        enemyValues.attackInitialized = true;
        enemyValues.IsAttacking = true;

        // Saldýrýdan önce bekleme süresi (hazýrlýk süresi)
        enemyValues.enemyRb.linearVelocity = Vector2.zero; // Saldýrý sýrasýnda hareketi durdur
        yield return new WaitForSeconds(enemyValues.attackWaitTime); // örn. 1 saniye hazýrlýk

        // Animasyon baþlat
        enemyValues.enemyAnimator.SetBool("IsAttacking", true);
        enemyValues.enemyHitbox.gameObject.SetActive(true); // Saldýrý hitbox'ýný aktif et
        enemyValues.IsInAttackAnimation = true;

        StartCoroutine(SwordAttack()); // Saldýrý animasyonu sýrasýnda oyuncuya hasar verme

        // Saldýrý animasyon süresi boyunca bekle
        yield return new WaitForSeconds(0.5f); // animasyon süresi kadar

        // Animasyon bitir
        enemyValues.enemyAnimator.SetBool("IsAttacking", false);
        enemyValues.enemyHitbox.gameObject.SetActive(false); // Saldýrý hitbox'ýný pasif et
        enemyValues.IsInAttackAnimation = false;

        // Saldýrýdan sonra tekrar bekleme (cooldown)
        yield return new WaitForSeconds(enemyValues.attackWaitTime);

        // Saldýrý tamamlandý
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

    public IEnumerator SwordAttack()
    {
        Vector2 position = enemyValues.IsFacingRight ? new Vector2(enemyValues.enemyCollider.bounds.max.x + 1f, enemyValues.enemyCollider.bounds.center.y) : new Vector2(enemyValues.enemyCollider.bounds.min.x - 1f, enemyValues.enemyCollider.bounds.center.y);
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(position, PlayerManager.Instance.playerValues.hitboxRadius, enemyValues.playerLayerMask);
        foreach (Collider2D player in hitPlayer)
        {
            GameObject playerGO = player.gameObject;
            if (!damagedPlayer.Contains(playerGO))
            {
                PlayerHealth playerScript = player.GetComponent<PlayerHealth>();
                if (playerScript != null)
                {
                    playerScript.TakePLayerDamage(enemyValues.damage, transform.position);
                    Debug.Log("Enemy took damage from player!");
                    damagedPlayer.Add(playerGO); // tekrar vurulmasýný engelle
                }
            }
        }

        yield return new WaitForSeconds(PlayerManager.Instance.playerValues.attackDelay); // Bekleme süresi
        damagedPlayer.Clear(); // Attack bittiðinde hasar verilen düþmanlarý temizle
    }
}
