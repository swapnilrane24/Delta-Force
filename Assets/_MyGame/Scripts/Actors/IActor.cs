using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public interface IActor
    {
        int TeamID { get; }
        void Damage(int value);
        UnityEvent OnDeadEvent { get; }
        Transform ActorTransfrom { get; }

    }
}