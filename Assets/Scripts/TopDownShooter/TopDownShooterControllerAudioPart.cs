using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class TopDownShooterControllerAudioPart : MonoBehaviour
{
    [SerializeField] private TopDownShooterControler _topDownShooterControler;

    [SerializeField] private AudioElement _fireAudioElement;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    private void Start()
    {
        _topDownShooterControler.OnFire+= TopDownShooterControlerOnOnFire;
    }

    private void TopDownShooterControlerOnOnFire(object sender, EventArgs e) {
        AudioSource audioSource = transform.AddComponent<AudioSource>();
        audioSource.clip = _fireAudioElement.GetSound();
        audioSource.pitch = _fireAudioElement.GetPitch();
        audioSource.outputAudioMixerGroup = _audioMixerGroup;
        audioSource.Play();
        Destroy(audioSource ,audioSource.clip.length+1);
    }
}