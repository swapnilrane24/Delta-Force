using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class WeaponSwitch : MonoBehaviour
    {
        [SerializeField] protected WeaponController[] weaponsList;

        protected int selectedGunIndex;

        public int SelectedGunIndex => selectedGunIndex;
        public int WeaponsCount => weaponsList.Length;
        public WeaponController SelectedGun => weaponsList[selectedGunIndex];
        public int RandomGunIndex => Random.Range(1, weaponsList.Length);

        protected virtual void OnDisable()
        {

        }

        protected virtual void Start()
        {
            for (int i = 0; i < weaponsList.Length; i++)
            {
                weaponsList[i].gameObject.SetActive(false);
            }


            //GameManager.Instance.onGameStartEvent.AddListener(() =>
            //{
                SelectedGun.gameObject.SetActive(true);
                //SelectedGun.InitializeWeapon();
            //});
            
        }

        public virtual void SwitchWeapon(int weaponID)
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
    }
}