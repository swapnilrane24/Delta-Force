using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

namespace Curio.Gameplay
{
    public class EnemyActor : Actor
    {
        
        [SerializeField] protected Color blueTeamColor, redTeamColor;
        [SerializeField] protected PickUpDrop pickUpDrop; 
        [SerializeField] protected Enemy enemy;
        [SerializeField] protected ActorApperenceDecide actorApperence;
        [SerializeField] protected FillBarUI healthFillBar;
        [SerializeField] protected Target targetIndicator;
        [SerializeField] protected Outlinable outline;

        public bool ShouldBackOff
        {
            //current health is less than 20% of max health
            get => healthScript.healthRatio < 0.2f || currentWeapon.IsReloading;
        }

        public override void InitializeActor(TeamManager teamManager, int teamID)
        {
            selectedGunID = weaponSwitch.RandomGunIndex;

            base.InitializeActor(teamManager, teamID);
            enemy.InitializeEnemy(this);
            actorApperence.SelectApperence(_teamID);
            actorName = NameMaker.GetName();
            healthFillBar.SetFillvalue(1);
            healthScript.OnHealthRatioChangeEvent.AddListener(healthFillBar.SetFillvalue);
            actorNameUI.SetActorName(actorName);
            actorNameUI.SetTextColor(_teamID == 0 ? redTeamColor : blueTeamColor);
            //we know 0 is for red and 1 is for blue
            healthFillBar.SetFillColor(_teamID == 0 ? redTeamColor : blueTeamColor);
            targetIndicator.TargetColor = _teamID == 0 ? redTeamColor : blueTeamColor;

            if (_teamID != DeathMatchManager.Instance.PLayerTeamID)
            {
                pickUpDrop.PickUpWeaponIndex(selectedGunID);
            }
        }

        protected virtual void Update()
        {
            if (isAlive)
            {
                healthScript.HealthRegenerationProcess();
            }
        }

        public void DetectedByPlayer(bool isDetected)
        {
            outline.enabled = isDetected;
        }

        public override void Damage(ProjectileData projectileData)
        {
            base.Damage(projectileData);

            if (healthScript.IsDead)
            {
                //if the team is opponent team
                if (_teamID != DeathMatchManager.Instance.PLayerTeamID)
                {
                    pickUpDrop.Drop();
                }

            }

            if (isAlive)
                enemy.GotHit(projectileData.attackerActor);
        }

        public override void ListenFriendDeathAlert(Transform deathLocation)
        {
            if (enemy.Target == null && isAlive)
            {
                if (Vector3.Distance(transform.position, deathLocation.position) >= enemy.MinimumInvestigationDistanceFromObject)
                {
                    if (Random.Range(0, 10) <= enemy.InvestigateFriendDeathProbability)
                    {
                        enemy.StartMovement();
                        enemy.SetDestination(deathLocation.position);
                    }
                }
                
            }
        }

        public override void RespawnActor()
        {
            base.RespawnActor();
            currentWeapon.ResetWeapon();
            healthFillBar.SetFillvalue(1);
            enemy.ChangeState(StateEnum.PATROL);
            DetectedByPlayer(false);
        }

        public Transform GetRandomWaypoint()
        {
            return _teamManager.GetRandomWayPoint();
        }

        public override void DeathMatchTimeUp()
        {
            isAlive = false;
            enemy.StopMovement();
        }
    }
}