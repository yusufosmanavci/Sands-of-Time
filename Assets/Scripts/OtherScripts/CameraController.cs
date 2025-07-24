using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public float smoothSpeed = 5f; // Speed of camera movement
    public BoxCollider2D roomBorder; // Reference to the room GameObject

    public Vector2 minBounds;
    public Vector2 maxBounds;

    public float camHalfWidth; // Half width of the camera view
    public float camHalfHeight; // Half height of the camera view

    private void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight; // Calculate half width based on camera aspect ratio
        if(roomBorder == null)
        {
            GameObject border = GameObject.FindGameObjectWithTag("Borders");
            if(border != null)
            {
                roomBorder = border.GetComponent<BoxCollider2D>();
            }
        }
        minBounds = roomBorder.bounds.min;
        maxBounds = roomBorder.bounds.max;
        SetBounds(minBounds, maxBounds);
    }

    private void Update()
    {
        if (player == null) return;
        Vector2 desiredPoisition = player.position;

        float clampedX = Mathf.Clamp(desiredPoisition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPoisition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);
        Vector3 clampedPostion = new Vector3(clampedX, clampedY, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, clampedPostion, smoothSpeed * Time.deltaTime);

    }

    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
    }

    public void UpdateRoomBorders()
    {
        GameObject border = GameObject.FindGameObjectWithTag("Borders");
        if (border != null)
        {
            roomBorder = border.GetComponent<BoxCollider2D>();
            if (roomBorder != null)
            {
                minBounds = roomBorder.bounds.min;
                maxBounds = roomBorder.bounds.max;
                SetBounds(minBounds, maxBounds);
            }
        }
    }
}
