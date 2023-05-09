using System;
using DG.Tweening;
using UnityEngine;

namespace Sound
{
    public class AudioList : MonoBehaviour
    {
        public enum Music
        {
            main,
            menu,
            ending
        }
    
        public static AudioList Instance;

        [SerializeField] private StepsSoundVariator steps;
        [SerializeField] private AudioSource audioSource0;
        [SerializeField] private AudioSource audioSource1;
        [SerializeField] private AudioSource audioSourceOneShot;
        [SerializeField] private float fadeDuration;
        private bool _playOn1, _notFirstCall;
        private float _targetVolume;
        private AudioSource _currentSource;

        [Header("Global")]
        [SerializeField] private AudioClip mainTheme;
        [SerializeField] [Range(0, 1)] private float mainVolume;
        [SerializeField] private AudioClip menuTheme;
        [SerializeField] [Range(0, 1)] private float menuVolume;
        [SerializeField] private AudioClip endingTheme;
        [SerializeField] [Range(0, 1)] private float endingVolume;

        [Header("UI")]
        public AudioClip uiClick;

        [Header("Player")]
        public AudioClip basicAttack;
        public AudioClip playerDash;

        [Header("Non-spatialize")]
        public AudioClip playerHit;
        [Range(0, 1)] public float playerHitVolume;
        public AudioClip buyInShop;
        [Range(0, 1)] public float buyInShopVolume;
    
        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    
        public void StartMusic(Music music, bool loop)
        {
            if (_notFirstCall) FadeOut();
            else _notFirstCall = true;

            _currentSource = _playOn1? audioSource1 : audioSource0;
            _playOn1 = !_playOn1;
        
            switch (music)
            {
                case Music.main:
                    _currentSource.clip = mainTheme;
                    _targetVolume = mainVolume;
                    break;
                case Music.menu:
                    _currentSource.clip = menuTheme;
                    _targetVolume = menuVolume;
                    break;
                case Music.ending:
                    _currentSource.clip = endingTheme;
                    _targetVolume = endingVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(music), music, null);
            }
            _currentSource.loop = loop;
            _currentSource.Play();
        
            FadeIn();
        }
    
        public void PlayOneShot(AudioClip clip, float volumeScale)
        {
            audioSourceOneShot.PlayOneShot(clip,volumeScale);
        }

        private void FadeIn()
        {
            _currentSource.DOFade(_targetVolume, fadeDuration);
        }

        private void FadeOut()
        {
            _currentSource.DOFade(0f, fadeDuration);
        }
    }
}
