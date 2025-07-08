using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyValues enemyValues;
    Vector2 lastPosition;

    private void Awake()
    {
        enemyValues = GetComponent<EnemyValues>();
        enemyValues.currentTarget = enemyValues.APoint; // Ba�lang�� hedefi A noktas� olarak ayarla
        enemyValues.enemyAnimator = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
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
        float yOffset = Mathf.Abs(transform.position.y - enemyValues.player.transform.position.y); // Y�kseklik fark� i�in yOffset hesapla
        float xDistanceToPlayer = Mathf.Abs(transform.position.x - enemyValues.player.transform.position.x);
        if (xDistanceToPlayer > 10 || yOffset > 3f)
        {
            enemyValues.IsPlayerInRange = false;
            enemyValues.playerNotFound = true; // Oyuncu bulunamad�
        }
        else if (xDistanceToPlayer <= 10f && xDistanceToPlayer > 2 && yOffset <= 3f)
        {
            enemyValues.IsPlayerInRange = true;
            enemyValues.playerNotFound = false; // Oyuncu bulundu
            return true;
        }
        else if (xDistanceToPlayer <= 2f && yOffset <= 3f)
        {
            Debug.Log("Player is too close!");
        }
        return false;
    }

    private void PlayerTrackker()
    {
        Vector2 enemyPos = transform.position;
        Vector2 playerPos = enemyValues.player.position;

        // Y�n g�ncelle
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

        // Y�n� kullanarak hareket et
        float direction = enemyValues.IsFacingRight ? 1f : -1f;
        enemyValues.enemyRb.linearVelocity = new Vector2(direction * enemyValues.enemySpeed, enemyValues.enemyRb.linearVelocity.y);


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
}
