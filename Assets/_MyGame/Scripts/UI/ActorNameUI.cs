using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Curio.Gameplay
{
    public class ActorNameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;

        public void SetTextColor(Color color)
        {
            nameText.color = color;
        }

        public void SetActorName(string value)
        {
            nameText.text = value;
        }


    }
}