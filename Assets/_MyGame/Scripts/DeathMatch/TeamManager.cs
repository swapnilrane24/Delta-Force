using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Curio.Gameplay
{
    public class TeamManager : MonoBehaviour
    {

        [Tooltip("0 for red team and 1 for blue team")]
        [SerializeField] private int _teamID;
        [SerializeField] private PointsList spawnPoints;
        [SerializeField] private PointsList wayPoints;
        [SerializeField] private Material teamMaterial;
        [SerializeField] private bool spawnBots = true;

        private List<Actor> teamActors;

        int opponentTeamScore;
        int currentSpawnPointIndex;
        private PlayerActor _playerPrefab;
        private EnemyActor _botPrefab;

        private PlayerActor player;
        public UnityEvent onScoreChangeEvent;
        public int TeamID => _teamID;

        public int OpponentTeamScore => opponentTeamScore;
        public List<Actor> TeamActors => teamActors;

        private void OnDisable()
        {
            //DeathMatchManager.Instance.onDeathMatchFinishEvent.RemoveListener(DeathMatchFinishListner);
            GameManager.Instance.onLevelCompleteEvent.RemoveListener(DeathMatchFinishListner);
        }

        public void SetActorsPrefabs(PlayerActor playerPrefab, EnemyActor botPrefab)
        {
            _playerPrefab = playerPrefab;
            _botPrefab = botPrefab;
        }

        public void SetSpawnPointsAndWayPoints()
        {

        }

        public void InitializeTeams(bool playerTeam, int noOfTeamMembers)
        {
            currentSpawnPointIndex = Random.Range(0, spawnPoints.pointsList.Length);
            opponentTeamScore = 0;
            int count = noOfTeamMembers;

            teamActors = new List<Actor>();
            if (playerTeam)
            {
                //We spawn 1 less bot than total team members
                count = noOfTeamMembers - 1;
                player = Instantiate(_playerPrefab);
                player.InitializeActor(this, _teamID);

                teamActors.Add(player);

                player.SetPosition(spawnPoints.pointsList[currentSpawnPointIndex % spawnPoints.pointsList.Length].position);
                currentSpawnPointIndex++;
            }

            if (spawnBots)
            {
                for (int i = 0; i < count; i++)
                {
                    EnemyActor bot = Instantiate(_botPrefab);
                    bot.transform.name = _teamID == 0 ? "Bot [RedTeam]" : "Bot [BlueTeam]";
                    bot.transform.position = spawnPoints.pointsList[currentSpawnPointIndex % spawnPoints.pointsList.Length].position;
                    bot.InitializeActor(this, _teamID);
                    currentSpawnPointIndex++;
                    teamActors.Add(bot);
                }
            }

            //DeathMatchManager.Instance.onDeathMatchFinishEvent.AddListener(DeathMatchFinishListner);
            GameManager.Instance.onLevelCompleteEvent.AddListener(DeathMatchFinishListner);
        }

        private void Update()
        {
            if (GameManager.Instance.GameState != GameState.PLAYING) return;

            if (teamActors != null)
            {
                for (int i = 0; i < teamActors.Count; i++)
                {
                    if (teamActors[i].IsAlive == false)
                    {
                        teamActors[i].CountDownTimer();
                    }
                }
            }
        }

        private void DeathMatchFinishListner()
        {
            for (int i = 0; i < teamActors.Count; i++)
            {
                teamActors[i].DeathMatchTimeUp();
            }
        }

        public Vector3 GetRandomSpawnPoint()
        {
            return spawnPoints.pointsList[Random.Range(0, spawnPoints.pointsList.Length)].position;
        }

        public Transform GetRandomWayPoint()
        {
            return wayPoints.pointsList[Random.Range(0, wayPoints.pointsList.Length)];
        }

        public void ActorDead(Actor actor)
        {
            opponentTeamScore++;
            onScoreChangeEvent?.Invoke();

            if (actor.IsPlayer)
            {
                DeathMatchManager.Instance.PlayerDeadActivateRespawnUI();
            }

            for (int i = 0; i < teamActors.Count; i++)
            {
                teamActors[i].ListenFriendDeathAlert(actor.ActorTransfrom);
            }

        }

        public void RespawnPlayer()
        {
            player.RespawnActor();
        }

    }
}