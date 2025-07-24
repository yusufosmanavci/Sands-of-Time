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

            
        }
    }
}