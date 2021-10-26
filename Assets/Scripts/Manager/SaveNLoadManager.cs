using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace TOF
{
    [System.Serializable]
    public class SaveData
    {
        public Vector3 playerPos;
        public Vector3 playerRot;
    }

    public class SaveNLoadManager : MonoBehaviour
    {
        private SaveData saveData = new SaveData();

        private string SAVE_DATA_DIRECTORY;
        private string SAVE_FILENAME = "/SaveFile.txt";

        private PlayerManager player;

        void Start()
        {
            SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

            // #. Save 파일 자동 생성
            if(!Directory.Exists(SAVE_DATA_DIRECTORY))
                Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
        
        public void SaveData()
        {
            player = FindObjectOfType<PlayerManager>();
            saveData.playerPos = player.transform.position;
            saveData.playerRot = player.transform.eulerAngles;

            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
            Debug.Log("저장 완료");
            Debug.Log(json);
        }

        public void LoadDate()
        {
            if(File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
            {
                string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
                saveData = JsonUtility.FromJson<SaveData>(loadJson);

                player = FindObjectOfType<PlayerManager>();
                var playerController = player.GetComponent<CharacterController>();
                playerController.enabled = false;
                player.transform.position = saveData.playerPos;
                player.transform.eulerAngles = saveData.playerRot;
                playerController.enabled = true;
            }
            else
            {
                Debug.Log("파일이 존재하지 않습니다.");
            }

        }
    }
}


