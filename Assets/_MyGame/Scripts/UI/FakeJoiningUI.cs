using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    public class FakeJoiningUI : MonoBehaviour
    {
        [SerializeField] private int countDown;
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private TextMeshProUGUI playerJoinedText;

        private float currentCountDown;
        private int playerJoinedCount;
        [HideInInspector] public UnityEvent countDownCompleteEvent;

        public void InitializeFakingUI()
        {
            currentCountDown = countDown;
            gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (currentCountDown > 0)
            {
                currentCountDown -= Time.deltaTime;
                SetTimer(currentCountDown);

                playerJoinedCount = Mathf.FloorToInt((1f - (currentCountDown / countDown)) * DeathMatchManager.Instance.NumberOfTeamMembers * 2);
                playerJoinedText.text = playerJoinedCount + "/" + (DeathMatchManager.Instance.NumberOfTeamMembers * 2);
                if (currentCountDown <= 0)
                {
                    countDownCompleteEvent?.Invoke();
                }
            }
        }

        public void SetTimer(float value)
        {
            TimeSpan time = TimeSpan.FromSeconds(value); //set the time value
            countDownText.text = time.ToString("mm':'ss");   //convert time to Time format
        }
    }
}