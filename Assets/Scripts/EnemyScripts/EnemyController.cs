using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyValues enemyValues;

    private void Awake()
    {
        enemyValues = GetComponent<EnemyValues>();
        enemyValues.currentTarget = enemyValues.APoint; // Ba�lang�� hedefi A noktas� olarak ayarla
    }

    private void Update()
    {
        PlayerDistanceControl();

        if (enemyValues.IsPlayerInRange)
        {
            PlayerTrackker();
        }
        else
        {
            if (enemyValues.playerNotFound)
            {
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


    private void PlayerDistanceControl()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, enemyValues.player.transform.position);
        Debug.Log($"Distance to player: {distanceToPlayer}");
        if (distanceToPlayer > 10)
        {
            enemyValues.IsPlayerInRange = false;
            enemyValues.playerNotFound = true; // Oyuncu bulunamad�
            Debug.Log("Player is too far away!");
        }
        else if (distanceToPlayer <= 10f && distanceToPlayer > 2)
        {
            enemyValues.IsPlayerInRange = true;
        }
        else if (distanceToPlayer <= 2f)
        {
            Debug.Log("Player is too close!");
        }
    }

    private void PlayerTrackker()
    {
        float direction = enemyValues.IsFacingRight ? 1f : -1f;
        bool IsPLayerInTheLeft = (transform.position.x < enemyValues.player.transform.position.x);
        enemyValues.enemyRb.linearVelocity = new Vector2(direction * enemyValues.enemySpeed, enemyValues.enemyRb.linearVelocity.y);

    }

    /*private void PlayerDetector()
    {
        Vector2 direction = enemyValues.IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new(transform.position.x, transform.position.y + 0.5f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, 10f, enemyValues.playerLayer);
        Debug.DrawRay(origin, direction * 10f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                enemyValues.IsPlayerInRange = true;
            }
            else
                enemyValues.IsPlayerInRange = false;
        }
        else
        {
            enemyValues.IsPlayerInRange = false;
        }
    }*/

    private void PlayerNotFound()
    {
        if (enemyValues.playerNotFound)
        {
            if (Vector2.Distance(transform.position, enemyValues.currentTarget.position) < 0.2f)
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
            transform.position = Vector2.MoveTowards(transform.position, enemyValues.currentTarget.position, enemyValues.enemySpeed * Time.deltaTime);

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


}
