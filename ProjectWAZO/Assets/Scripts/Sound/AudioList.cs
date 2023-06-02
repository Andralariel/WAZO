using System;
using DG.Tweening;
using UnityEngine;

namespace Sound
{
    public class AudioList : MonoBehaviour
    {
        private enum SourceType
        {
            Music,
            Ambiance
        }
        
        public enum Music
        {
            Main,
            Ending,
            ZoneOuverte,
            Temple
        }
        
        public enum Ambiance
        {
            Rain,
            Wind
        }

        #region variables
        
        public static AudioList Instance;
        [SerializeField] private AudioSource audioSourceOneShot;
        
        [Header("Music")]
        [SerializeField] private AudioSource musicAudioSource0;
        [SerializeField] private AudioSource musicAudioSource1;
        [SerializeField] private float fadeMusicDuration = 2;
        private bool _playMusicOn1, _notMusicFirstCall;
        private float _targetMusicVolume;
        private AudioSource _currentMusicSource;

        [Header("Ambiance")]
        [SerializeField] private AudioSource ambianceAudioSource0;
        [SerializeField] private AudioSource ambianceAudioSource1;
        [SerializeField] private float fadeAmbianceDuration = 2;
        private bool _playAmbianceOn1, _notAmbianceFirstCall;
        private float _targetAmbianceVolume;
        private AudioSource _currentAmbianceSource;

        [Header("Global")]
        [SerializeField] private AudioClip mainTheme;
        [SerializeField] [Range(0, 1)] private float mainVolume;
        [SerializeField] private AudioClip endingTheme;
        [SerializeField] [Range(0, 1)] private float endingVolume;
        [SerializeField] private AudioClip zoneOuverteTheme;
        [SerializeField] [Range(0, 1)] private float zoneOuverteVolume;
        [SerializeField] private AudioClip zoneTempleTheme;
        [SerializeField] [Range(0, 1)] private float zoneTempleVolume;
        [SerializeField] private AudioClip rainAmbiance;
        [SerializeField] [Range(0, 1)] private float rainAmbianceVolume;
        [SerializeField] private AudioClip windAmbiance;
        [SerializeField] [Range(0, 1)] private float windAmbianceVolume;

        [Header("UI")]
        public AudioClip uiClick1;
        public AudioClip uiClick2;
        public AudioClip uiClick3;
        public AudioClip openCarnet;
        [Range(0, 1)] public float openCarnetVolume;
        public AudioClip closeCarnet;
        [Range(0, 1)] public float closeCarnetVolume;

        [Header("Cinematique")]
        public AudioClip cinematiqueOrbe;
        [Range(0, 1)] public float cinematiqueOrbeVolume;
        public AudioClip fallingRock;
        [Range(0, 1)] public float fallingRockVolume;
        public AudioClip crashGround;
        [Range(0, 1)] public float crashGroundVolume;
        public AudioClip openDoorTemple;
        [Range(0, 1)] public float openDoorTempleVolume;
        public AudioClip keyOpenDoor;
        [Range(0, 1)] public float keyOpenDoorVolume;
        public AudioClip spiritSpawn;
        [Range(0, 1)] public float spiritSpawnVolume;
        public AudioClip spiritDespawn;
        [Range(0, 1)] public float spiritDespawnVolume;
        public AudioClip mapSpawn;
        [Range(0, 1)] public float mapSpawnVolume;
        public AudioClip keyOnDoor;
        [Range(0, 1)] public float keyOnDoorVolume;
        public AudioClip hatOnHead;
        [Range(0, 1)] public float hatOnHeadVolume;
        public AudioClip eboulementEnding1;
        [Range(0, 1)] public float eboulementEnding1Volume;
        public AudioClip eboulementEnding2;
        [Range(0, 1)] public float eboulementEnding2Volume;

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
        public AudioClip splashPlayer;
        [Range(0, 1)] public float splashPlayerVolume;
        public AudioClip grabOther;
        [Range(0, 1)] public float grabOtherVolume;
        public AudioClip splashObject;
        [Range(0, 1)] public float splashObjectVolume;

        [Header("Interacteur")]
        public AudioClip getKey;
        [Range(0, 1)] public float getKeyVolume;
        public AudioClip putSpiritAltar;
        [Range(0, 1)] public float putSpiritAltarVolume;
        public AudioClip altarActive;
        [Range(0, 1)] public float altarActiveVolume;
        public AudioClip turnWindmill;
        
        #endregion
        
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
            if (_notMusicFirstCall) FadeOut(SourceType.Music);
            else _notMusicFirstCall = true;

            _currentMusicSource = _playMusicOn1? musicAudioSource1 : musicAudioSource0;
            _playMusicOn1 = !_playMusicOn1;
        
            switch (music)
            {
                case Music.Main:
                    _currentMusicSource.clip = mainTheme;
                    _targetMusicVolume = mainVolume;
                    break;
                case Music.Ending:
                    _currentMusicSource.clip = endingTheme;
                    _targetMusicVolume = endingVolume;
                    break;
                case Music.ZoneOuverte:
                    _currentMusicSource.clip = zoneOuverteTheme;
                    _targetMusicVolume = zoneOuverteVolume;
                    break;
                case Music.Temple:
                    _currentMusicSource.clip = zoneTempleTheme;
                    _targetMusicVolume = zoneTempleVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(music), music, null);
            }
            _currentMusicSource.loop = loop;
            _currentMusicSource.Play();
        
            FadeIn(SourceType.Music);
        }
        
        public void StartAmbiance(Ambiance ambiance, bool loop)
        {
            if (_notAmbianceFirstCall) FadeOut(SourceType.Ambiance);
            else _notAmbianceFirstCall = true;

            _currentAmbianceSource = _playAmbianceOn1? ambianceAudioSource1 : ambianceAudioSource0;
            _playAmbianceOn1 = !_playAmbianceOn1;
        
            switch (ambiance)
            {
                case Ambiance.Rain:
                    _currentAmbianceSource.clip = rainAmbiance;
                    _targetAmbianceVolume = rainAmbianceVolume;
                    break;
                case Ambiance.Wind:
                    _currentAmbianceSource.clip = windAmbiance;
                    _targetAmbianceVolume = windAmbianceVolume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ambiance), ambiance, null);
            }
            _currentAmbianceSource.loop = loop;
            _currentAmbianceSource.Play();
        
            FadeIn(SourceType.Ambiance);
        }

        public void StopMusic()
        {
            FadeOut(SourceType.Music);
        }

        public void StopAmbiance()
        {
            FadeOut(SourceType.Ambiance);
        }

        public void PlayOneShot(AudioClip clip, float volumeScale)
        {
            audioSourceOneShot.PlayOneShot(clip,volumeScale);
        }

        private void FadeIn(SourceType type)
        {
            switch (type)
            {
                case SourceType.Music:
                    _currentMusicSource.DOFade(_targetMusicVolume, fadeMusicDuration);
                    break;
                case SourceType.Ambiance:
                    _currentAmbianceSource.DOFade(_targetAmbianceVolume, fadeAmbianceDuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void FadeOut(SourceType type)
        {
            switch (type)
            {
                case SourceType.Music:
                    _currentMusicSource.DOFade(0, fadeMusicDuration);
                    break;
                case SourceType.Ambiance:
                    _currentAmbianceSource.DOFade(0, fadeAmbianceDuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
