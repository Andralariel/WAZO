using System;
using UnityEngine;

namespace TechArt
{
    public class SplashPoolingSystem : MonoBehaviour
    {
        public static SplashPoolingSystem Instance;

        [SerializeField] private ParticleSystem[] splashes;
        [SerializeField] private ParticleSystem[] dustClouds;

        private int _currentSplash;
        private int _currentCloud;

        private void Awake()
        {
            if (Instance != default && Instance!=this)
            {
                DestroyImmediate(this);
            }
            Instance = this;
        }

        public ParticleSystem LendASplash()
        {
            _currentSplash++;
            if (_currentSplash > splashes.Length - 1) _currentSplash = 0;
            return splashes[_currentSplash];
        }
        
        public ParticleSystem LendADustCloud()
        {
            _currentCloud++;
            if (_currentCloud > dustClouds.Length - 1) _currentCloud = 0;
            return dustClouds[_currentCloud];
        }
    }
}
