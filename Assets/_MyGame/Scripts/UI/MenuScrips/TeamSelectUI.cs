using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class TeamSelectUI : BaseUI
    {
        public Button blueTeamButton, redTeamButton;
        [SerializeField] private FakeJoiningUI fakeJoiningUI;
        //[SerializeField] private GameUI gameUI;
        [SerializeField] private GameObject teamButtonHolder;

        private void OnDisable()
        {
            fakeJoiningUI.countDownCompleteEvent.RemoveListener(StartTheMatch);
        }

        protected override void Start()
        {
            base.Start();
            blueTeamButton.onClick.AddListener(BlueTeamButtonListner);
            redTeamButton.onClick.AddListener(RedTeamButtonListner);
            fakeJoiningUI.countDownCompleteEvent.AddListener(StartTheMatch);
        }

        private void BlueTeamButtonListner()
        {
            DeathMatchManager.Instance.PlayerBlueTeamSelected();
            teamButtonHolder.SetActive(false);
            fakeJoiningUI.InitializeFakingUI();
            //togglePanel.ToggleVisibility(false);
        }

        private void RedTeamButtonListner()
        {
            DeathMatchManager.Instance.PlayerRedTeamSelected();
            teamButtonHolder.SetActive(false);
            fakeJoiningUI.InitializeFakingUI();
            //togglePanel.ToggleVisibility(false);
        }

        private void StartTheMatch()
        {
            DeactivatePanel();
            menuRoot.GetMenu(MenuEnum.GameUI).ActivatePanel();
            DeathMatchManager.Instance.StartMatch();
        }

        protected override void BackButton()
        {
            base.BackButton();
            menuRoot.GetMenu(MenuEnum.MapSelection).ActivatePanel();
        }
    }
}