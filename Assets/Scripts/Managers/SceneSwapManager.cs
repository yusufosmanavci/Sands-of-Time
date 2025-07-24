using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager Instance;

    private DoorInteraction.DoorToSpawnAt _doorToSpawnTo;

    public static bool loadFromDoor;

    private GameObject _player;
    private Collider2D _playerCollider;
    private Collider2D _doorCollider;
    private Vector3 _playerSpawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCollider = _player.GetComponent<Collider2D>();

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void SwapSceneFromDoorUse(SceneField myscene, DoorInteraction.DoorToSpawnAt doorToSpawnAt)
    {
        loadFromDoor = true;
        Instance.StartCoroutine(Instance.FadeOutAndThenChangeScene(myscene, doorToSpawnAt));
    }

    private IEnumerator FadeOutAndThenChangeScene(SceneField myscene, DoorInteraction.DoorToSpawnAt doorToSpawnAt = DoorInteraction.DoorToSpawnAt.None)
    {
        InputManager.DeactivatePlayerControls();
        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.IsFadingOut)
        {
            yield return null;
        }

        _doorToSpawnTo = doorToSpawnAt;

        SceneManager.LoadScene(myscene);
    }

    private IEnumerator ActivePlayerControlsAfterFadeIn()
    {
        while (SceneFadeManager.instance.IsFadingIn)
        {
            yield return null;
        }

        InputManager.ActivatePlayerControls();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();

        if (loadFromDoor)
        {
            StartCoroutine(ActivePlayerControlsAfterFadeIn());
            FindDoor(_doorToSpawnTo);
            _player.transform.position = _playerSpawnPosition;

            CameraController cam = FindAnyObjectByType<CameraController>();
            cam.UpdateRoomBorders();

            loadFromDoor = false;
        }
    }

    private void FindDoor(DoorInteraction.DoorToSpawnAt doorSpawnNumber)
    {
        DoorInteraction[] doors = FindObjectsByType<DoorInteraction>(FindObjectsSortMode.None);

        for(int i =0; i< doors.Length; i++)
        {
            if (doors[i].currentDoorPosition == doorSpawnNumber)
            {
                _doorCollider = doors[i].gameObject.GetComponent<Collider2D>();

                CalculateSpawnPosition();
                return;
            }
        }
    }

    private void CalculateSpawnPosition()
    {
        float colliderHeight = _playerCollider.bounds.extents.y;
        _playerSpawnPosition = _doorCollider.transform.position - new Vector3(0f, colliderHeight, 0f);
    }
}
