using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DamageNumbersPro;
using static UnityEngine.InputSystem.InputControlScheme;

namespace Curio.Gameplay
{
    public class BaseDefenseManager : MonoBehaviour
    {
        private static BaseDefenseManager instance;
        public static BaseDefenseManager Instance => instance;

        [SerializeField] private Camera baseDefenseCamera;
        [SerializeField] private DamageNumber damageNumber;

        [SerializeField] private Wave[] waves;
        [SerializeField] private BaseArena[] baseArenas;
        [SerializeField] private PlayerActor playerPrefab;
        [SerializeField] private EnemyActor_BD botPrefab;

        private int baseDefenseLevel;
        private GameUI gameUI;

        public List<IActor> availableEnemyActorList;

        public UnityEvent onBaseDefenseWinEvent;
        public UnityEvent onBaseDefenseFailedEvent;
        public IActor GetBattery => currentBaseArena.GetBatteryTarget();
        public int BaseDefenseLevel => baseDefenseLevel;
        public GameUI GameUI { set => gameUI = value; }

        private PlayerActor player;
        private Wave currentWave;
        private BaseArena currentBaseArena;
        private int currentWaveIndex;
        private WaveManager waveManager;
        private int totalEnemies;
        private int totalEnemyKilled;
        private float countDownTimer;

        public int CurrentWaveIndex => currentWaveIndex;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            baseDefenseLevel = ES3.Load<int>("BaseLevel", 0);
        }

        public void InitializeBaseDefenceManager()
        {
            GameManager.Instance.GameMode = GameMode.DEFENSE;
            waveManager = new WaveManager();
            baseDefenseCamera.gameObject.SetActive(true);
            currentBaseArena = Instantiate(baseArenas[baseDefenseLevel % baseArenas.Length]);

            availableEnemyActorList = new List<IActor>();
            currentWave = waves[baseDefenseLevel % waves.Length];

            player = Instantiate(playerPrefab);
            player.InitializePlayer_BD();
            player.onDeadEvent.AddListener(LostBase);
            player.SetPosition(currentBaseArena.PlayerSpawnPoint.position);

            currentBaseArena.InitializeArena(currentWave.batteryLifeMultiplier);

            totalEnemies = 0;
            for (int i = 0; i < currentWave.waveDatas.Length; i++)
            {
                totalEnemies += currentWave.waveDatas[i].enemyCount;
            }

            waveManager.SetBaseDefenseManager(this);
            waveManager.SetWaveData(currentWave.waveDatas[currentWaveIndex]);
            GameManager.Instance.LevelStarted();

            gameUI.SetEnemyKillText(totalEnemyKilled + "/" + totalEnemies);

            countDownTimer = currentWave.waveDatas[currentWaveIndex].initialDelay;
            gameUI.ActivateCountDownHolder(true);
        }

        private void Update()
        {
            if (countDownTimer > 0)
            {
                countDownTimer -= Time.deltaTime;
                gameUI.UpdatCountDownText(countDownTimer);
                if (countDownTimer <= 0)
                {
                    gameUI.ActivateCountDownHolder(false);
                }
            }

            if (waveManager != null)
            {
                waveManager.OnUpdate();
            }
        }

        public IActor GetEnemy()
        {
            IActor enemy = null;

            if (availableEnemyActorList.Count > 0)
            {
                enemy = availableEnemyActorList[0];
                availableEnemyActorList.RemoveAt(0);
            }
            else
            {
                enemy = Instantiate(botPrefab, transform);
            }

            enemy.ActorTransfrom.gameObject.SetActive(true);
            enemy.OnDeadEvent.AddListener(ReturnEnemy);
            enemy.ActorTransfrom.position = currentBaseArena.GetEnemySpawnPosition();
            enemy.ActorTransfrom.GetComponent<EnemyActor_BD>().InitializeActorBD(currentWave.waveDatas[currentWaveIndex].healthMultiplier,
                currentWave.waveDatas[currentWaveIndex].damageMultiplier);
            return enemy;
        }

        public void SetBatteryRemaining(string value)
        {
            gameUI.SetBatteryRemainingText(value);
        }

        private void ReturnEnemy(IActor enemyActor)
        {
            totalEnemyKilled++;
            gameUI.SetEnemyKillText(totalEnemyKilled + "/" + totalEnemies);
            if (availableEnemyActorList.Contains(enemyActor))
            {
                availableEnemyActorList.Add(enemyActor);
            }
            enemyActor.OnDeadEvent.RemoveListener(ReturnEnemy);
            enemyActor.ActorTransfrom.gameObject.SetActive(false);
        }

        public void WaveComplete()
        {
            currentWaveIndex++;
            if (currentWaveIndex < currentWave.waveDatas.Length)
            {
                gameUI.ActivateCountDownHolder(true);
                countDownTimer = currentWave.waveDatas[currentWaveIndex].initialDelay;
                waveManager.SetWaveData(currentWave.waveDatas[currentWaveIndex]);
            }
            else
            {
                LevelCompleted();
            }
        }

        private void LevelCompleted()
        {
            Debug.Log("Level COmpleted");
            baseDefenseLevel++;
            ES3.Save<int>("BaseLevel", baseDefenseLevel);
            
            WaitExtension.Wait(this, 1.5f, () =>
            {
                onBaseDefenseWinEvent?.Invoke();
                GameManager.Instance.LevelFinish();
            });
            
        }

        public void LostBase(IActor actor)
        {
            Debug.Log("Level Failed");
            onBaseDefenseFailedEvent?.Invoke();
            GameManager.Instance.LevelFinish();
        }

        //public void LostBase()
        //{
        //    Debug.Log("Level Failed");
        //}



    }
}