using System;
using UnityEngine;

namespace Curio.Gameplay
{
    public abstract class BaseState : MonoBehaviour
    {
        protected StateEnum state;
        protected Enemy aiController;
        protected StateMachine stateMachine;

        public StateEnum State { get => state; }
        public Enemy Controller { set => aiController = value; }
        public StateMachine StateMachine { set => stateMachine = value; }

        public abstract void Tick();
        public abstract void FixedTick();

        public abstract void StateEnter();
        public abstract void StateExit();
    }
}