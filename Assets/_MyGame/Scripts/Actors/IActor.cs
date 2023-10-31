using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public abstract class IActor : MonoBehaviour
    {
        public virtual int TeamID { get; }
        public virtual void Damage(ProjectileData projectileData) { }
        public virtual void Damage(int value) { }
        public virtual UnityEvent<IActor> OnDeadEvent { get; }
        public virtual Transform ActorTransfrom { get; }
        public virtual bool IsAlive { get; }
        public virtual bool IsPlayer { get; }
        public virtual string ActorName { get; }

    }
}