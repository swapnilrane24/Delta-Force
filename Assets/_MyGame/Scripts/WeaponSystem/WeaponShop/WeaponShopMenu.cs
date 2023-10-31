using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace Curio.Gameplay
{
    public class WeaponShopMenu : BaseUI
    {
        [SerializeField] private WeaponListData weaponData;
        [SerializeField] private UIWeaponSwitch uIWeaponSwitch;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI damageText, reloadText, ammoText;
        [SerializeField] private TextMeshProUGUI damageUpgradeAmountText;
        [SerializeField] private TextMeshProUGUI reloadUpgradeAmountText;
        [SerializeField] private TextMeshProUGUI ammoUpgradeAmountText;
        [SerializeField] private TextMeshProUGUI upgradeCostText;
        [SerializeField] private TextMeshProUGUI unlockCostText;
        [SerializeField] private GameObject lockIcon;

        [SerializeField] private Button nextWeaponButton;
        [SerializeField] private Button previousWeaponButton;
        [SerializeField] private Button adUnlockButton, selectButton, upgradeButton;

        [SerializeField] private Image[] gunLevelstars;

        private int currentWeaponIndex;
        private int minIndex = 0;
        private int maxIndex = 0;

        private void OnDisable()
        {
            RemoveListners();
        }

        private void RemoveListners()
        {
            upgradeButton.onClick.RemoveListener(UpgradeButtonListner);
            adUnlockButton.onClick.RemoveListener(AdUnlockListner);
            selectButton.onClick.RemoveListener(SelectButtonListner);
            nextWeaponButton.onClick.RemoveListener(NextWeaponButtonListner);
            previousWeaponButton.onClick.RemoveListener(PreviousWeaponButtonListner);
            GameManager.Instance.TotalMoney.RemoveListener(() => CanUpgrade());
        }

        protected override void Start()
        {
            base.Start();
            //uIWeaponSwitch.WeaponHolder.gameObject.SetActive(false);
            for (int i = 0; i < weaponData.WeaponsList.Length; i++)
            {
                weaponData.WeaponsList[i].LoadWeaponStats();
            }
            weaponData.LoadSelectedWeaponIndex();

            upgradeButton.onClick.AddListener(UpgradeButtonListner);
            adUnlockButton.onClick.AddListener(AdUnlockListner);
            selectButton.onClick.AddListener(SelectButtonListner);
            nextWeaponButton.onClick.AddListener(NextWeaponButtonListner);
            previousWeaponButton.onClick.AddListener(PreviousWeaponButtonListner);
        }

        private void InitializeShop()
        {
            maxIndex = weaponData.WeaponsList.Length;

            currentWeaponIndex = 0;
            bool isSelected = weaponData.SelectedWeaponIndex == currentWeaponIndex;
            SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], isSelected);
            //Debug.Log(weaponData.SelectedWeaponIndex);
        }

        public override void ActivatePanel()
        {
            uIWeaponSwitch.ActivateWeaponCam();//.WeaponHolder.gameObject.SetActive(true);
            //uIWeaponSwitch.CharacterObject.gameObject.SetActive(false);
            InitializeShop();
            //uIWeaponSwitch.Activate(true);
            GameManager.Instance.TotalMoney.AddListener(() => CanUpgrade());
            togglePanel.ToggleVisibility(true);
        }

        public override void DeactivatePanel()
        {
            uIWeaponSwitch.DeactivateWeaponCam();//.WeaponHolder.gameObject.SetActive(false);
            //uIWeaponSwitch.CharacterObject.gameObject.SetActive(true);
            //uIWeaponSwitch.Activate(false);
            GameManager.Instance.TotalMoney.RemoveListener(() => CanUpgrade());
            togglePanel.ToggleVisibility(false);
        }

        private void NextWeaponButtonListner()
        {
            if (currentWeaponIndex < maxIndex - 1)
            {
                currentWeaponIndex++;

                uIWeaponSwitch.MoveHolderRight();
            }

            bool isSelected = weaponData.SelectedWeaponIndex == currentWeaponIndex;
            SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], isSelected);

            if (currentWeaponIndex >= maxIndex)
            {
                nextWeaponButton.interactable = false;
            }

            if (currentWeaponIndex > 0)
            {
                previousWeaponButton.interactable = true;
            }
        }

        private void PreviousWeaponButtonListner()
        {
            if (currentWeaponIndex > 0)
            {
                currentWeaponIndex--;

                uIWeaponSwitch.MoveHolderLeft();
            }

            bool isSelected = weaponData.SelectedWeaponIndex == currentWeaponIndex;
            SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], isSelected);

            if (currentWeaponIndex <= 0)
            {
                previousWeaponButton.interactable = false;
            }

            if (currentWeaponIndex < maxIndex)
            {
                nextWeaponButton.interactable = true;
            }
        }

        private void AdUnlockListner()
        {
            if (weaponData.WeaponsList[currentWeaponIndex].Unlocked)
            {
                SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], true);
                selectButton.interactable = false;
                weaponData.SetSelectedWeaponIndex(currentWeaponIndex);
            }
            else
            {
                ////Show Ads
                //GameAdsManager.Instance.ShowRewardedAds((bool adsSuccess) =>
                //{
                //    if (adsSuccess)
                //    {
                //        weaponData.SetSelectedWeaponIndex(currentWeaponIndex);

                //        weaponData.WeaponsList[currentWeaponIndex].SaveWeaponUnlocked();

                //        SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], true);

                //        adUnlockButton.gameObject.SetActive(false);
                //        selectButton.gameObject.SetActive(true);
                //        selectButton.interactable = false;
                //    }
                //});

                GameManager.Instance.RemoveMoney(weaponData.WeaponsList[currentWeaponIndex].UnlockCost);
                weaponData.SetSelectedWeaponIndex(currentWeaponIndex);
                weaponData.WeaponsList[currentWeaponIndex].SaveWeaponUnlocked();
                SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], true);
                selectButton.gameObject.SetActive(true);
                selectButton.interactable = false;
            }
        }

        private void SelectButtonListner()
        {
            selectButton.interactable = false;
            weaponData.SetSelectedWeaponIndex(currentWeaponIndex);
            SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], true);
        }

        private void UpgradeButtonListner()
        {
            int nextUpgradeCount = weaponData.WeaponsList[currentWeaponIndex].UpgradeCount + 1;
            int upgradeCost = weaponData.WeaponsList[currentWeaponIndex].WeaponUpgrades[nextUpgradeCount].UpgradeCost;

            GameManager.Instance.RemoveMoney(upgradeCost);

            weaponData.WeaponsList[currentWeaponIndex].SaveUpgradeCount(nextUpgradeCount);

            bool isSelected = currentWeaponIndex == weaponData.SelectedWeaponIndex;
            SetWeaponData(weaponData.WeaponsList[currentWeaponIndex], isSelected);
        }

        private void SetWeaponData(WeaponConfig weaponData, bool isSelected)
        {
            weaponNameText.text = weaponData.WeaponName;
            uIWeaponSwitch.SwitchWeapon(weaponData.WeaponID);

            reloadUpgradeAmountText.text = "";
            ammoUpgradeAmountText.text = "";
            damageUpgradeAmountText.text = "";

            if (weaponData.AvailableToUnlock)
            {
                lockIcon.SetActive(false);

                damageText.text = "" + weaponData.Damage;

                reloadText.text = "" + weaponData.ReloadTime.ToString("F2");

                //if (weaponData.EnableAmmo)
                ammoText.text = "" + weaponData.MagazineSize;
                //else
                //ammoText.text = "-";

                if (weaponData.Unlocked)
                {
                    adUnlockButton.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                    selectButton.interactable = !isSelected;
                }
                else
                {
                    //TODO
                    //bool internetAvailable = true;
                    unlockCostText.text = CurrencyToString.Convert(weaponData.UnlockCost);
                    adUnlockButton.interactable = GameManager.Instance.TotalMoney >= weaponData.UnlockCost;
                    adUnlockButton.gameObject.SetActive(true);
                    //adUnlockButton.interactable = internetAvailable;

                    selectButton.gameObject.SetActive(false);
                }
                uIWeaponSwitch.SelectedGun.GunUnknown(false);
            }
            else
            {
                lockIcon.SetActive(true);
                weaponNameText.text = "UNKNOWN";
                reloadText.text = "....";
                ammoText.text = "....";
                damageText.text = "....";
                adUnlockButton.gameObject.SetActive(true);
                adUnlockButton.interactable = false;
                selectButton.gameObject.SetActive(false);
                uIWeaponSwitch.SelectedGun.GunUnknown(true);
            }

            if (weaponData.Unlocked)
                CanUpgrade();
            else
            {
                upgradeButton.interactable = false;
                upgradeCostText.text = "LOCKED";
            }
        }

        private void CanUpgrade()
        {
            int nextWeaponLevel = 0;

            //upgradeAdsButton.gameObject.SetActive(false);
            upgradeButton.gameObject.SetActive(true);

            WeaponConfig weaponConfig = weaponData.WeaponsList[currentWeaponIndex];

            nextWeaponLevel = weaponConfig.UpgradeCount + 1;

            reloadUpgradeAmountText.text = "";
            ammoUpgradeAmountText.text = "";
            damageUpgradeAmountText.text = "";

            if (nextWeaponLevel < weaponConfig.WeaponUpgrades.Length)
            {
                if (GameManager.Instance.TotalMoney >= weaponConfig.
                    WeaponUpgrades[nextWeaponLevel].UpgradeCost)
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }

                upgradeCostText.text = CurrencyToString.Convert(weaponConfig
                       .WeaponUpgrades[nextWeaponLevel].UpgradeCost);

                if (weaponConfig.WeaponUpgrades[nextWeaponLevel].DamageIncrease > 0)
                {
                    damageUpgradeAmountText.text = "+" + weaponConfig.
                    WeaponUpgrades[nextWeaponLevel].DamageIncrease;
                }
                else
                {
                    damageUpgradeAmountText.text = "";
                }

                if (weaponConfig.WeaponUpgrades[nextWeaponLevel].ReloadingTimeReduce > 0)
                    reloadUpgradeAmountText.text = "-" + weaponConfig.WeaponUpgrades[nextWeaponLevel].ReloadingTimeReduce.ToString("F2");

                if (weaponConfig.WeaponUpgrades[nextWeaponLevel].MagazineSizeIncrease > 0)
                {
                    ammoUpgradeAmountText.text = "+" + weaponConfig.WeaponUpgrades[nextWeaponLevel].MagazineSizeIncrease;
                }
                else
                {
                    ammoUpgradeAmountText.text = "";
                }
            }
            else
            {
                upgradeButton.interactable = false;
                upgradeCostText.text = "MAX";

                //reloadUpgradeAmountText.text = "";
                //ammoUpgradeAmountText.text = "";
                //damageUpgradeAmountText.text = "";
            }

        }

        protected override void BackButton()
        {
            base.BackButton();
            menuRoot.GetMenu(MenuEnum.MainMenu).ActivatePanel();
        }


    }
}