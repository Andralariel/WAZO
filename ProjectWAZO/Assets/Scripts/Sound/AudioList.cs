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
            ending,
            intro,
            zoneOuverte,
            temple,
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
        [SerializeField] private AudioClip zoneIntroTheme;
        [SerializeField] [Range(0, 1)] private float zoneIntroVolume;
        [SerializeField] private AudioClip zoneOuverteTheme;
        [SerializeField] [Range(0, 1)] private float zoneOuverteVolume;
        [SerializeField] private AudioClip zoneTempleTheme;
        [SerializeField] [Range(0, 1)] private float zoneTempleVolume;

        [Header("UI")]
        public AudioClip uiClick1;
        public AudioClip uiClick2;
        public AudioClip uiClick3;

        [Header("Cinematique")]
        public AudioClip cinematiqueOrbe;
        [Range(0, 1)] public float cinematiqueOrbeVolume;
        public AudioClip fallingRock;
        [Range(0, 1)] public float fallingRockVolume;
        public AudioClip crashGround;
        [Range(0, 1)] public float crashGroundVolume;
        public AudioClip openDoorTemple;
        [Range(0, 1)] public float openDoorTempleVolume;

        [Header("Player")]
        public AudioClip saut;
        [Range(0, 1)] public float sautVolume;
        public AudioClip grabPetitAir;
        [Range(0, 1)] public float grabPetitAirVolume;
        public AudioClip grabGrosAir;
        [Range(0, 1)] public float grabGrosAirVolume;
        public AudioClip grabGrosTerre;
        [Range(0, 1)] public float grabGrosTerreVolume;
        public AudioClip deathScream;
        [Range(0, 1)] public float deathScreamVolume;
        public AudioClip step;
        [Range(0, 1)] public float stepVolume;
        public AudioClip grabOther;
        [Range(0, 1)] public float grabOtherVolume;

        [Header("Interacteur")]
        public AudioClip getKey;
        [Range(0, 1)] public float getKeyVolume;
        public AudioClip putSpiritAltar;
        [Range(0, 1)] public float putSpiritAltarVolume;
        public AudioClip altarActive;
        [Range(0, 1)] public float altarActiveVolume;
        public AudioClip turnWindmill;
        [Range(0, 1)] public float turnWindWillVolume;

        [Header("environnement")]
        public AudioClip waterfall;
        public AudioClip auraKey;
        public AudioClip river;

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
                case Music.intro:
                    _currentSource.clip = zoneIntroTheme;
                    _targetVolume = zoneIntroVolume;
                    break;
                case Music.zoneOuverte:
                    _currentSource.clip = zoneOuverteTheme;
                    _targetVolume = zoneOuverteVolume;
                    break;
                case Music.temple:
                    _currentSource.clip = zoneTempleTheme;
                    _targetVolume = zoneTempleVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(music), music, null);
            }
            _currentSource.loop = loop;
            _currentSource.Play();
        
            FadeIn();
        }

        public void StopMusic()
        {
            FadeOut();
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
