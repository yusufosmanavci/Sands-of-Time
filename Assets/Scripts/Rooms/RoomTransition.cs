using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RoomSystem
{
    public RoomData RoomData;
}
public class RoomTransition : MonoBehaviour
{
    public static RoomTransition Instance { get; private set; }

    public List<RoomSystem> rooms = new();

    private void Awake()
    {
        Instance = this;
    }
  

    public IEnumerator LoadRoom(RoomData roomData)
    {
        SceneFadeManager.instance.StartFadeOut();
        while (SceneFadeManager.instance.IsFadingOut)
        {
            yield return null;
        }
        CameraController.Instance.SetBounds(roomData.minBounds, roomData.maxBounds, roomData.cameraPosition, roomData.cameraSize);
        PlayerManager.Instance.playerController.SetCharacterPosition(roomData.playerPosition);
        SceneFadeManager.instance.StartFadeIn();
    }
}
