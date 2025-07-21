using UnityEngine;
using Unity.Cinemachine;

public class CameraInitManager : MonoBehaviour
{
    public CinemachineConfiner2D cameraConfiner;

    private void Start()
    {
        string lastRoomID = PlayerPrefs.GetString("LastCheckpointRoomID", "DefaultRoom");
        GameObject lastRoom = GameObject.Find(lastRoomID);

        if (lastRoom != null)
        {
            BoxCollider2D bounds = lastRoom.GetComponentInChildren<BoxCollider2D>();
            if (bounds != null)
            {
                cameraConfiner.BoundingShape2D = bounds;
                lastRoom.SetActive(true);
            }
        }
    }
}
