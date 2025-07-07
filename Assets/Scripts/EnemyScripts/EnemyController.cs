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
        EnemyPatrol();
    }

    private void EnemyPatrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, enemyValues.BPoint.transform.position, enemyValues.enemySpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, enemyValues.BPoint.transform.position) < 0.1f)
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

}
