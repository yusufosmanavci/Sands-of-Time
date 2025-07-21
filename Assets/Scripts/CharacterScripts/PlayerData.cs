using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

namespace Assets.Scripts.CharacterScripts
{
    public class PlayerData : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12)) // Örneğin F12 tuşuna basınca tüm kayıtlar silinsin
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("PlayerPrefs resetlendi.");
            }
        }

        public void SandsOfTimeSave()
        {
            PlayerPrefs.SetInt("Sands Of Time", PlayerManager.Instance.playerValues.sandsOfTime);
        }

        public void CheckPointSave()
        {
            PlayerPrefs.SetFloat("lastCheckpointX", PlayerValues.lastCheckpointPosition.x);
            PlayerPrefs.SetFloat("lastCheckpointY", PlayerValues.lastCheckpointPosition.y);
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey("Sands Of Time"))
            {
                PlayerManager.Instance.playerValues.sandsOfTime = PlayerPrefs.GetInt("Sands Of Time");
            }
            else
            {
                PlayerManager.Instance.playerValues.sandsOfTime = 0; // Default value if not set
            }

            if (PlayerPrefs.HasKey("lastCheckpointX") && PlayerPrefs.HasKey("lastCheckpointY"))
            {
                float x = PlayerPrefs.GetFloat("lastCheckpointX");
                float y = PlayerPrefs.GetFloat("lastCheckpointY");
                PlayerValues.lastCheckpointPosition = new Vector2(x, y);
            }
            else
            {
                PlayerValues.lastCheckpointPosition = Vector2.zero; // Default value if not set
            }
        }
    }
}