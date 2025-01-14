using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourcePlayerControllerPlatformer : MonoBehaviour
{
    [SerializeField] private PlayerControler _playerControler;
    [SerializeField] private AudioElement _aeJump;
    [SerializeField] private AudioElement _aeClimb;
    [SerializeField] private AudioElement _aeLanding;
    [SerializeField] private AudioMixerGroup _mixerGroup;

    private void Start()
    {
        _playerControler.OnCimb+= PlayerControlerOnOnCimb;
        _playerControler.OnJump+= PlayerControlerOnOnJump;
        _playerControler.OnLanding += PlayerControlerOnOnLanding;
    }

    private void PlayerControlerOnOnLanding(object sender, EventArgs e)=>PlayerAudioElement(_aeLanding);
    private void PlayerControlerOnOnJump(object sender, EventArgs e)=>PlayerAudioElement(_aeJump);
    private void PlayerControlerOnOnCimb(object sender, EventArgs e)=> PlayerAudioElement(_aeClimb);
    

    private void PlayerAudioElement(AudioElement audioElement)
    {
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = audioElement.GetSound();
        audioSource.pitch = audioElement.GetPitch();
        audioSource.volume = audioElement.Volume;
        audioSource.outputAudioMixerGroup = _mixerGroup;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length+1);
    }
}