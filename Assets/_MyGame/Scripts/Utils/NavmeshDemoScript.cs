using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Curio.Gameplay
{
    public class NavmeshDemoScript : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] protected NavMeshAgent navMeshAgent;

        private void Start()
        {
            if (navMeshAgent == null)
            {
                navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (target)
            {
                navMeshAgent.SetDestination(target.position);
            }
        }
    }
}