using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    
    //[SerializeField] private AudioMixer _audioMixer;
    //[SerializeField] private AudioMixerGroup _sfxMixer;
    [SerializeField] private AudioMixerGroup _musicMixer;
    [SerializeField] private AudioMixerGroup _AmbianceMixer;
    [SerializeField] private AudioMixerGroup _SFXMixer;

    [Space(10), SerializeField] private AudioElement _defaultMusic;
    [SerializeField] private AudioElement _defaultAmbiance;
    
    
    private float _musicFadeTime;
    
    private float _ambianceFadeTime;
    
    private List<ActiveMusicAudioSource> _activeMusicAudioSources = new List<ActiveMusicAudioSource>();
    private ActiveMusicAudioSource _activeMusicAudioSource;
    
    private List<ActiveMusicAudioSource> _activeAmbianceAudioSources = new List<ActiveMusicAudioSource>();
    private ActiveMusicAudioSource _activeAmbianceAudioSource;
    private void Awake() {
        if( Instance!=null) Destroy(gameObject);
        Instance = this;
    }

    private void Start() {
        if( _defaultMusic!=null) PlayMusic(_defaultMusic.GetSound(),1, _defaultMusic.Volume);
        if( _defaultAmbiance!=null) PlayAmbiance(_defaultAmbiance.GetSound(),1, _defaultAmbiance.Volume);
    }


    // Update is called once per frame
    void Update()
    {
       ManageActiveMusicAudioSource(); 
       ManageActiveAmbianceAudioSource();
    }
    
    public void PlayMusic(AudioClip audioClip,float fadingTime=3, float volume = 1f) {
        if (_activeMusicAudioSource != null) {
            if (_activeMusicAudioSource.AudioSource.clip == audioClip) return;
            foreach (var asMusic in _activeMusicAudioSources) {
                asMusic.Time = fadingTime * asMusic.NormalizeVolume;
                asMusic.IsActiveMusic = false;
            }
        }
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.outputAudioMixerGroup = _musicMixer;
        audioSource.volume = 0;
        audioSource.spatialize = false;
        
        audioSource.Play();

        _musicFadeTime = fadingTime;
        
        ActiveMusicAudioSource activeMusicAudioSource = new ActiveMusicAudioSource();
        activeMusicAudioSource.AudioSource = audioSource;
        activeMusicAudioSource.Time = 0;
        activeMusicAudioSource.endvolume = volume;
        activeMusicAudioSource.IsActiveMusic = true;
        _activeMusicAudioSource = activeMusicAudioSource;
        _activeMusicAudioSources.Add(activeMusicAudioSource);
    }
    
    private void ManageActiveMusicAudioSource() {
        for (int i = _activeMusicAudioSources.Count-1; i >=0 ; i--) {
            if (_activeMusicAudioSources[i].IsActiveMusic) {
                _activeMusicAudioSources[i].Time += Time.deltaTime;
                if (_activeMusicAudioSources[i].Time >= _musicFadeTime) {
                    _activeMusicAudioSources[i].Time = _musicFadeTime;
                }
            }
            else {
                _activeMusicAudioSources[i].Time -= Time.deltaTime;
                if (_activeMusicAudioSources[i].Time <= 0) {
                    Destroy(_activeMusicAudioSources[i].AudioSource);
                    _activeMusicAudioSources.Remove(_activeMusicAudioSources[i]);
                    continue;
                }
            }
            _activeMusicAudioSources[i].NormalizeVolume = _activeMusicAudioSources[i].Time / _musicFadeTime;
            _activeMusicAudioSources[i].AudioSource.volume = _activeMusicAudioSources[i].endvolume*_activeMusicAudioSources[i].NormalizeVolume;
        }
    }
    public void PlayAmbiance(AudioClip audioClip,float fadingTime=2, float volume = 1f) {
        if (_activeAmbianceAudioSource != null) {
            if (_activeAmbianceAudioSource.AudioSource.clip == audioClip) return;
            foreach (var asMusic in _activeAmbianceAudioSources) {
                asMusic.Time = fadingTime * asMusic.NormalizeVolume;
                asMusic.IsActiveMusic = false;
            }
        }
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.outputAudioMixerGroup = _AmbianceMixer;
        audioSource.volume = 0;
        audioSource.spatialize = false;
        
        audioSource.Play();

        _ambianceFadeTime = fadingTime;
        
        ActiveMusicAudioSource activeMusicAudioSource = new ActiveMusicAudioSource();
        activeMusicAudioSource.AudioSource = audioSource;
        activeMusicAudioSource.Time = 0;
        activeMusicAudioSource.endvolume = volume;
        activeMusicAudioSource.IsActiveMusic = true;
        _activeAmbianceAudioSource = activeMusicAudioSource;
        _activeAmbianceAudioSources.Add(activeMusicAudioSource);
        
    }
    
    private void ManageActiveAmbianceAudioSource() {
        for (int i = _activeAmbianceAudioSources.Count-1; i >=0 ; i--) {
            if (_activeAmbianceAudioSources[i].IsActiveMusic) {
                _activeAmbianceAudioSources[i].Time += Time.deltaTime;
                if (_activeAmbianceAudioSources[i].Time >= _ambianceFadeTime) {
                    _activeAmbianceAudioSources[i].Time = _ambianceFadeTime;
                }
            }
            else {
                _activeAmbianceAudioSources[i].Time -= Time.deltaTime;
                if (_activeAmbianceAudioSources[i].Time <= 0) {
                    Destroy(_activeAmbianceAudioSources[i].AudioSource);
                    _activeAmbianceAudioSources.Remove(_activeAmbianceAudioSources[i]);
                    continue;
                }
            }
            _activeAmbianceAudioSources[i].NormalizeVolume = _activeAmbianceAudioSources[i].Time / _ambianceFadeTime;
            _activeAmbianceAudioSources[i].AudioSource.volume = _activeAmbianceAudioSources[i].endvolume*_activeAmbianceAudioSources[i].NormalizeVolume;
        }
    }

    public void PlaySFX(AudioElement audioElement)
    {
        if (audioElement.IsNull) return;
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = audioElement.GetSound();
        audioSource.loop = false;
        audioSource.outputAudioMixerGroup = _SFXMixer;
        audioSource.volume = audioElement.Volume;
        audioSource.spatialize = false;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length+1);
    }
    
    private class ActiveMusicAudioSource
    {
        public AudioSource AudioSource;
        public float NormalizeVolume;
        public float endvolume;
        public float Time;
        public bool IsActiveMusic;
    }
}
