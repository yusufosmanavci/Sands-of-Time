using UnityEngine;

public class SpellHit : MonoBehaviour
{
    public float spellDamage = 30f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
            player.TakePLayerDamage(spellDamage, transform.position);
            Debug.Log("Spell hit the player");
        }
    }
}
