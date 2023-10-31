using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Curio/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        //WeaponFireType
        [SerializeField] private WeaponFireType weaponFireType = WeaponFireType.PROJECTILE;
        [SerializeField] private Sprite weaponIcon;
        [SerializeField] private bool unlockedByDefault;
        [SerializeField] private int weaponId; //Used only by player
        [SerializeField] private string weaponName;
        [SerializeField] private Projectile projectile;
        [SerializeField] private float projectileSpeed;

        // Range
        public float gunRange = 9999.0f;	// How far this weapon can shoot (for raycast and beam)
        [Range(10, 100)]
        public float gunAccuracy;
        [SerializeField] private int bulletsPerShot = 1;
        [SerializeField] private bool isInfiniteAmmo;
        [SerializeField] private int magazineSize;
        [Tooltip("If firing multiple bullets per shot, should only a single bullet be removed from the inventory.")]
        [SerializeField] private bool consumeSingleBulletPerShot = true;

        [SerializeField] private float reloadTime;
        [SerializeField] private bool autoReload = true;
        [SerializeField] private AnimatorOverrideController weaponAnimController;
        [Tooltip("Bullets per seconds")]
        [SerializeField] private float fireRate;
        [SerializeField] private int damage;

        [SerializeField] private AudioClip[] fireSfx;
        [SerializeField] private AudioClip magazineOutFx, magazineInFx;

        [SerializeField] private int unlockCost;
        [SerializeField] private WeaponUpgradeConfig[] weaponUpgrades;

        private int upgradeCount = 0;//since the 1st element on Upgade data array is zero
        private bool unlocked;
        private bool availableToUnlock = true;

        public WeaponFireType WeaponFireType => weaponFireType;
        public Sprite WeaponIcon => weaponIcon;
        public int    WeaponID => weaponId;
        public string WeaponName => weaponName;
        public int    BulletsPerShot => bulletsPerShot;
        public bool   IsInfiniteAmmo => isInfiniteAmmo;
        public bool   ConsumeSingleBulletPerShot => consumeSingleBulletPerShot;
        public int    MagazineSize => currentMagazineSize;
        public float  ReloadTime => currentReloadTime;
        public float  FireRate => currentFireRate;
        public bool   AutoReload => autoReload;
        public int    Damage => currentDamage;
        public int    UpgradeCount => upgradeCount;
        public int    UnlockCost => unlockCost;
        public bool   Unlocked => unlocked;
        public bool   AvailableToUnlock => availableToUnlock;
        public Projectile Projectile => projectile;
        public float  ProjectileSpeed => projectileSpeed;
        public AnimatorOverrideController WeaponAnimController => weaponAnimController;
        public WeaponUpgradeConfig[] WeaponUpgrades => weaponUpgrades;


        public AudioClip[] FireSfx => fireSfx;
        public AudioClip MagazineOutFx => magazineOutFx;
        public AudioClip MagazineInFx => magazineInFx;

        private int currentDamage;
        private float currentReloadTime;
        private int currentMagazineSize;
        private float currentFireRate;


        public void SaveWeaponUnlocked()
        {
            string saveName = "Unlock_" + weaponName;
            unlocked = true;
            ES3.Save<bool>(saveName, true);
        }

        public void LoadWeaponUnlocked()
        {
            string saveName = "Unlock_" + weaponName;
            unlocked = ES3.Load<bool>(saveName, unlockedByDefault);
            if (unlockedByDefault) availableToUnlock = true;
        }

        public void SaveWeaponAvailableToUnlock()
        {
            //string saveName = "AvailableToUnlock_" + weaponName;
            //availableToUnlock = true;
            //ES3.Save<bool>(saveName, true);
        }

        public void LoadWeaponAvailableToUnlock()
        {
            //string saveName = "AvailableToUnlock_" + weaponName;
            //availableToUnlock = ES3.Load<bool>(saveName, false);
        }

        public void SaveUpgradeCount(int count)
        {
            string saveName = "Upgrade_" + weaponName;
            upgradeCount = count;
            ES3.Save<int>(saveName, upgradeCount);

            LoadWeaponStats();
        }

        public void LoadUpgradeCount()
        {
            string saveName = "Upgrade_" + weaponName;

            upgradeCount = ES3.Load<int>(saveName, 0);
        }

        public void LoadWeaponStats()
        {
            LoadUpgradeCount();
            LoadWeaponAvailableToUnlock();
            LoadWeaponUnlocked();

            int damageIncrement = 0;
            float reloadTimeReduction = 0;
            float fireRateIncrement = 0;
            int ammoIncrement = 0;

            for (int i = 0; i <= upgradeCount; i++)
            {
                if (i < weaponUpgrades.Length)
                {
                    damageIncrement += weaponUpgrades[i].DamageIncrease;
                    reloadTimeReduction += weaponUpgrades[i].ReloadingTimeReduce;
                    fireRateIncrement += weaponUpgrades[i].FireRateIncrement;
                    ammoIncrement += weaponUpgrades[i].MagazineSizeIncrease;
                }
            }

            currentDamage = damage + damageIncrement;
            currentReloadTime = reloadTime - reloadTimeReduction;
            currentFireRate = fireRate + fireRateIncrement;
            currentMagazineSize = magazineSize + ammoIncrement;

            if (reloadTime <= 0) reloadTime = 0.1f;
        }

        public void Reset()
        {
            if (unlockedByDefault == false)
            {
                string unlockSaveName = "Unlock_" + weaponName;
                unlocked = false;
                ES3.Save<bool>(unlockSaveName, false);
            }

            string upgradeSaveName = "Upgrade_" + weaponName;
            upgradeCount = 0;
            ES3.Save<int>(upgradeSaveName, 0);

            LoadWeaponStats();
        }

    }
}