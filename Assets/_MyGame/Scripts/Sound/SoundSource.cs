using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class SoundSource : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public AudioSource AudioSource => audioSource;

        private SoundManager _soundManager;
        private float activationTime;

        private void Update()
        {
            activationTime -= Time.deltaTime;
            if (activationTime <= 0)
            {
                //return to manager
                _soundManager.ReturnSoundSource(this);
                gameObject.SetActive(false);
            }
        }

        public void InitializeSoundSource(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        public void PlayAudio(AudioClip clip, float volume, float pitch)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            activationTime = clip.length * (1 + 0.1f);
            gameObject.SetActive(true);
            audioSource.Play();
        }


    }
}