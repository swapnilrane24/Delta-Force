using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    [CreateAssetMenu(fileName = "WeaponListData", menuName = "Curio/WeaponListData")]
    public class WeaponListData : ScriptableObject
    {
        [SerializeField] private WeaponConfig[] weaponsList;

        private int selectedWeaponIndex = 0;
        private string saveName = "Selected_Weapon";
        private int totalWeaponsAvailableToUnlock = 0;

        public WeaponConfig[] WeaponsList => weaponsList;
        public int SelectedWeaponIndex => selectedWeaponIndex;
        public int SelectedWeaponID => weaponsList[selectedWeaponIndex].WeaponID;
        public int TotalWeaponsAvailableToUnlock => totalWeaponsAvailableToUnlock;

        public void SetSelectedWeaponIndex(int weaponIndex)
        {
            selectedWeaponIndex = weaponIndex;
            ES3.Save<int>(saveName, selectedWeaponIndex);
            
        }

        public void LoadSelectedWeaponIndex()
        {
            selectedWeaponIndex = ES3.Load<int>(saveName, 0);
            LoadTotalWeaponsAvailableToUnlock();
        }

        public void SaveTotalWeaponAvailableToUnlock()
        {
            totalWeaponsAvailableToUnlock++;
            ES3.Save<int>("TotalWeaponsAvailable", totalWeaponsAvailableToUnlock);
        }

        public void LoadTotalWeaponsAvailableToUnlock()
        {
            totalWeaponsAvailableToUnlock = ES3.Load<int>("TotalWeaponsAvailable", 0);
        }

        public WeaponConfig GetWeaponDataByIndex(int weaponIndex)
        {
            WeaponConfig weaponConfig = null;
            for (int i = 0; i < weaponsList.Length; i++)
            {
                if (weaponsList[i].WeaponID == weaponIndex)
                {
                    weaponConfig = weaponsList[i];
                    break;
                }
            }

            return weaponConfig;
        }
    }
}