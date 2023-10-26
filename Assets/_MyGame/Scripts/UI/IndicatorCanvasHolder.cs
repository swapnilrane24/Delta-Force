using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class IndicatorCanvasHolder : MonoBehaviour
    {
        [SerializeField] private ToggleLabel toggleLabel;

        private void OnDisable()
        {
            GameManager.Instance.onDeathMatchStartEvent.RemoveListener(() => toggleLabel.ToggleVisibility(true));
            GameManager.Instance.onLevelCompleteEvent.AddListener(() => toggleLabel.ToggleVisibility(false));
        }

        private void Start()
        {
            GameManager.Instance.onDeathMatchStartEvent.AddListener(() => toggleLabel.ToggleVisibility(true));
            GameManager.Instance.onLevelCompleteEvent.AddListener(() => toggleLabel.ToggleVisibility(false));
        }
    }
}