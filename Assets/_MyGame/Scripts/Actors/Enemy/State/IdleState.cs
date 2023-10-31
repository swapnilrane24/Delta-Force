using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class IdleState : BaseState
    {
        #region Serialized Variables
        [SerializeField] private float idleTime;
        #endregion

        #region Private Variables
        private float currentIdleTime;
        #endregion

        #region Getters & Setters
        #endregion

        #region Unity Methods
        private void Awake()
        {
            state = StateEnum.IDLE;
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
            currentIdleTime = idleTime;

            aiController.StopMovement();
        }

        public override void StateExit()
        {
            currentIdleTime = idleTime;
        }

        public override void Tick()
        {
            if (currentIdleTime > 0)
            {
                currentIdleTime -= Time.deltaTime;
            }
            else
            {
                if (aiController.Target != null)
                {
                    stateMachine.SwitchToNextState(StateEnum.CHASE);
                }
                else
                {
                    aiController.SeekTarget();

                    if (aiController.IsSeekingTarget == false)
                        stateMachine.SwitchToNextState(StateEnum.PATROL);
                }
            }
        }
        #endregion
    }
}