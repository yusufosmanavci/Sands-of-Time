using Assets.Scripts.BossScripts;
using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossValues bossValues;
    public PlayerHealth playerHealth;

    private void Awake()
    {
        bossValues = GetComponent<BossValues>();
        bossValues.bossRb=GetComponent<Rigidbody2D>();
        bossValues.bossAnimator = GetComponentInChildren<Animator>();
        bossValues.bossSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bossValues.player = FindFirstObjectByType<PlayerValues>().transform; // Oyuncunun transformunu al
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void Update()
    {
        float rawSpeed = bossValues.bossRb.linearVelocity.magnitude;

        // Çok küçük deðerleri sýfýr kabul et (gürültüyü engelle)
        float speed = rawSpeed < 0.05f ? 0f : rawSpeed;

        bossValues.bossAnimator.SetFloat("Speed", speed);

        PlayerDistanceControl();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtbox"))
        {
            // Sadece düþman saldýrýyorsa ve oyuncu saldýrmýyorsa
            PlayerController playerController = collision.gameObject.GetComponentInParent<PlayerController>();
            if (bossValues.IsAttacking && playerController != null && !playerController.playerValues.IsAttacking)
            {
                PlayerHealth player = playerController.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakePLayerDamage(bossValues.bossDamage, transform.position);
                    Debug.Log("Player took damage from enemy!");
                }
            }
        }
    }
    private void PlayerTrackker()
    {
        Vector2 bossPos = transform.position;
        Vector2 playerPos = bossValues.player.position;

        if (playerPos.x > bossPos.x)
        {
            bossValues.IsFacingRight = true;
            bossValues.bossSpriteRenderer.flipX = false;
        }
        else
        {
            bossValues.IsFacingRight = false;
            bossValues.bossSpriteRenderer.flipX = true;
        }

        // Saldýrý animasyonu ve bekleme AttackRoutine'de yönetiliyor
        if (!bossValues.IsAttacking)
        {
            // Düþmaný hareket ettir
            float direction = bossValues.IsFacingRight ? 1f : -1f;
            bossValues.bossRb.linearVelocity = new Vector2(direction * bossValues.bossSpeed, bossValues.bossRb.linearVelocity.y);
        }
    }

    private bool PlayerDistanceControl()
    {
        if(bossValues.player == null)
        {
            return true;
        }
        float yOffset = Mathf.Abs(transform.position.y - bossValues.player.transform.position.y);
        float xDistanceToPlayer = Mathf.Abs(transform.position.x - bossValues.player.transform.position.x);

        if (xDistanceToPlayer > 20 || yOffset > 2.5f)
        {
            PlayerTrackker();
            bossValues.IsAttacking = false;
            bossValues.attackInitialized = false;
        }
        else if (xDistanceToPlayer <= 10f && xDistanceToPlayer > 4f && yOffset <= 2.5f)
        {
            PlayerTrackker();
            bossValues.IsAttacking = false;
            bossValues.attackInitialized = false;
        }
        else if (xDistanceToPlayer <= 4f && yOffset <= 2.5f)
        {
            if (!bossValues.attackInitialized)
            {
                StartCoroutine(AttackRoutine());
            }
        }

        return false;
    }

    private IEnumerator AttackRoutine()
    {
        bossValues.attackInitialized = true;
        bossValues.IsAttacking = true;

        // Saldýrýdan önce bekleme süresi (hazýrlýk süresi)
        bossValues.bossRb.linearVelocity = Vector2.zero; // Saldýrý sýrasýnda hareketi durdur
        yield return new WaitForSeconds(bossValues.attackWaitTime); // örn. 1 saniye hazýrlýk

        // Animasyon baþlat
        bossValues.bossAnimator.SetBool("IsAttacking", true);
        bossValues.bossHitbox.gameObject.SetActive(true); // Saldýrý hitbox'ýný aktif et
        bossValues.IsInAttackAnimation = true;

        if (bossValues.IsFacingRight)
        {
            bossValues.bossHitbox.localScale = new Vector3(1f, 1f, 1f); // Sað tarafa saldýrýrken
        }
        else
        {
            bossValues.bossHitbox.localScale = new Vector3(-1f, 1f, 1f); // Sol tarafa saldýrýrken
        }

        // Saldýrý animasyon süresi boyunca bekle
        yield return new WaitForSeconds(0.5f); // animasyon süresi kadar

        // Animasyon bitir
        bossValues.bossAnimator.SetBool("IsAttacking", false);
        bossValues.bossHitbox.gameObject.SetActive(false); // Saldýrý hitbox'ýný pasif et
        bossValues.IsInAttackAnimation = false;

        // Saldýrýdan sonra tekrar bekleme (cooldown)
        yield return new WaitForSeconds(bossValues.attackWaitTime);

        // Saldýrý tamamlandý
        bossValues.IsAttacking = false;
        bossValues.attackInitialized = false;
    }
}
