using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;

        [SerializeField] private AudioSource audioUI;
        [SerializeField] private SoundSource soundSourcePrefab;
        [SerializeField] private AudioClip mainMusic;
        [Range(0f, 1f)][SerializeField] private float musicVolume;

        [SerializeField] private AudioSource musicSource;
        private List<SoundSource> deactiveSoundSourceList;

        public static SoundManager Instance { get => instance; }

        private bool soundOn;
        public bool IsSoundOn => soundOn;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            soundOn = ES3.Load<bool>("GameSound", true);
            deactiveSoundSourceList = new List<SoundSource>();

            for (int i = 0; i < 5; i++)
            {
                SoundSource soundSource = SpawnSoundSource();
                deactiveSoundSourceList.Add(soundSource);
            }

            if (mainMusic)
            {
                musicSource.clip = mainMusic;
                musicSource.volume = musicVolume;
                musicSource.loop = true;
                musicSource.Play();
            }

            //GameManager.Instance.onDeathMatchStartEvent.AddListener(() =>
            //{
            //    if (mainMusic)
            //    {
            //        musicSource.Play();
            //    }
            //});

            //GameManager.Instance.onLevelCompleteEvent.AddListener(() =>
            //{
            //    if (mainMusic)
            //    {
            //        musicSource.Stop();
            //    }
            //});


            AudioListener.volume = soundOn == true ? 1 : 0;

            //SoundOn(true, true);
        }

        public void RecheckSoundAfterAd()
        {
            AudioListener.volume = soundOn == true ? 1 : 0;
        }

        public void ActivateAudio()
        {
            soundOn = true;
            AudioListener.volume = soundOn == true ? 1 : 0;
            ES3.Save<bool>("GameSound", soundOn);
        }

        public void DeactivateAudio()
        {
            soundOn = false;
            AudioListener.volume = soundOn == true ? 1 : 0;
            ES3.Save<bool>("GameSound", soundOn);
        }

        public void Play(AudioClip clip, float volumn, float RandomPitchPercent, Vector3 placeSourceLocation, bool isGUISound)
        {
            if (isGUISound)
            {
                audioUI.volume = volumn;
                audioUI.PlayOneShot(clip);
            }
            else
            {
                SoundSource selectedAudioSource = GetSoundSource();
                float pitch = 1f;

                if (RandomPitchPercent > 2)
                {
                    pitch *= 1 + Random.Range(-RandomPitchPercent / 100,
                        RandomPitchPercent / 100);
                }

                selectedAudioSource.transform.position = placeSourceLocation;
                selectedAudioSource.PlayAudio(clip, volumn, pitch);
            }
        }

        private SoundSource SpawnSoundSource()
        {
            SoundSource soundSource = Instantiate(soundSourcePrefab, transform);
            soundSource.InitializeSoundSource(this);
            soundSource.gameObject.SetActive(false);

            return soundSource;
        }

        private SoundSource GetSoundSource()
        {
            SoundSource soundSource = null;

            if (deactiveSoundSourceList.Count > 0)
            {
                soundSource = deactiveSoundSourceList[0];
                deactiveSoundSourceList.RemoveAt(0);
            }
            else
            {
                soundSource = SpawnSoundSource();
            }

            return soundSource;
        }

        public void ReturnSoundSource(SoundSource soundSource)
        {
            if (!deactiveSoundSourceList.Contains(soundSource))
                deactiveSoundSourceList.Add(soundSource);
        }




    }
}