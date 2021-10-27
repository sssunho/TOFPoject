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

        public List<int> RightWeaponSlotArrayNumber = new List<int>();
        public List<string> RightWeaponSlotName = new List<string>();
        public List<int> LeftWeaponSlotArrayNumber = new List<int>();
        public List<string> LeftWeaponSlotName = new List<string>();
        public List<int> ConsumableSlotArrayNumber = new List<int>();
        public List<string> ConsumableSlotName = new List<string>();
        public List<int> SpellSlotArrayNumber = new List<int>();
        public List<string> SpellSlotName = new List<string>();
        public int UnEquipWeaponInventorySize;
        public List<string> WeaponInventoryItemName = new List<string>();

    }

    public class SaveNLoadManager : MonoBehaviour
    {
        private SaveData saveData = new SaveData();

        private string SAVE_DATA_DIRECTORY;
        private string SAVE_FILENAME = "/SaveFile.txt";

        private PlayerManager player;
        private PlayerInventory playerInventory;

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

            SaveInvenData();

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

                LoadInvenData();
            }
            else
            {
                Debug.Log("파일이 존재하지 않습니다.");
            }
        }

        private void SaveInvenData()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();

            WeaponItem[] weaponsInRightHandSlot = playerInventory.GetRightWeaponItems();
            WeaponItem[] weaponsInLeftHandSlot = playerInventory.GetLeftWeaponItems();
            ConsumableItem[] consumablesSlot = playerInventory.GetConsumableItems();
            SpellItem[] spellsSlot = playerInventory.GetSpellItems();
            List<WeaponItem> weaponsInventory = playerInventory.GetWeaponInventory();
            int cnt = 0;
            for (int i = 0; i < weaponsInRightHandSlot.Length; i++)
            {
                if (weaponsInRightHandSlot[i] != null)
                {
                    saveData.RightWeaponSlotArrayNumber.Add(i);
                    saveData.RightWeaponSlotName.Add(weaponsInRightHandSlot[i].itemName);
                    cnt++;
                }
            }
            for (int i = 0; i < weaponsInLeftHandSlot.Length; i++)
            {
                if (weaponsInLeftHandSlot[i] != null)
                {
                    saveData.LeftWeaponSlotArrayNumber.Add(i);
                    saveData.LeftWeaponSlotName.Add(weaponsInLeftHandSlot[i].itemName);
                    cnt++;
                }
            }
            for (int i = 0; i < consumablesSlot.Length; i++)
            {
                if (consumablesSlot[i] != null)
                {
                    saveData.ConsumableSlotArrayNumber.Add(i);
                    saveData.ConsumableSlotName.Add(consumablesSlot[i].itemName);
                }
            }
            for (int i = 0; i < spellsSlot.Length; i++)
            {
                if (spellsSlot[i] != null)
                {
                    saveData.SpellSlotArrayNumber.Add(i);
                    saveData.SpellSlotName.Add(spellsSlot[i].itemName);
                }
            }
            for(int i = cnt; i < weaponsInventory.Count; i++)
            {
                if(weaponsInventory[i] != null)
                    saveData.WeaponInventoryItemName.Add(weaponsInventory[i].itemName);
            }
            saveData.UnEquipWeaponInventorySize = weaponsInventory.Count - cnt;
        }

        private void LoadInvenData()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            playerInventory.InvenWeapons = new WeaponItem[saveData.UnEquipWeaponInventorySize];

            for (int i = 0; i < saveData.RightWeaponSlotName.Count; i++)
                playerInventory.LoadToRightWeapon(
                    saveData.RightWeaponSlotArrayNumber[i],
                    saveData.RightWeaponSlotName[i]);

            for (int i = 0; i < saveData.LeftWeaponSlotName.Count; i++)
                playerInventory.LoadToLefttWeapon(
                    saveData.LeftWeaponSlotArrayNumber[i],
                    saveData.LeftWeaponSlotName[i]);

            for (int i = 0; i < saveData.WeaponInventoryItemName.Count; i++)
                playerInventory.LoadToInven(i, saveData.WeaponInventoryItemName[i]);

            for (int i = 0; i < saveData.ConsumableSlotName.Count; i++)
                playerInventory.LoadToConsumableItem(
                    saveData.ConsumableSlotArrayNumber[i],
                    saveData.ConsumableSlotName[i]);

            for (int i = 0; i < saveData.SpellSlotName.Count; i++)
                playerInventory.LoadToSpellItem(
                    saveData.SpellSlotArrayNumber[i],
                    saveData.SpellSlotName[i]);
        }
    }
}


