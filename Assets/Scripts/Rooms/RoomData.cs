using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable Objects/RoomData")]
public class RoomData : ScriptableObject
{
    public int roomID; // Unique identifier for the room
    public Vector3 cameraPosition;
    public float cameraSize;
    public Vector2 playerPosition; // Position where the player spawns in this room
    public Vector2 minBounds;
    public Vector2 maxBounds; // Bounds of the room for camera clamping
}