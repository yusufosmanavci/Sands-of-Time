using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyValues enemyValues;

    private void Awake()
    {
        enemyValues = GetComponent<EnemyValues>();
    }

    private void Update()
    {
        if (!enemyValues.IsPlayerInRange)
        {
            EnemyPatrol();
            PlayerDetector();
        }
        else
        {
            PlayerDetector();
            PlayerTrackker();
        }
    }

    private void EnemyPatrol()
    {
        bool AtoB = enemyValues.APoint.position.x < enemyValues.BPoint.position.x;
        transform.position = Vector2.MoveTowards(transform.position, enemyValues.BPoint.transform.position, enemyValues.enemySpeed * Time.deltaTime);
        if (AtoB)
        {
            enemyValues.IsFacingRight = true;
            enemyValues.enemySpriteRenderer.flipX = false;
        }
        else
        {
            enemyValues.IsFacingRight = false;
            enemyValues.enemySpriteRenderer.flipX = true;
        }
        if (Vector2.Distance(transform.position, enemyValues.BPoint.transform.position) < 0.1f)
        {
            (enemyValues.BPoint, enemyValues.APoint) = (enemyValues.APoint, enemyValues.BPoint);
            transform.position = Vector2.MoveTowards(transform.position, enemyValues.BPoint.transform.position, enemyValues.enemySpeed * Time.deltaTime);
        }
        if (Vector2.Distance(transform.position, enemyValues.APoint.transform.position) < 0.1f)
        {
            (enemyValues.APoint, enemyValues.BPoint) = (enemyValues.BPoint, enemyValues.APoint);
            transform.position = Vector2.MoveTowards(transform.position, enemyValues.BPoint.transform.position, enemyValues.enemySpeed * Time.deltaTime);
        }
    }

    private void PlayerTrackker()
    {
        Vector2 direction = enemyValues.IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 origin = new(transform.position.x, transform.position.y + 0.5f);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, 10f, enemyValues.playerLayer);
        Debug.DrawRay(origin, direction * 10f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                transform.position = Vector2.MoveTowards(transform.position, enemyValues.player.transform.position, enemyValues.enemySpeed * Time.deltaTime);
            }
        }
    }

    private void PlayerDetector()
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
            enemyValues.IsPlayerInRange = false;
    }
}
