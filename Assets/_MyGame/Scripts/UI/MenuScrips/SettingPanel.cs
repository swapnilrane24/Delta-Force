using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace Curio.Gameplay
{
    public class SettingPanel : BaseUI
    {
        public Slider sensitivitySlider;
        public Slider soundSlider;

        private void OnDisable()
        {
            sensitivitySlider.onValueChanged.RemoveListener(UpdateSensitivity);
            soundSlider.onValueChanged.RemoveListener(UpdateSound);
        }

        protected override void Start()
        {
            base.Start();
            GameManager.Instance.mouseSensitivity = ES3.Load<float>("MouseSensitivity", 0.4f);
            GameManager.Instance.UpdateSoundValumn(ES3.Load<float>("SoundVolume", 1f));

            // Initialize the slider's value with the default sensitivity
            sensitivitySlider.value = GameManager.Instance.mouseSensitivity;
            soundSlider.value = GameManager.Instance.GetSoundVolumn;

            // Add a listener to the slider to react to changes in sensitivity
            sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
            soundSlider.onValueChanged.AddListener(UpdateSound);
        }

        private void UpdateSensitivity(float newSensitivity)
        {
            // Update the sensitivity value when the slider value changes
            GameManager.Instance.mouseSensitivity = newSensitivity;
            ES3.Save<float>("MouseSensitivity", GameManager.Instance.mouseSensitivity);
        }

        private void UpdateSound(float volume)
        {
            // Update the sensitivity value when the slider value changes
            GameManager.Instance.UpdateSoundValumn(volume);
            ES3.Save<float>("SoundVolume", GameManager.Instance.GetSoundVolumn);
        }

        protected override void BackButton()
        {
            base.BackButton();
            menuRoot.GetMenu(MenuEnum.MainMenu).ActivatePanel();
        }

    }
}