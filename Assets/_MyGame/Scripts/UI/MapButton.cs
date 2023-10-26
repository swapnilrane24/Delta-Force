using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Curio.Gameplay
{
    [System.Serializable]
    public struct MapButtonData
    {
        public string mapName;
        public Sprite mapIcon;
        public int arenaIndex;
    }

    public class MapButton : MonoBehaviour
    {
        [SerializeField] private Button mapButton;
        [SerializeField] private TextMeshProUGUI mapNameText;
        [SerializeField] private Image mapIconImage;
        [SerializeField] private Image selectedImage;

        public UnityEvent<int> onButtonSelected;

        private int arenaIndex;

        public void SetMapButton(MapButtonData mapButtonData)
        {
            arenaIndex = mapButtonData.arenaIndex;
            mapNameText.text = mapButtonData.mapName;
            mapIconImage.sprite = mapButtonData.mapIcon;
            mapButton.onClick.AddListener(MapButtonListner);
        }

        private void MapButtonListner()
        {
            DeathMatchManager.Instance.SelectedArenaIndex(arenaIndex);
            selectedImage.enabled = true;
            onButtonSelected?.Invoke(arenaIndex);
        }

        public void CheckIfThisButtonIsClicked(int selectedArenaIndex)
        {
            selectedImage.enabled = arenaIndex == selectedArenaIndex;
        }
    }
}