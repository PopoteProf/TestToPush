using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AudioElement {
    [SerializeField, Range(-3, 3)] private float _minPitch=1;
    [SerializeField, Range(-3, 3)] private float _maxPitch=1;
    [SerializeField, Range(0, 1)] private float _volume=0.75f;
    [SerializeField] private List<AudioClip> _sounds;

    public float Volume => _volume;

    public bool IsNull {
        get {
            if (_sounds == null || _sounds.Count == 0) return true;
            return false;
        }
    }

    public AudioClip GetSound() {
        if (_sounds == null || _sounds.Count == 0) return null;
        return _sounds[Random.Range(0, _sounds.Count)];
    }

    public float GetPitch() => Random.Range(_minPitch, _maxPitch);

}