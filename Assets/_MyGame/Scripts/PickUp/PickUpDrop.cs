using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

namespace Curio.Gameplay
{
    public class PickUpDrop : MonoBehaviour
    {
        [System.Serializable]
        private struct DropOptions
        {
            public float weight;
            public PickUpType pickUpType;
        }

        [SerializeField] private PickUpType weaponTypePickUp;
        [SerializeField] private bool canAnimateDrop = true;
        [Range(0,100)]
        [SerializeField] private int canDropProbabilityPercent;
        //[SerializeField] private bool dropOnDeactive;
        [SerializeField] private DropOptions[] dropablePickUps;

        private float weightSum;
        private bool canDrop;
        private int weaponIndexOfEnemy;

        private void OnEnable()
        {
            //we are take range from 0-100 as percent
            canDrop = Random.Range(0, 101) <= canDropProbabilityPercent ? true : false;
        }

        private void Start()
        {
            weightSum = 0;
            for (int i = 0; i < dropablePickUps.Length; i++)
            {
                weightSum += dropablePickUps[i].weight;
            }
        }

        float jumpDuration = 0.5f;
        float jumpDistance = 2.5f;

        public void PickUpWeaponIndex(int index)
        {
            weaponIndexOfEnemy = index;
        }

        public void Drop()
        {
            if (canDrop == false) return;

            PickUp pickUp = null;
            float randomWeight = Random.Range(0, weightSum);
            for (int i = 0; i < dropablePickUps.Length; i++)
            {
                randomWeight -= dropablePickUps[i].weight;
                if (randomWeight <= 0)
                {
                    pickUp = PickUpManager.Instance.GetPickUp();
                    pickUp.WeaponDropIndex = weaponIndexOfEnemy;
                    pickUp.InitializePickUp(dropablePickUps[i].pickUpType);
                    break;
                }
            }

            pickUp.transform.position = transform.position;

            if (canAnimateDrop)
            {
                Vector3 jumpPosition = transform.position + Random.insideUnitSphere * jumpDistance;
                jumpPosition.y = transform.position.y;
                pickUp.transform.DOJump(jumpPosition, 2, 1, jumpDuration);
            }
        }

        public void Drop(Vector3 position)
        {
            if (canDrop == false) return;
            PickUp pickUp = null;
            float randomWeight = Random.Range(0, weightSum);
            for (int i = 0; i < dropablePickUps.Length; i++)
            {
                randomWeight -= dropablePickUps[i].weight;
                if (randomWeight <= 0)
                {
                    pickUp = PickUpManager.Instance.GetPickUp();
                    pickUp.WeaponDropIndex = weaponIndexOfEnemy;
                    pickUp.InitializePickUp(dropablePickUps[i].pickUpType);
                    break;
                }
            }

            pickUp.transform.position = position;

            if (canAnimateDrop)
            {
                Vector3 jumpPosition = position + Random.insideUnitSphere * jumpDistance;
                jumpPosition.y = position.y;
                pickUp.transform.DOJump(jumpPosition, 2, 1, jumpDuration);
            }
        }

        public void Drop(PickUpType pickUpType)
        {
            if (canDrop == false) return;

            PickUp pickUp = PickUpManager.Instance.GetPickUp();
            pickUp.WeaponDropIndex = weaponIndexOfEnemy;
            pickUp.InitializePickUp(pickUpType);
            pickUp.transform.position = transform.position;

            if (canAnimateDrop)
            {
                Vector3 jumpPosition = transform.position + Random.insideUnitSphere * jumpDistance;
                jumpPosition.y = transform.position.y;
                pickUp.transform.DOJump(jumpPosition, 2, 1, jumpDuration);
            }
        }

        public void Drop(PickUpType pickUpType, Vector3 position)
        {
            if (canDrop == false) return;

            PickUp pickUp = PickUpManager.Instance.GetPickUp();
            pickUp.WeaponDropIndex = weaponIndexOfEnemy;
            pickUp.InitializePickUp(pickUpType);
            pickUp.transform.position = position;

            if (canAnimateDrop)
            {
                Vector3 jumpPosition = position + Random.insideUnitSphere * jumpDistance;
                jumpPosition.y = position.y;
                pickUp.transform.DOJump(jumpPosition, 2, 1, jumpDuration);
            }
        }


    }
}