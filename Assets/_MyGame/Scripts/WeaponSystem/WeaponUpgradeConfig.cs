using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    [System.Serializable]
    public class WeaponUpgradeConfig
    {
        [SerializeField] private int upgradeCost;

        [SerializeField] private bool availableByAds;

        [SerializeField] private int damageIncrease;

        [SerializeField] private float reloadingTimeReduce;

        [SerializeField] private float fireRateIncrement;

        [SerializeField] private int magazineSizeIncrease;

        public int UpgradeCost => upgradeCost;
        public bool AvailableByAds => availableByAds;
        public int DamageIncrease => damageIncrease;
        public float ReloadingTimeReduce => reloadingTimeReduce;
        public float FireRateIncrement => fireRateIncrement;
        public int MagazineSizeIncrease => magazineSizeIncrease;




    }
}