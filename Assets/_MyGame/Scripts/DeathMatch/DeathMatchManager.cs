using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DamageNumbersPro;

namespace Curio.Gameplay
{
    public class DeathMatchManager : MonoBehaviour
    {
        private static DeathMatchManager instance;
        public static DeathMatchManager Instance => instance;

        [SerializeField] private int respawnTime;
        [SerializeField] private bool canSpawnPlayer = true;
        [SerializeField] private DamageNumber damageNumber;

        [SerializeField] private Arena[] arenaPrefabs;
        [Tooltip("Time is in Minutes")]
        [SerializeField] private int deathMatchTimer;
        [SerializeField] private int numberOfTeamMembers;
        [SerializeField] private PlayerActor playerPrefab;
        [SerializeField] private EnemyActor botPrefab;
        

        public UnityEvent onDeathMatchFinishEvent;

        private RespawningMenu respawningMenu;
        private bool deathMatchTimeUp;
        public float currentDeathMatchTimer;
        int playerTeamAssignIndex;

        private GameUI gameUI;
        private MatchResultUI matchResultUI;
        private KillDataUI killDataUI;
        private TeamManager redTeamManager, blueTeamManager;
        private TeamManager playerTeam;
        private int arenaIndex;

        public int RespawnTime => respawnTime;
        public int NumberOfTeamMembers => numberOfTeamMembers;
        public RespawningMenu RespawningMenu { set => respawningMenu = value; }
        public GameUI GameUI { set => gameUI = value; }
        public MatchResultUI MatchResultUI { set => matchResultUI = value; }
        public KillDataUI KillDataUI { set => killDataUI = value; }
        public int PLayerTeamID => playerTeam.TeamID;

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
        }

        public void SelectedArenaIndex(int index)
        {
            arenaIndex = index;
        }

        public void InitializeDeathMatchManager()
        {
            Arena arena = Instantiate(arenaPrefabs[arenaIndex]);
            if (arena)
            {
                redTeamManager = arena.RedTeamManager;
                blueTeamManager = arena.BlueTeamManager;
                arena.InitializeArena();
            }
            else
            {
                Debug.Log("No Arena");
            }
        }

        public void PlayerBlueTeamSelected()
        {
            playerTeamAssignIndex = 1;
        }

        public void PlayerRedTeamSelected()
        {
            playerTeamAssignIndex = 0;
        }

        public void StartMatch()
        {
            deathMatchTimeUp = false;
            currentDeathMatchTimer = deathMatchTimer * 60;
            redTeamManager.SetActorsPrefabs(playerPrefab, botPrefab);
            blueTeamManager.SetActorsPrefabs(playerPrefab, botPrefab);

            if (playerTeamAssignIndex == 0)
            {
                playerTeam = redTeamManager;
                redTeamManager.InitializeTeams(canSpawnPlayer, numberOfTeamMembers);
                blueTeamManager.InitializeTeams(false, numberOfTeamMembers);
            }
            else
            {
                playerTeam = blueTeamManager;
                redTeamManager.InitializeTeams(false, numberOfTeamMembers);
                blueTeamManager.InitializeTeams(canSpawnPlayer, numberOfTeamMembers);
            }

            redTeamManager.onScoreChangeEvent.AddListener(UpdateScoreListner);
            blueTeamManager.onScoreChangeEvent.AddListener(UpdateScoreListner);

            gameUI.SetScore(redTeamManager.OpponentTeamScore, blueTeamManager.OpponentTeamScore);

            GameManager.Instance.DeathMatchStarted();
        }

        private void Update()
        {
            if (currentDeathMatchTimer > 0)
            {
                currentDeathMatchTimer -= Time.deltaTime;
                if (currentDeathMatchTimer <= 0)
                {
                    GameManager.Instance.DeathMatchFinish();
                    onDeathMatchFinishEvent?.Invoke();
                    matchResultUI.SetScore(redTeamManager.OpponentTeamScore, blueTeamManager.OpponentTeamScore);
                    matchResultUI.ToggleVisibility(true);
                }

                gameUI.SetTimer(currentDeathMatchTimer);
            }
        }

        private void UpdateScoreListner()
        {
            gameUI.SetScore(redTeamManager.OpponentTeamScore, blueTeamManager.OpponentTeamScore);
        }

        public void PlayerDeadActivateRespawnUI()
        {
            respawningMenu.ActivateRespawnScreen(respawnTime);
        }

        public void RespawnPlayer()
        {
            playerTeam.RespawnPlayer();
        }

        public void KillListner(KillUIData killUIData)
        {
            killDataUI.PopulateData(killUIData);
        }

        public DamageNumber GetDamageNumber()
        {
            return damageNumber;
        }
    }
}