using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class PickUpCoin : PickUpAction
    {
        [SerializeField] private int coinAmount;

        //private void OnEnable()
        //{
        //    GameManager.Instance.onLevelWinEvent.AddListener(GetCollected);
        //}

        //private void OnDisable()
        //{
        //    GameManager.Instance.onLevelWinEvent.RemoveListener(GetCollected);
        //}

        private void GetCollected()
        {
            if (gameObject.activeInHierarchy)
            {
                GameManager.Instance.IncreaseRoundEarning(coinAmount);
                gameObject.SetActive(false);
            }
        }

        public override void TakeAction(Actor actor)
        {
            //Call Popup Text
            if (actor.IsPlayer) //only player can pickup coins
            {
                onPickUpEvent?.Invoke();
                Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
                PickUpManager.Instance.GetDamageNumber().leftText = "+";
                PickUpManager.Instance.GetDamageNumber().rightText = "";
                PickUpManager.Instance.GetDamageNumber().Spawn(spawnPos, coinAmount);
                actor.CollectCoinPickUp();
                GameManager.Instance.IncreaseRoundEarning(coinAmount);
            }
        }
    }
}