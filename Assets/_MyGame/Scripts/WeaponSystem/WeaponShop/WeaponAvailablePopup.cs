using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class WeaponAvailablePopup : MonoBehaviour
    {
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private Image weaponIcon;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI damageText, reloadText, ammoText;
        [SerializeField] private Button adUnlockButton, noThanksButton;

        private WeaponConfig _weaponData;

        private void Start()
        {
            adUnlockButton.onClick.AddListener(AdButtonListner);
            noThanksButton.onClick.AddListener(NoThanksButtonListner);
        }

        public void InitializePopUp(WeaponConfig weaponData)
        {
            _weaponData = weaponData;
            _weaponData.SaveWeaponAvailableToUnlock();
            togglePanel.ToggleVisibility(true);

            weaponIcon.sprite = weaponData.WeaponIcon;

            weaponNameText.text = weaponData.WeaponName;

            damageText.text = "" + weaponData.Damage;

            reloadText.text = "" + weaponData.ReloadTime;

            //if (weaponData.EnableAmmo)
            ammoText.text = "" + weaponData.MagazineSize;
            //else
                //ammoText.text = "-";


        }

        private void AdButtonListner()
        {
            //Show ads
            GameAdsManager.Instance.ShowRewardedAds((bool adsSuccess) =>
            {
                if (adsSuccess)
                {
                    adUnlockButton.interactable = false;
                    noThanksButton.interactable = false;

                    _weaponData.SaveWeaponUnlocked();

                }

                WaitExtension.Wait(this, 0.5f, () => togglePanel.ToggleVisibility(false));
            });

        }

        private void NoThanksButtonListner()
        {
            togglePanel.ToggleVisibility(false);
        }


    }
}