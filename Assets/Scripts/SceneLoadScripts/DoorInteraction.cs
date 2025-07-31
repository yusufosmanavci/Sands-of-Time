using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public enum DoorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
    }

    public GameObject Player { get; set; }
    public bool canInteract { get; set; }

    [Header("Spawn To")]
    [SerializeField] private DoorToSpawnAt doorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;

    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt currentDoorPosition;

    public RoomData nextRoom;
    public GameObject roomToActivate;
    public GameObject roomToDeactivate;
    EnemySpawner enemySpawner;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (canInteract)
        {
            //SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, doorToSpawnTo);
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            canInteract = true;
            StartCoroutine(RoomTransition.Instance.LoadRoom(nextRoom));
            PlayerManager.Instance.playerValues.deathRoom = roomToActivate;
            while (SceneFadeManager.instance.IsFadingOut)
            {
                yield return null;
            }
            roomToDeactivate.SetActive(false); // Disable the door after interaction

            enemySpawner = roomToActivate.GetComponentInChildren<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.ClearEnemies(); // Clear any existing enemies in the new room
                enemySpawner.SpawnEnemies(); // Spawn new enemies in the new room
            }

            if (roomToDeactivate != null)
            {
                enemySpawner = roomToDeactivate.GetComponentInChildren<EnemySpawner>();
                if (enemySpawner != null)
                {
                    enemySpawner.ClearEnemies(); // Clear any existing enemies in the old room
                }
                roomToDeactivate.SetActive(false); // Deactivate the old room if it exists
            }
            roomToActivate.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            canInteract = false;
        }
    }
}
