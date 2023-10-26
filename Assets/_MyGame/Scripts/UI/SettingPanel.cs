using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Curio.Gameplay
{
    public class SettingPanel : MonoBehaviour
    {
        [SerializeField] private TogglePanel togglePanel;
        [SerializeField] private Button soundButton;
        [SerializeField] private Image soundBtnImg;
        [SerializeField] private Sprite soundOnIcon, soundOffIcon;
        public Slider sensitivitySlider;

        private void OnDisable()
        {
            sensitivitySlider.onValueChanged.RemoveListener(UpdateSensitivity);
            soundButton.onClick.RemoveListener(SoundButtonListner);
        }

        private void Start()
        {
            GameManager.Instance.mouseSensitivity = ES3.Load<float>("MouseSensitivity", 0.4f);

            // Initialize the slider's value with the default sensitivity
            sensitivitySlider.value = GameManager.Instance.mouseSensitivity;

            // Add a listener to the slider to react to changes in sensitivity
            sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
            soundButton.onClick.AddListener(SoundButtonListner);
        }

        public void InitilalizePanel()
        {
            togglePanel.ToggleVisibility(true);

            soundBtnImg.sprite = SoundManager.Instance.IsSoundOn == true ? soundOnIcon : soundOffIcon;
        }

        private void UpdateSensitivity(float newSensitivity)
        {
            // Update the sensitivity value when the slider value changes
            GameManager.Instance.mouseSensitivity = newSensitivity;
            ES3.Save<float>("MouseSensitivity", GameManager.Instance.mouseSensitivity);
        }

        private void SoundButtonListner()
        {
            if (SoundManager.Instance.IsSoundOn)
            {
                SoundManager.Instance.DeactivateAudio();
                soundBtnImg.sprite = soundOffIcon;
            }
            else
            {
                SoundManager.Instance.ActivateAudio();
                soundBtnImg.sprite = soundOnIcon;
            }
        }

    }
}