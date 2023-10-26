using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Curio.Gameplay
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool spawnOnActive, spawnOnDeactive;
        [SerializeField] private ParticleType particleType;

        bool applicationCLosed = false;

        private void OnEnable()
        {
            if (spawnOnActive)
                PlayParticle();
        }

        private void OnDisable()
        {
            if (spawnOnDeactive && applicationCLosed == false)
                PlayParticle();
        }

        private void OnApplicationQuit()
        {
            applicationCLosed = true;
        }

        public void PlayParticle()
        {
            MegaParticle particle = ParticleManager.Instance.GetParticle(particleType);
            particle.ActivateParticle(particleType);
            particle.transform.position = transform.position + offset;
        }

        public void PlayParticle(Vector3 spawnPos)
        {
            MegaParticle particle = ParticleManager.Instance.GetParticle(particleType);
            particle.ActivateParticle(particleType);
            particle.transform.position = spawnPos;
        }

        public void PlayParticle(ParticleType particleTypeVal, Vector3 spawnPos)
        {
            MegaParticle particle = ParticleManager.Instance.GetParticle(particleTypeVal);
            particle.ActivateParticle(particleTypeVal);
            particle.transform.position = spawnPos;
        }

    }
}