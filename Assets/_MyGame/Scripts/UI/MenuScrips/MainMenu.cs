using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Curio.Gameplay
{
    public class MainMenu : BaseUI
    {
        [SerializeField] private Button baseDefenseButton;
        [SerializeField] private Button deathMatchButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private TextMeshProUGUI baseLevelText;


        protected override void Start()
        {
            base.Start();
            baseDefenseButton.onClick.AddListener(DefenseButtonListner);
            deathMatchButton.onClick.AddListener(DeathMatchButtonListner);
            storeButton.onClick.AddListener(StoreButtonListner);
            settingsButton.onClick.AddListener(SettingButtonListner);
            baseLevelText.text = "Level " + (BaseDefenseManager.Instance.BaseDefenseLevel + 1);
        }

        private void DefenseButtonListner()
        {
            BaseDefenseManager.Instance.InitializeBaseDefenceManager();
            menuRoot.GetMenu(MenuEnum.GameUI).ActivatePanel();
            menuRoot.TotalCurrencyUI.gameObject.SetActive(false);
            DeactivatePanel();
        }

        private void DeathMatchButtonListner()
        {
            DeactivatePanel();
            menuRoot.GetMenu(MenuEnum.MapSelection).ActivatePanel();
        }

        private void StoreButtonListner()
        {
            DeactivatePanel();
            menuRoot.GetMenu(MenuEnum.WeaponShop).ActivatePanel();
        }

        private void SettingButtonListner()
        {
            DeactivatePanel();
            menuRoot.GetMenu(MenuEnum.SettingPanel).ActivatePanel();
        }



    }
}