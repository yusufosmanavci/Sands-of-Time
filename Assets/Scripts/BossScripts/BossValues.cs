using UnityEngine;
using System.Collections;

namespace Assets.Scripts.BossScripts
{
    public class BossValues : MonoBehaviour
    {
        public float bossSpeed = 2f; // Speed of the boss
        public float attackWaitTime = 1f; // Time to wait before attacking
        public float hitboxRadius = 3f;
        public float bossDamage = 50f; // Damage dealt by the boss
        public float bossSpellDamage = 40f;

        public Transform player; // Player's transform
        public SpriteRenderer bossSpriteRenderer; // Boss's sprite renderer
        public Rigidbody2D bossRb; // Boss's Rigidbody2D
        public Collider2D bossCollider; // Boss's hitbox
        public Animator bossAnimator; // Boss's animator
        public LayerMask playerLayerMask;
        public GameObject spellObject;

        public bool IsFacingRight = false; // Is the boss facing right?
        public bool IsAttacking = false; // Is the boss currently attacking?
        public bool attackInitialized = false; // Has the attack been initialized?
        public bool IsInAttackAnimation = false; // Is the boss in the attack animation?
        public bool IsCasting = false;
        public bool HasTriedCasting = false;
        public bool IsDead = false;

        private void OnEnable()
        {
            ResetBossValues();
        }

        public void ResetBossValues()
        {
            IsFacingRight = false; // Is the boss facing right?
            IsAttacking = false; // Is the boss currently attacking?
            attackInitialized = false; // Has the attack been initialized?
            IsInAttackAnimation = false; // Is the boss in the attack animation?
            IsCasting = false;
            HasTriedCasting = false;
        }
    }
}