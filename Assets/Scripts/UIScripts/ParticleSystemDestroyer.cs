using UnityEngine;

public class ParticleSystemDestroyer : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
}
