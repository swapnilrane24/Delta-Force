using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Curio.Gameplay
{
    public class ChaseState : BaseState
    {
        #region Serialized Variables
        [Range(0f, 0.3f)]
        [SerializeField] private float increaseInSpeedByTimes = 0.15f;
        [Tooltip("Angular speed in radians per sec")]
        [SerializeField] private float rotSpeed = 5.0f;
        #endregion

        #region Private Variables

        #endregion

        #region Getters & Setters
        #endregion

        #region Unity Methods
        private void Awake()
        {
            state = StateEnum.CHASE;
        }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        public override void FixedTick()
        {

        }

        public override void StateEnter()
        {
            aiController.StartMovement();
            //aiController.NavMeshAgent.SetDestination(aiController.Target.ActorTransfrom.position);
        }

        public override void StateExit()
        {
            aiController.StopMovement();
        }

        float distanceBetweenTargetAndThisObject = 0;
        float waitForDestinationUpdate;

        public override void Tick()
        {
            if (aiController.Target != null)
            {
                if (waitForDestinationUpdate < Time.time)
                {
                    waitForDestinationUpdate = 0.2f + Time.time;
                    aiController.SetDestination(aiController.Target.ActorTransfrom.position);
                }

                distanceBetweenTargetAndThisObject = Vector3.Distance(transform.position, aiController.Target.ActorTransfrom.position);

                if (distanceBetweenTargetAndThisObject <= aiController.AttackDistance)
                {
                    stateMachine.SwitchToNextState(StateEnum.ATTACK);
                }
                else
                {
                    if (distanceBetweenTargetAndThisObject >= (aiController.ChaseRange + aiController.ChaseRange * 0.15f))
                    {
                        aiController.LostTarget();
                        stateMachine.SwitchToNextState(StateEnum.IDLE);
                    }
                }

            }
            else
            {
                stateMachine.SwitchToNextState(StateEnum.IDLE);
            }




        }
        #endregion


    }
}