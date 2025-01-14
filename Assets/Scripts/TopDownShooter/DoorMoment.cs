using System;
using Cinemachine;
using UnityEngine;

public class DoorMoment : MonoBehaviour{
    [SerializeField] private float _momentTime = 34;
    [SerializeField] private Vector3 _doorEndPos;
    [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private Transform _door;
    [SerializeField] private CinemachineImpulseSource _cISRumble;
    [SerializeField] private CinemachineImpulseSource _cIS_FinalBumb;
    [SerializeField] private CinemachineVirtualCamera _vCam;
    [SerializeField] private AudioElement _music;
    [SerializeField] private ParticleSystem _doorSparks;
    [SerializeField] private GameObject[] _gameObjectToActivate;


    private bool _isActivated;
    private Vector3 _doorStartPos;
    private float _time;

    private void Start() {
        _doorStartPos = _door.position;
        ActivateGameObject(false);
        
    }

    public void Activate() {
        _isActivated = true;
        _cISRumble.GenerateImpulse();
        _vCam.enabled = true;
        AudioManager.Instance.PlayMusic(_music.GetSound(), 1, _music.Volume);
        _doorSparks.Play();
        ActivateGameObject(true);
        
    }

    private void ActivateGameObject(bool value) {
        foreach (var go in _gameObjectToActivate) {
            if (go == null) continue;
            go.SetActive(value);
        }
    }

    private void Update()
    {
        if (!_isActivated) return;
        _time += Time.deltaTime;
        _door.position = Vector3.Lerp(_doorStartPos, _doorEndPos, _animationCurve.Evaluate(_time / _momentTime));
        if (_time >= _momentTime) {
            _door.position = _doorEndPos;
            _cIS_FinalBumb.GenerateImpulse();
            _vCam.enabled = false;
            _isActivated = false;
            _doorSparks.Stop();

        }
    }
}