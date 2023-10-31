using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public class ActiveEvent : MonoBehaviour
    {
        public UnityEvent onActiveEvent;

        private void OnEnable()
        {
            onActiveEvent?.Invoke();
        }


    }
}