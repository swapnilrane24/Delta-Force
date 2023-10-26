using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using UnityEngine;

namespace Curio.Gameplay
{
    public class PickUpManager : MonoBehaviour
    {
        private static PickUpManager instance;
        public static PickUpManager Instance => instance;

        [SerializeField] private DamageNumber damageNumber;
        [SerializeField] private PickUp pickUpPrefab;

        private ObjectPoolGeneric<PickUp> pickUpPool;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            pickUpPool = new ObjectPoolGeneric<PickUp>(SpawnPickUp, ActivatePickUp, DeactivatePickUp, 1);
        }

        public DamageNumber GetDamageNumber()
        {
            return damageNumber;
        }

        public PickUp GetPickUp()
        {
            return pickUpPool.GetObject();
        }

        private PickUp SpawnPickUp()
        {
            PickUp pickUp = Instantiate(pickUpPrefab, transform);
            pickUp.SetObjectPoolGeneric(pickUpPool);
            return pickUp;
        }

        private void ActivatePickUp(PickUp pickUp)
        {
            pickUp.gameObject.SetActive(true);
        }

        private void DeactivatePickUp(PickUp pickUp)
        {
            pickUp.gameObject.SetActive(false);
        }

    }
}