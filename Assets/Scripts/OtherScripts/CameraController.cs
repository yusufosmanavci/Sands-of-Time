using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Transform player; // Reference to the player transform
    public float smoothSpeed = 5f; // Speed of camera movement

    public Vector2 minBounds;
    public Vector2 maxBounds;

    public float camHalfWidth; // Half width of the camera view
    public float camHalfHeight; // Half height of the camera view

    public bool IsNewRoomActivated = true;

    public RoomData roomData;

    private void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight; // Calculate half width based on camera aspect ratio
        minBounds = roomData.minBounds;
        maxBounds = roomData.maxBounds;
        Vector3 camStartPosition = new(player.position.x, player.position.y, -10f);
        transform.position = camStartPosition;
    }

    private void Update()
    {
        if (IsNewRoomActivated)
        {
            if (player == null) return;
            Vector2 desiredPoisition = player.position;

            float clampedX = Mathf.Clamp(desiredPoisition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            float clampedY = Mathf.Clamp(desiredPoisition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);
            Vector3 clampedPostion = new Vector3(clampedX, clampedY, transform.position.z);

            transform.position = Vector3.Lerp(transform.position, clampedPostion, smoothSpeed * Time.deltaTime);
        }


    }

    

    public void SetBounds(Vector2 min, Vector2 max, Vector3 cameraPosition)
    {
        IsNewRoomActivated = false; // Disable camera movement until bounds are set
        minBounds = min;
        maxBounds = max;
        transform.position = cameraPosition;
        IsNewRoomActivated = true;

    }



   /* public void UpdateRoomBorders()
    {
        GameObject border = GameObject.FindGameObjectWithTag("Borders");
        if (border != null)
        {
            roomBorder = border.GetComponent<BoxCollider2D>();
            if (roomBorder != null)
            {
                minBounds = roomBorder.bounds.min;
                maxBounds = roomBorder.bounds.max;
                //SetBounds(minBounds, maxBounds);
            }
        }
    }*/

}
