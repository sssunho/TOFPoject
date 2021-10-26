using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TOF
{
    public class TitleManager : MonoBehaviour
    {
        public string sceneName = "1Phase_2";
        public static TitleManager instance;
        private SaveNLoadManager saveNLoad;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(this.gameObject);
        }

        public void ClickStart()
        {
            Debug.Log("로딩");
            SceneManager.LoadScene(sceneName);
        }

        public void ClickLoad()
        {
            Debug.Log("로드");
            StartCoroutine(LoadCoroutine());
        }

        public void ClickExit()
        {
            Debug.Log("종료");
            Application.Quit();
        }

        IEnumerator LoadCoroutine()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while(!operation.isDone)
            {
                // Loading Scene operation.process 활용
                yield return null;
            }

            saveNLoad = FindObjectOfType<SaveNLoadManager>();
            saveNLoad.LoadDate();
            gameObject.SetActive(false);
        }
    }
}

