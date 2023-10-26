using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public class PickUpAction : MonoBehaviour
    {
        [SerializeField] protected PickUpType pickUpType;

        public PickUpType PickUpType => pickUpType;
        public UnityEvent onPickUpEvent;

        protected PickUp pickUp;
        public PickUp PickUp { set => pickUp = value; }

        public virtual void TakeAction(Actor actor)
        {

        }
    }
}