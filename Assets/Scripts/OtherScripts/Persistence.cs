using UnityEngine;

public class Persistence : MonoBehaviour
{
    public static Persistence Instance { get; private set; } // Singleton instance of Persistence

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Keep this instance across scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
}
