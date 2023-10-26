using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class MegaParticle : MonoBehaviour
    {
        [System.Serializable]
        protected class ParticleData
        {
            public ParticleType particleType;
            public ParticleSystem particleSystem;
            public float deactivateTimer = 2f;
        }

        [SerializeField] private ParticleData[] particleList;

        private float currentDeactivateTimer;
        private ParticleSystem currentActiveParticle;

        private ObjectPoolGeneric<MegaParticle> _objectPoolGeneric;

        public void SetObjectPoolGeneric(ObjectPoolGeneric<MegaParticle> objectPoolGeneric)
        {
            _objectPoolGeneric = objectPoolGeneric;
        }

        public void ActivateParticle(ParticleType particleType)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < particleList.Length; i++)
            {
                if (particleList[i].particleType == particleType)
                {
                    particleList[i].particleSystem.gameObject.SetActive(true);
                    currentActiveParticle = particleList[i].particleSystem;
                    particleList[i].particleSystem.Stop();
                    particleList[i].particleSystem.Play();
                    currentDeactivateTimer = Time.time + particleList[i].deactivateTimer;
                    break;
                }
            }
        }

        private void Update()
        {
            if (currentDeactivateTimer <= Time.time && currentActiveParticle)
            {
                currentActiveParticle.gameObject.SetActive(false);
                currentActiveParticle = null;
                ReturnItem();
            }
        }

        public void ReturnItem()
        {
            if (_objectPoolGeneric != null)
            {
                _objectPoolGeneric.ReturnObject(this);
            }
        }

    }
}