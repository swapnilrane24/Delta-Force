using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class ParticleManager : MonoBehaviour
    {
        private static ParticleManager instance;

        public static ParticleManager Instance => instance;

        [SerializeField] private MegaParticle megaParticlePrefab;

        private ObjectPoolGeneric<MegaParticle> megaParticlePool;

        bool managerDisabled;

        private void OnDisable()
        {
            managerDisabled = true;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            megaParticlePool = new ObjectPoolGeneric<MegaParticle>(SpawnProjectile,
                    ActivateParticle, ReturnParticle, 1);
        }

        public MegaParticle GetParticle(ParticleType particleType)
        {
            if (managerDisabled == true) return null;

            // MegaParticle megaParticle = SpawnProjectile();
            MegaParticle megaParticle = megaParticlePool.GetObject();
            megaParticle.ActivateParticle(particleType);

            return megaParticle;
        }

        private MegaParticle SpawnProjectile()
        {
            MegaParticle particle = Instantiate(megaParticlePrefab, transform);
            particle.SetObjectPoolGeneric(megaParticlePool);
            return particle;
        }

        private void ActivateParticle(MegaParticle megaParticle)
        {
            megaParticle.gameObject.SetActive(true);
        }

        private void ReturnParticle(MegaParticle megaParticle)
        {
            megaParticle.gameObject.SetActive(false);
        }


    }
}