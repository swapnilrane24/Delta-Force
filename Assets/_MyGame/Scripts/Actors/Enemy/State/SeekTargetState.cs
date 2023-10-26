using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Curio.Gameplay
{
    public class SeekTargetState : BaseState
    {
        [SerializeField] private float stopDistance = 0.25f;
        private float confirmPathWaitTime = 0.2f;
        private void Awake()
        {
            state = StateEnum.SEEK;
        }

        public override void FixedTick()
        {
            
        }

        public override void StateEnter()
        {
            aiController.IsSeekingTarget = true;
            confirmPathWaitTime = 0.1f + Time.time;
            
            aiController.StartMovement();
            aiController.NavMeshAgent.SetDestination(aiController.SeekDestination);
        }

        public override void StateExit()
        {
            aiController.IsSeekingTarget = false;
            aiController.StopMovement();
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
                    if (aiController.NavMeshAgent.remainingDistance <= stopDistance)
                    {
                        //aiController.StopMovement();
                        stateMachine.SwitchToNextState(StateEnum.IDLE);
                    }
                }
            }
        }
    }
}