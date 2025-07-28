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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            canInteract = true;
            StartCoroutine(RoomTransition.Instance.LoadRoom(nextRoom));
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
