using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class PickUpHealth : PickUpAction
    {
        [Range(0f,100f)]
        [SerializeField] private float healthIncreasePercent = 25;
        public override void TakeAction(Actor actor)
        {
            if (actor.CanPickUpHealth && actor.IsPlayer)
            {
                //Call Popup Text
                onPickUpEvent?.Invoke();
                Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
                PickUpManager.Instance.GetDamageNumber().leftText = "+";
                PickUpManager.Instance.GetDamageNumber().rightText = "%HP";
                PickUpManager.Instance.GetDamageNumber().Spawn(spawnPos, healthIncreasePercent);
                actor.CollectHealthPickUp(healthIncreasePercent / 100f);
            }
        }

    }
}