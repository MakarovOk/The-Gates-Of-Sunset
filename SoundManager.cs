using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scriptes
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager sounder { get; private set; }
        private AudioSource source;
        private AudioSource sourceMusic;
        private void Awake()
        {
            sounder = this;
            source = GetComponent<AudioSource>();
            sourceMusic = transform.GetChild(0).GetComponent<AudioSource>();
        }
        private void Start()
        {
            if (!PlayerPrefs.HasKey("volumeMusic"))
                sourceMusic.volume = 1;
            if (!PlayerPrefs.HasKey("volumeSound"))
                source.volume = 1;
        }
        public void ActivateSounds(AudioClip[] _sounds)
        {
            var randomNumber = Random.Range(0, _sounds.Length);
            source.PlayOneShot(_sounds[randomNumber]);
        }
        public void ActivateSound(AudioClip _sound)
        {
            source.PlayOneShot(_sound);
        }
        private void Update()
        {
            sourceMusic.volume = PlayerPrefs.GetFloat("volumeMusic");
            source.volume = PlayerPrefs.GetFloat("volumeSound");
        }
    }
}

