using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class PickUp : MonoBehaviour
    {
        [SerializeField] private PickUpType initialPickUpType;
        [SerializeField] private PickUpAction[] pickUpActions;

        private PickUpAction currectActivePickupAction;

        private ObjectPoolGeneric<PickUp> _objectPoolGeneric;

        [SerializeField] private int weaponDropIndex;
        public int WeaponDropIndex { set => weaponDropIndex = value; get => weaponDropIndex; }

        bool isPickedUp;

        private void OnDisable()
        {
            for (int i = 0; i < pickUpActions.Length; i++)
            {
                pickUpActions[i].gameObject.SetActive(false);
                if (isPickedUp == false)
                {
                    ReturnItem();
                }
            }
        }

        private void OnEnable()
        {
            isPickedUp = false;
            if (initialPickUpType)
            {
                InitializePickUp(initialPickUpType);
            }
        }

        public void InitializePickUp(PickUpType pickUpType)
        {
            for (int i = 0; i < pickUpActions.Length; i++)
            {
                pickUpActions[i].PickUp = this;
                if (pickUpActions[i].PickUpType == pickUpType)
                {
                    currectActivePickupAction = pickUpActions[i];
                    break;
                }
            }

            isPickedUp = true;
            currectActivePickupAction.gameObject.SetActive(true);
            currectActivePickupAction.onPickUpEvent.AddListener(ReturnItem);
            gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Actor>(out Actor actor))
            {
                currectActivePickupAction.TakeAction(actor); 
            }
        }

        public void SetObjectPoolGeneric(ObjectPoolGeneric<PickUp> objectPoolGeneric)
        {
            _objectPoolGeneric = objectPoolGeneric;
        }

        public void ReturnItem()
        {
            if (currectActivePickupAction)
                currectActivePickupAction.onPickUpEvent.RemoveListener(ReturnItem);

            if (_objectPoolGeneric != null)
                _objectPoolGeneric.ReturnObject(this);

            gameObject.SetActive(false);
        }

    }
}