using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class SoundPlay : MonoBehaviour
    {
        [SerializeField] private bool onlyPlayInGamePlayMode;
        [SerializeField] private bool playOnActive;
        [SerializeField] private bool playOnDeactive;
        [SerializeField] private bool isGUISound;
        [Range(0f, 1f)]
        [SerializeField] private float volumn = 1;
        [Range(0,100)]
        [SerializeField] private float randomPitchPercent = 10;
        [SerializeField] private AudioClip _clip;

        private void OnEnable()
        {
            if (playOnActive)
            {
                bool canPlay = true;

                if (onlyPlayInGamePlayMode && GameManager.Instance.GameState != GameState.PLAYING)
                    canPlay = false;

                if (canPlay)
                    Play();
            }
        }

        private void OnDisable()
        {
            if (playOnDeactive)
            {
                bool canPlay = true;

                if (onlyPlayInGamePlayMode && GameManager.Instance.GameState != GameState.PLAYING)
                    canPlay = false;

                if (canPlay)
                    Play();
            }
        }

        public void Play()
        {
            if (SoundManager.Instance)
            {
                SoundManager.Instance.Play(_clip, volumn, randomPitchPercent, transform.position, isGUISound);
            }
        }

        public void Play(AudioClip clip)
        {
            _clip = clip;
            if (SoundManager.Instance != null)
                SoundManager.Instance.Play(_clip, volumn, randomPitchPercent, transform.position, isGUISound);
        }
    }
}