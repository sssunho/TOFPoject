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

            // #. Save ���� �ڵ� ����
            if(!Directory.Exists(SAVE_DATA_DIRECTORY))
                Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
        
        public void SaveData()
        {
            player = FindObjectOfType<PlayerManager>();
            saveData.playerPos = player.transform.position;
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
            Debug.Log("���� �Ϸ�");
            Debug.Log(json);
        }
    }
}


