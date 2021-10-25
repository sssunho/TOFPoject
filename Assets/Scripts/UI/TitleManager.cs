using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TOF
{
    public class TitleManager : MonoBehaviour
    {
        public string sceneName = "1Phase_2";

        public void ClickStart()
        {
            Debug.Log("�ε�");
            SceneManager.LoadScene(sceneName);
        }

        public void ClickLoad()
        {
            Debug.Log("�ε�");
        }

        public void ClickExit()
        {
            Debug.Log("����");
            Application.Quit();
        }
    }
}

