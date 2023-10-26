using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class MainMenuUI : MonoBehaviour
    {
        public Button blueTeamButton, redTeamButton;
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private FakeJoiningUI fakeJoiningUI;
        [SerializeField] private GameUI gameUI;
        [SerializeField] private GameObject teamButtonHolder;

        private void OnDisable()
        {
            fakeJoiningUI.countDownCompleteEvent.RemoveListener(StartTheMatch);
        }

        private void Start()
        {
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
            togglePanel.ToggleVisibility(false);
            gameUI.ToggleVisibility(true);
            DeathMatchManager.Instance.StartMatch();
        }
    }
}