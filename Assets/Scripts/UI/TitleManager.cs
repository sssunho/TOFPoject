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
            Debug.Log("로딩");
            SceneManager.LoadScene(sceneName);
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

