using System.Collections;
using UnityEngine;

public class SpellHit : MonoBehaviour
{
    public float spellDamage = 30f;
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
            player.TakePLayerDamage(spellDamage, transform.position);
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
