using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Curio.Gameplay
{
    public class MatchResultUI : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private TextMeshProUGUI blueTeamScoreText, redTeamScoreText;
        [SerializeField] private TextMeshProUGUI WinnerTeamNameText;

        private void Start()
        {
            DeathMatchManager.Instance.MatchResultUI = this;
            continueButton.onClick.AddListener(ContinueButtonListner);
        }

        public void ToggleVisibility(bool visible)
        {
            Cursor.lockState = CursorLockMode.None;
            togglePanel.ToggleVisibility(visible);
        }

        public void SetScore(int blueTeamScore, int redTeamScore)
        {
            blueTeamScoreText.text = "" + blueTeamScore;
            redTeamScoreText.text = "" + redTeamScore;

            if (blueTeamScore > redTeamScore)
            {
                WinnerTeamNameText.text = "TEAM BLUE WINS";
            }
            else if (blueTeamScore < redTeamScore)
            {
                WinnerTeamNameText.text = "TEAM RED WINS";
            }
            else if (blueTeamScore == redTeamScore)
            {
                WinnerTeamNameText.text = "MATCH IS DRAW";
            }

        }

        public void ContinueButtonListner()
        {
            GameManager.Instance.LoadLevel();
        }

    }
}