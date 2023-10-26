using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class PickUpDamage : PickUpAction
    {

        [Range(0f, 100f)]
        [SerializeField] private float damageIncreasePercent = 25;

        public override void TakeAction(Actor actor)
        {
            if (actor.IsPlayer)
            {
                //Call Popup Text
                onPickUpEvent?.Invoke();
                Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
                PickUpManager.Instance.GetDamageNumber().leftText = "+";
                PickUpManager.Instance.GetDamageNumber().rightText = "%Dam";
                PickUpManager.Instance.GetDamageNumber().Spawn(spawnPos, damageIncreasePercent);
                actor.CollectDamagePickUp(damageIncreasePercent / 100f);
            }
        }

    }
}