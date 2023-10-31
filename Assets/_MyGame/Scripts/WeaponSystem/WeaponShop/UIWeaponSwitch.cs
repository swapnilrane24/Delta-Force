using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Curio.Gameplay
{
    public class UIWeaponSwitch : WeaponSwitch
    {
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private GameObject menuVCam, weaponVCam;

        //public Transform WeaponHolder => weaponHolder;

        private Vector3 weaponHolderPosition;

        protected override void OnDisable()
        {
            GameManager.Instance.onLevelStartEvent.RemoveListener(() => gameObject.SetActive(false));
        }

        private void OnEnable()
        {
            GameManager.Instance.onLevelStartEvent.AddListener(() => gameObject.SetActive(false));
        }

        protected override void Start()
        {
            for (int i = 0; i < weaponsList.Length; i++)
            {
                weaponsList[i].gameObject.SetActive(false);
            }

            SelectedGun.gameObject.SetActive(true);

            weaponHolderPosition = weaponHolder.localPosition;
        }

        public override void SwitchWeapon(int weaponID)
        {
            SelectedGun.gameObject.SetActive(false);

            for (int i = 0; i < weaponsList.Length; i++)
            {
                if (weaponsList[i].WeaponConfig.WeaponID == weaponID)
                {
                    selectedGunIndex = i;
                    break;
                }
            }

            SelectedGun.gameObject.SetActive(true);
        }

        public void ActivateWeaponCam()
        {
            menuVCam.SetActive(false);
            weaponVCam.SetActive(true);
        }

        public void DeactivateWeaponCam()
        {
            menuVCam.SetActive(true);
            weaponVCam.SetActive(false);
        }


        public void Activate(bool canActivate)
        {
            gameObject.SetActive(canActivate);
        }

        public void MoveHolderLeft()
        {
            weaponHolder.localPosition = weaponHolderPosition + Vector3.right * -1.25f;
            weaponHolder.DOComplete();
            weaponHolder.DOLocalMoveX(weaponHolderPosition.x, 0.25f);
        }

        public void MoveHolderRight()
        {
            weaponHolder.localPosition = weaponHolderPosition + Vector3.right * 1.25f;
            weaponHolder.DOComplete();
            weaponHolder.DOLocalMoveX(weaponHolderPosition.x, 0.25f);
        }

    }
}