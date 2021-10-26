using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject go_BaseUI;
        [SerializeField] private SaveNLoadManager saveNLoad;
        InputHandler inputHandler;

        private void Awake()
        {
            inputHandler = FindObjectOfType<InputHandler>();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                if (!GameManager.isPause)
                {
                    CallMenu();
                    inputHandler.OnDisable();
                }
                else
                {
                    CloseMenu();
                    inputHandler.OnEnable();
                }
            }
        }

        private void CallMenu()
        {
            GameManager.isPause = true;
            go_BaseUI.SetActive(true);
        }

        private void CloseMenu()
        {
            GameManager.isPause = false;
            go_BaseUI.SetActive(false);
        }

        public void ClickSave()
        {
            Debug.Log("���̺�");
            saveNLoad.SaveData();
        }

        public void ClickLoad()
        {
            Debug.Log("�ε�");
            saveNLoad.LoadDate();
        }
        public void ClickExit()
        {
            Debug.Log("����");
            Application.Quit();
        }
    }
}

