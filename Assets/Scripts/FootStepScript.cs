using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class FootStepScript : MonoBehaviour
{
    [SerializeField] private AudioElement _footStep;
    [SerializeField] private AudioMixerGroup _mixerGroup;
    
    public void FootStep() {
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = _footStep.GetSound();
        audioSource.pitch = _footStep.GetPitch();
        audioSource.volume = _footStep.Volume;
        audioSource.outputAudioMixerGroup = _mixerGroup;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length+1);
        
        
    }
}
