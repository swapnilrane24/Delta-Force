using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public enum MenuEnum
    {
        None, MainMenu, SettingPanel, MapSelection, TeamSelection, GameUI, MatchResultUI, RespawningMenu, WeaponShop, GameOverUI, GameWinUI
    }

    public class MenuRoot : MonoBehaviour
    {
        [SerializeField] private TotalCurrencyUI totalCurrencyUI;
        [SerializeField] private CoinCollectAnimUI coinCollectAnimUI;
        [SerializeField]private List<BaseUI> menuList;

        public CoinCollectAnimUI CoinCollectAnimUI => coinCollectAnimUI;
        public TotalCurrencyUI TotalCurrencyUI => totalCurrencyUI;

        private void Start()
        {
            for (int i = 0; i < menuList.Count; i++)
            {
                menuList[i].SetMenuRoot = this;
            }
        }

        public BaseUI GetMenu(MenuEnum menuEnum)
        {
            BaseUI baseUI = null;

            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].MenuEnum == menuEnum)
                {
                    baseUI = menuList[i];
                    break;
                }
            }

            return baseUI;
        }




    }
}