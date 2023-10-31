using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class IdleState_BD : BaseState
    {
        [SerializeField] private float idleTime;

        private float currentIdleTime;

        private void Awake()
        {
            state = StateEnum.IDLE;
        }

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
                    aiController.GetBatteryTarget();
                }
            }
        }



    }
}