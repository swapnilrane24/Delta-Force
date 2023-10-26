using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

namespace Curio.Gameplay
{
    [System.Serializable]
    public struct WaypointData
    {
        public Transform waypoint;
        public float time;
    }

    public class PatrolState : BaseState
    {
        #region Serialized Variables
        [SerializeField] private float stopDistance = 0.25f;
        #endregion

        #region Private Variables
        private bool isDestinationReached;
        private float confirmPathWaitTime = 0.2f;
        #endregion

        #region Getters & Setters
        #endregion

        #region Unity Methods
        private void Awake()
        {
            state = StateEnum.PATROL;
        }
        #endregion

        #region Private Methods
        private void SetTargetAndMove()
        {
            aiController.StartMovement();
            aiController.SetPatrolTargetAndMove();
            //aiController.NavMeshAgent.SetDestination(_wayPointList[currentWaypointIndex % _wayPointList.Length].waypoint.position);
            isDestinationReached = false;
        }

        private void StopMovement()
        {
            isDestinationReached = true;
            aiController.StopMovement();
            stateMachine.SwitchToNextState(StateEnum.IDLE);
        }
        #endregion

        #region Public Methods
        public override void FixedTick()
        {

        }

        public override void StateEnter()
        {
            confirmPathWaitTime = 0.1f + Time.time;
            SetTargetAndMove();
        }

        public override void StateExit()
        {

        }

        public override void Tick()
        {
            if (aiController.Target != null)
            {
                stateMachine.SwitchToNextState(StateEnum.CHASE);
            }
            else
            {
                if (confirmPathWaitTime < Time.time)
                {
                    if (aiController.NavMeshAgent.remainingDistance <= stopDistance && isDestinationReached == false)
                    {
                        StopMovement();
                    }

                    aiController.SeekTarget();
                }
            }
        }

        #endregion
    }
}