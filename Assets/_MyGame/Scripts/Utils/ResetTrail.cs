using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class ResetTrail : MonoBehaviour
    {
        private TrailRenderer trailRenderer;

        private void Awake()
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnDisable()
        {
            if (trailRenderer) trailRenderer.Clear();
        }


    }
}