using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Micosmo.SensorToolkit;

namespace Curio.Gameplay
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float acceleration = 6, decceleration = 60;
        [SerializeField] protected Transform targetLocation;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float chaseRange;
        [SerializeField] protected float seekRange;
        [Range(0, 10)]
        [SerializeField] private int investigateFriendDeathProbability = 0;
        [SerializeField] private int minimumInvestigationDistanceFromObject;
        [SerializeField] protected StateMachine stateMachine;
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected RangeSensor seekSensor;
        [SerializeField] protected RangeSensor rangeSensor;
        [SerializeField] protected LOSSensor enemyDetectionSensor;

        protected float attackDistance;
        protected Actor target;
        protected EnemyActor enemyActor;
        private bool isInitialized;
        private Vector3 seekDestination;
        private bool isSeekingTarget;

        public int InvestigateFriendDeathProbability => investigateFriendDeathProbability;
        public int MinimumInvestigationDistanceFromObject => minimumInvestigationDistanceFromObject;
        public float AttackDistance => attackDistance;
        public NavMeshAgent NavMeshAgent => navMeshAgent;
        public WeaponController SelectedWeapon => enemyActor.SelectedWeapon;
        public Actor Target { get => target; }
        public float CurrentMoveSpeed => moveSpeed;
        public float ChaseRange => chaseRange;
        public EnemyActor EnemyActor => enemyActor;
        public Vector3 SeekDestination => seekDestination;
        public bool IsSeekingTarget { get => isSeekingTarget; set => isSeekingTarget = value; }

        private Vector3 spawnLocation;

        private float lookForTargetInterval = 0.5f; //twice every second
        private float currentLookTargetInterval;

        private float seekTargetInterval = 0.5f; //twice every second
        private float currentSeekTargetInterval;

        private void OnDisable()
        {
            enemyDetectionSensor.OnLostDetection.RemoveListener(OnTargetLostListner);
        }

        public void InitializeEnemy(EnemyActor actor)
        {
            enemyActor = actor;

            spawnLocation = transform.position;

            stateMachine.Initialize(this);

            attackDistance = enemyActor.SelectedWeapon.AttackRange * 0.75f; //we make attack range 75% of gun range
            //isAlive = true;

            seekSensor.SetSphereShape(seekRange);
            rangeSensor.SetSphereShape(chaseRange);
            //enemyDetectionSensor.OnDetected.AddListener(OnTargetDetectionListner);
            enemyDetectionSensor.OnLostDetection.AddListener(OnTargetLostListner);

            isInitialized = true;

            navMeshAgent.enabled = true;

            WaitExtension.Wait(this, 0.1f, () =>
            {
                transform.position = spawnLocation;
            });

            navMeshAgent.speed = CurrentMoveSpeed;
            currentLookTargetInterval = lookForTargetInterval + Time.time;
            currentSeekTargetInterval = seekTargetInterval + Time.time;
            //aIPathControl.SetSpeed(CurrentMoveSpeed);
        }

        public void GotHit(Actor attackerActor)
        {
            target = attackerActor;
            //SetDestination(attackerActor.ActorTransfrom.position);
        }

        private void Update()
        {
            if (isInitialized && enemyActor.IsAlive)
            {
                if (stateMachine)
                {
                    if (stateMachine.CurrentState != null)
                    {
                        enemyActor.SetActorWalkAnim(navMeshAgent.velocity.magnitude > 0);

                        if (stateMachine.CurrentState.State != StateEnum.ATTACK)
                        {
                            enemyActor.SetActorDirectionAnim(navMeshAgent.velocity.magnitude * Vector2.up);
                        }
                    }

                    stateMachine.OnUpdate();
                }

                LookForTarget();
            }
        }

        public void EnemyAlertListner(Enemy hitEnemy)
        {
            if (enemyActor.IsAlive)
            {
                if (target == null)
                {
                    navMeshAgent.SetDestination(hitEnemy.transform.position);
                }
            }
        }

        public void ChangeState(StateEnum stateEnum)
        {
            stateMachine.SwitchToNextState(stateEnum);
        }

        private void LookForTarget()
        {
            if (target == null)
            {
                if (currentLookTargetInterval < Time.time)
                {
                    currentLookTargetInterval = lookForTargetInterval + Time.time;
                    Actor actor = enemyDetectionSensor.GetNearestComponent<Actor>();

                    if (actor != null)
                    {
                        if (actor.TeamID != enemyActor.TeamID)
                        {
                            target = actor;
                            target.onDeadEvent.AddListener(OnTargetDeadListner);
                        }
                    }
                }
            }
            else
            {
                if (target.IsAlive == false)
                {
                    target.onDeadEvent.RemoveListener(OnTargetDeadListner);
                    target = null;
                }
            }
        }

        public void SeekTarget()
        {
            if (stateMachine)
            {
                if (target == null)
                {
                    if (isSeekingTarget == false)
                    {
                        if (currentSeekTargetInterval < Time.time)
                        {
                            currentSeekTargetInterval = seekTargetInterval + Time.time;

                            Actor seekActor = seekSensor.GetNearestComponent<Actor>();

                            if (seekActor)
                            {
                                if (seekActor.TeamID != enemyActor.TeamID)
                                {
                                    //isSeekingTarget = true;
                                    seekDestination = seekActor.transform.position;
                                    stateMachine.SwitchToNextState(StateEnum.SEEK);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetPatrolTargetAndMove()
        {
            targetLocation = enemyActor.GetRandomWaypoint();// GetRandomWayPoint();
   
            if (targetLocation)
            {
                SetDestination(targetLocation.position);
            }
            else
            {
                stateMachine.SwitchToNextState(StateEnum.IDLE);
            }
        }

        public void SetDestination(Vector3 position)
        {
            navMeshAgent.SetDestination(position);
        }

        public void StopMovement()
        {
            if (gameObject.activeInHierarchy)
            {
                navMeshAgent.acceleration = decceleration;
                navMeshAgent.SetDestination(transform.position);
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
            }
        }

        public void StartMovement()
        {
            navMeshAgent.acceleration = acceleration;
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = CurrentMoveSpeed;
        }

        private void OnTargetDeadListner(Actor actor)
        {
            if (target == actor)
            {
                actor.onDeadEvent.RemoveListener(OnTargetDeadListner);
                target = null;
            }
        }

        protected void OnTargetLostListner(GameObject value, Sensor sensor)
        {
            if (value.TryGetComponent<Actor>(out Actor actor))
            {
                if (target == actor)
                {
                    //OnTargetDeadListner(enemy);
                }
            }
        }

        public void LostTarget()
        {
            //_target.onDeadEvent.RemoveListener(OnTargetDeadListner);
            target = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, chaseRange);

            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, seekRange);

            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, minimumInvestigationDistanceFromObject);
        }


#endif

    }
}