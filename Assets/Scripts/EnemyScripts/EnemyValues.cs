using UnityEngine;

public class EnemyValues : MonoBehaviour
{
    public float enemySpeed = 5f;
    public Transform APoint;
    public Transform BPoint;

    public Rigidbody2D enemyRb;
    public GameObject player;
    public LayerMask playerLayer;
    public SpriteRenderer enemySpriteRenderer;

    public bool IsFacingRight = false;
    public bool IsPlayerInRange = false;
}
