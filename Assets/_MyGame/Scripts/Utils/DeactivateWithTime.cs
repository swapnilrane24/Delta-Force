using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public class DeactivateWithTime : MonoBehaviour
    {
        [SerializeField] private float aliveTime;
        [SerializeField] private UnityEvent onDeactivateEvent;

        private float currentTime;
        private void OnEnable()
        {
            currentTime = aliveTime + Time.time;
        }

        private void Update()
        {
            if (currentTime < Time.time)
            {
                onDeactivateEvent?.Invoke();
                gameObject.SetActive(false);
            }
        }


    }
}