using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class PickUpGun : PickUpAction
    {
        [System.Serializable]
        private struct WeaponPickInfo
        {
            public GameObject weaponVisual;
            public string weaponame;
            public int weaponIndex;
        }

        [SerializeField] private WeaponPickInfo[] weaponPickInfos;

        //private int weaponIndex;

        private void OnEnable()
        {
            for (int i = 0; i < weaponPickInfos.Length; i++)
            {
                weaponPickInfos[i].weaponVisual.SetActive(false);
            }

            weaponPickInfos[pickUp.WeaponDropIndex].weaponVisual.SetActive(true);
        }

        public override void TakeAction(Actor actor)
        {
            if (actor.IsPlayer)
            {
                //Call Popup Text
                onPickUpEvent?.Invoke();
                Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
                //PickUpManager.Instance.GetDamageNumber().leftText = "";
                //PickUpManager.Instance.GetDamageNumber().rightText = "";
                //PickUpManager.Instance.GetDamageNumber().Spawn(spawnPos, damageIncreasePercent);
                actor.CollecteWeapon(pickUp.WeaponDropIndex);
            }
        }
    }
}