using UnityEngine;

public class EnemyValues : MonoBehaviour
{
    public float enemySpeed = 5f;
    public float waitPatrolTime = 0f;
    public float lastLocationWaitTime = 0f;
    public float damage = 20f;
    public float currentAttackWaitTime = 0f;
    public float attackWaitTime = 1f;

    public Transform APoint;
    public Transform BPoint;
    public Transform currentTarget;
    public Transform player;
    public Transform enemyHitbox;

    public Rigidbody2D enemyRb;
    public SpriteRenderer enemySpriteRenderer;
    public Animator enemyAnimator;


    public bool IsFacingRight = false;
    public bool IsPlayerInRange = false;
    public bool playerNotFound = true;
    public bool lastLocationOfThePlayer = false;
    public bool wasInRangeLastFrame = false;
    public bool IsAttacking = false;
    public bool attackWaiting = false;
    public bool IsEnemyKnockbacked = false;
    public bool attackInitialized = false;
    public bool IsInAttackAnimation = false;
}
