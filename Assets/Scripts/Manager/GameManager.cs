using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TOF
{
    using UnityEngine.Events;

    public class GameManager : MonoBehaviour
    {
        [System.Serializable]
        public class OnRealoadGame : UnityEngine.Events.UnityEvent { }
        [Header("Player")]
        public GameObject playerPrefab;
        public CameraHandler cameraHandler;
        public GameObject playerUI;

        [Header("Enemy")]
        public GameObject enemyPrefab;
        public Transform[] enemySpawnPoint;

        [Header("Spawn")]
        public Bonfire[] bonfires;
        public Button[] checkPoints;
        public GameObject TeleportWindow;
        public GameObject ContentWindow;
        public Button CheckPointBTNPrefab;
        public Transform spawnPoint;
        public float respawnTimer = 4f;
        public bool destroyBodyAfterDead;
        public bool displayInfoInFadeText = true;

        [HideInInspector]
        public OnRealoadGame OnReloadGame = new OnRealoadGame();
        [HideInInspector]
        public GameObject currentPlayer;
        private PlayerStats currentController;
        public static GameManager instance;
        private GameObject oldPlayer;

        public static bool isPause = false; // 메뉴가 호출되면 true

        public UnityEvent onSpawn;

        protected virtual void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
                this.gameObject.name = gameObject.name + " Instance";
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            FindPlayer();
            SpawnEnemy();
            FineBonFirePoint();
        }

        public virtual void OnCharacterDead(GameObject _gameObject)
        {
            oldPlayer = _gameObject;

            if (playerPrefab != null)
            {
                StartCoroutine(RespawnRoutine());
            }
            else
            {
                Invoke("ResetScene", respawnTimer);
            }
        }

        protected virtual IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnTimer);

            if (playerPrefab != null && spawnPoint != null)
            {

                if (oldPlayer != null && destroyBodyAfterDead)
                {
                    Destroy(oldPlayer);
                }
                else
                {
                    DestroyPlayerComponents(oldPlayer);
                }
                yield return new WaitForEndOfFrame();

                currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
                currentController = currentPlayer.GetComponent<PlayerStats>();
                cameraHandler.transform.position = spawnPoint.transform.position;
                cameraHandler.FindTarget();

                OnReloadGame.Invoke();
                onSpawn.Invoke();
            }
        }

        protected virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (currentController.currentHealth > 0)
            {
                return;
            }

            OnReloadGame.Invoke();
            FindPlayer();
        }

        protected virtual void FindPlayer()
        {
            var player = GameObject.FindObjectOfType<PlayerStats>();
            var cam = GameObject.FindObjectOfType<CameraHandler>();

            if (player)
            {
                currentPlayer = player.gameObject;
                currentController = player;
                cameraHandler = cam;
            }
            else if (currentPlayer == null && playerPrefab != null && spawnPoint != null)
            {
                SpawnAtPoint(spawnPoint);
            }
        }

        protected virtual void DestroyPlayerComponents(GameObject target)
        {
            if (!target)
            {
                return;
            }
            var comps = target.GetComponentsInChildren<MonoBehaviour>();
            for (int i = 0; i < comps.Length; i++)
            {
                Destroy(comps[i]);
            }
            var coll = target.GetComponent<Collider>();
            if (coll != null)
            {
                Destroy(coll);
            }
            var rigidbody = target.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                Destroy(rigidbody);
            }
            var animator = target.GetComponent<Animator>();
            if (animator != null)
            {
                Destroy(animator);
            }
        }

        /// <summary>
        /// Set a custom spawn point (or use it as checkpoint to your level) 
        /// </summary>
        /// <param name="newSpawnPoint"> new point to spawn</param>
        public virtual void SetSpawnSpoint(Transform newSpawnPoint)
        {
            spawnPoint = newSpawnPoint;
        }

        /// <summary>
        /// Spawn New Player at a specific point
        /// </summary>
        /// <param name="targetPoint"> Point to spawn player</param>
        public virtual void SpawnAtPoint(Transform targetPoint)
        {
            if (playerPrefab != null)
            {
                if (oldPlayer != null && destroyBodyAfterDead)
                {
                    Destroy(oldPlayer);
                }
                else if (oldPlayer != null)
                {
                    DestroyPlayerComponents(oldPlayer);
                }
                else if(oldPlayer == null)
                {
                    oldPlayer = currentPlayer;
                    Destroy(oldPlayer);
                }
                currentPlayer = Instantiate(playerPrefab, targetPoint.position, targetPoint.rotation);
                currentController = currentPlayer.GetComponent<PlayerStats>();
                OnReloadGame.Invoke();
            }
        }

        /// <summary>
        /// Reload  current Scene and current Player
        /// </summary>
        public virtual void ResetScene()
        {
            if (oldPlayer)
            {
                DestroyPlayerComponents(oldPlayer);
            }
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            if (oldPlayer && destroyBodyAfterDead)
            {
                Destroy(oldPlayer);
            }
        }

        public void ResetHealth()
        {
            currentController.HealPlayer(currentController.maxHealth);
        }

        public void ResetEnemy()
        {
            for (int i = 1; i < enemySpawnPoint.Length; i++)
            {
                var oldEnemy = enemySpawnPoint[i].GetChild(0).gameObject;
                Destroy(oldEnemy);
                Instantiate(enemyPrefab, enemySpawnPoint[i].position, enemySpawnPoint[i].rotation).transform.parent = enemySpawnPoint[i].transform;
            }
        }
        public void SpawnEnemy()
        {
            enemySpawnPoint = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
            for (int i = 1; i < enemySpawnPoint.Length; i++)
                Instantiate(enemyPrefab, enemySpawnPoint[i].position, enemySpawnPoint[i].rotation).transform.parent = enemySpawnPoint[i].transform;
        }

        public void FineBonFirePoint()
        {
            bonfires = GameObject.Find("BonFirePoint").GetComponentsInChildren<Bonfire>();
            for (int i = 0; i < bonfires.Length; i++)
            {
                Button button = Instantiate(CheckPointBTNPrefab, ContentWindow.transform);
                button.interactable = false;
                button.transform.GetChild(0).GetComponent<Text>().text = bonfires[i].name;
                int num = i;
                button.onClick.AddListener(() => { SetDestination(num); });
            }
            checkPoints = ContentWindow.GetComponentsInChildren<Button>();
        }

        public void UpdateCheckPoint(string name)
        {
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (name.Equals(checkPoints[i].transform.GetChild(0).GetComponent<Text>().text))
                    checkPoints[i].interactable = true;
            }
        }

        public void SetDestination(int index)
        {
            Debug.Log(index);
            SetSpawnSpoint(bonfires[index].checkPoint);
            TeleportWindow.SetActive(false);
            var player = currentPlayer.GetComponent<PlayerManager>();
            player.isBonFire = false;
            StartCoroutine(Teleport());
        }

        protected virtual IEnumerator Teleport()
        {
            yield return new WaitForSeconds(3f);
            var player = currentPlayer.GetComponent<PlayerManager>();
            var playerController = currentPlayer.GetComponent<CharacterController>();
            playerController.enabled = false;
            player.transform.position = spawnPoint.position;
            playerController.enabled = true;
            playerUI.SetActive(true);
        }

        public void CancleTeleport()
        {
            TeleportWindow.SetActive(false);
            var player = currentPlayer.GetComponent<PlayerManager>();
            player.isBonFire = false;
            playerUI.SetActive(true);
        }
    }
}