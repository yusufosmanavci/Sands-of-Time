using UnityEngine;
using System.Collections;

namespace Assets.Scripts.BossScripts
{
	public class BossValues : MonoBehaviour
	{
        public float bossSpeed = 2f; // Speed of the boss
		public float attackWaitTime = 1f; // Time to wait before attacking
		public float bossDamage = 50f; // Damage dealt by the boss

        public Transform player; // Player's transform
		public SpriteRenderer bossSpriteRenderer; // Boss's sprite renderer
		public Rigidbody2D bossRb; // Boss's Rigidbody2D
		public Transform bossHitbox; // Boss's hitbox
		public Animator bossAnimator; // Boss's animator

        public bool IsFacingRight = true; // Is the boss facing right?
		public bool IsAttacking = false; // Is the boss currently attacking?
		public bool attackInitialized = false; // Has the attack been initialized?
		public bool IsInAttackAnimation = false; // Is the boss in the attack animation?
    }
}