using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject go_BaseUI;
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
            Debug.Log("세이브");
        }

        public void ClickLoad()
        {
            Debug.Log("로드");
        }
        public void ClickExit()
        {
            Debug.Log("종료");
            Application.Quit();
        }
    }
}

