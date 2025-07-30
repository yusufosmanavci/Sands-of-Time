using Assets.Scripts.BossScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossValues bossValues;
    public PlayerHealth playerHealth;
    public BossSpellCasting spellCasting;
    private HashSet<GameObject> damagedPlayer = new HashSet<GameObject>(); // Hasar verilen oyuncu listesi
    public int spellChance;


    private void Awake()
    {
        bossValues = GetComponent<BossValues>();
        bossValues.bossRb = GetComponent<Rigidbody2D>();
        bossValues.bossAnimator = GetComponentInChildren<Animator>();
        bossValues.bossSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bossValues.player = FindFirstObjectByType<PlayerValues>().transform; // Oyuncunun transformunu al
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        spellCasting = GetComponentInChildren<BossSpellCasting>();
    }

    private void Update()
    {
        float rawSpeed = bossValues.bossRb.linearVelocity.magnitude;

        // Çok küçük deðerleri sýfýr kabul et (gürültüyü engelle)
        float speed = rawSpeed < 0.05f ? 0f : rawSpeed;
        bossValues.bossAnimator.SetFloat("Speed", speed);
        PlayerDistanceControl();
    }

    private void PlayerTrackker()
    {
        Vector2 bossPos = transform.position;
        Vector2 playerPos = bossValues.player.position;

        if (playerPos.x > bossPos.x)
        {
            bossValues.IsFacingRight = true;
            bossValues.bossSpriteRenderer.flipX = true;
        }
        else
        {
            bossValues.IsFacingRight = false;
            bossValues.bossSpriteRenderer.flipX = false;
        }

        // Saldýrý animasyonu ve bekleme AttackRoutine'de yönetiliyor
        if (!bossValues.IsAttacking)
        {
            // Düþmaný hareket ettir
            float direction = bossValues.IsFacingRight ? 1f : -1f;
            bossValues.bossRb.linearVelocity = new Vector2(direction * bossValues.bossSpeed, bossValues.bossRb.linearVelocity.y);
        }
    }

    private void PlayerDistanceControl()
    {
        if (bossValues.player == null)
        {
            return;
        }
        float yOffset = Mathf.Abs(transform.position.y - bossValues.player.transform.position.y);
        float xDistanceToPlayer = Mathf.Abs(transform.position.x - bossValues.player.transform.position.x);

        if (xDistanceToPlayer > 10f || yOffset > 2.5f)
        {
            PlayerTrackker();
        }
        else if (xDistanceToPlayer <= 10f && xDistanceToPlayer > 4f && yOffset <= 2.5f)
        {
            if (!bossValues.HasTriedCasting && !bossValues.IsCasting)
            {
                spellChance = Random.Range(0, 100);
                bossValues.HasTriedCasting = true;

                if (spellChance < 45)
                {
                    StartCoroutine(BossCasting());
                }
                else
                {
                    PlayerTrackker();

                    StartCoroutine(ResetCastingAttemp());
                }
            }
            else if (!bossValues.IsCasting)
            {
                PlayerTrackker();
            }
        }
        else if (xDistanceToPlayer <= 4f && yOffset <= 2.5f)
        {
            Vector2 bossPos = transform.position;
            Vector2 playerPos = bossValues.player.position;
            if (bossValues.IsCasting || bossValues.IsDead)
            {
                return;
            }
            else if (!bossValues.attackInitialized)
            {
                StartCoroutine(AttackRoutine());

                if (playerPos.x > bossPos.x)
                {
                    bossValues.IsFacingRight = true;
                    bossValues.bossSpriteRenderer.flipX = true;
                }
                else
                {
                    bossValues.IsFacingRight = false;
                    bossValues.bossSpriteRenderer.flipX = false;
                }
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        bossValues.attackInitialized = true;
        bossValues.IsAttacking = true;

        // Saldýrýdan önce bekleme süresi (hazýrlýk süresi)
        bossValues.bossRb.linearVelocity = Vector2.zero; // Saldýrý sýrasýnda hareketi durdur
        yield return new WaitForSeconds(1f); // örn. 1 saniye hazýrlýk

        // Animasyon baþlat
        bossValues.bossAnimator.SetBool("IsAttacking", true);
        bossValues.IsInAttackAnimation = true;

        yield return new WaitForSeconds(0.5f);
        SwordAttack(); // Saldýrý animasyonu sýrasýnda oyuncuya hasar verme

        // Saldýrý animasyon süresi boyunca bekle
        yield return new WaitForSeconds(0.5f); // animasyon süresi kadar
        damagedPlayer.Clear();

        // Animasyon bitir
        bossValues.bossAnimator.SetBool("IsAttacking", false);
        bossValues.IsInAttackAnimation = false;

        // Saldýrýdan sonra tekrar bekleme (cooldown)
        yield return new WaitForSeconds(bossValues.attackWaitTime - 0.6f);

        // Saldýrý tamamlandý
        bossValues.IsAttacking = false;
        bossValues.attackInitialized = false;
    }
    public void SwordAttack()
    {
        Vector2 position = bossValues.IsFacingRight ? new Vector2(bossValues.bossCollider.bounds.max.x + 1.5f, bossValues.bossCollider.bounds.center.y) : new Vector2(bossValues.bossCollider.bounds.min.x - 1.5f, bossValues.bossCollider.bounds.center.y);
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(position, bossValues.hitboxRadius, bossValues.playerLayerMask);
        foreach (Collider2D player in hitPlayer)
        {
            GameObject playerGO = player.gameObject;
            if (!damagedPlayer.Contains(playerGO))
            {
                PlayerHealth playerScript = player.GetComponent<PlayerHealth>();
                if (playerScript != null)
                {
                    playerScript.TakePLayerDamage(bossValues.bossDamage, transform.position);
                    Debug.Log("Enemy took damage from player!");
                    damagedPlayer.Add(playerGO); // tekrar vurulmasýný engelle
                }
            }
        }
    }


    public IEnumerator BossCasting()
    {
        bossValues.IsCasting = true;
        bossValues.bossAnimator.SetBool("IsCasting", true);
        yield return new WaitForSeconds(1f);
        bossValues.bossAnimator.SetBool("IsCasting", false);
        bossValues.IsCasting = false;
        if (spellCasting.spellCount == 0)
        {
            spellCasting.spellCount++;
            spellCasting.Shoot();
        }

        yield return new WaitForSeconds(1.5f);
        bossValues.HasTriedCasting = false;
    }

    public IEnumerator ResetCastingAttemp()
    {
        yield return new WaitForSeconds(1.5f);
        bossValues.HasTriedCasting = false;
    }
}
