using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class GeneratorTrailMoment : MonoBehaviour
{

    [SerializeField] private float _animationTime;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private SplineAnimate _splineAnimate;
    [SerializeField] private MeshRenderer _meshCableToRail;
    [SerializeField] private CinemachineVirtualCamera _originalVcam;

    [Space(10), Header("OnComplet")] [SerializeField]
    private Transform _newVCamPos;
    [SerializeField] private MeshRenderer _meshCableToDoor;
    [SerializeField] private TopDownConsole _doorConsole;
    public UnityEvent _onComplet;

    private float _timer;
    private bool _isInAnimation;

    private void Start() {
        _virtualCamera.enabled = false;
        _splineAnimate.Duration = _animationTime;
        _meshCableToRail.material.SetFloat("_EnergieOn", 0);
        _meshCableToDoor.material.SetFloat("_EnergieOn", 0);
    }

    public void StartMoment() {
        _splineAnimate.Play();
        _virtualCamera.enabled = true;
        _meshCableToRail.material.SetFloat("_EnergieOn", 1);
        _isInAnimation = true;
    }
    
    private void Complete() {
        //_originalVcam.gameObject.SetActive(false);
        //_originalVcam.transform.position = _newVCamPos.position;
        //_originalVcam.gameObject.SetActive(true);
        _originalVcam.ForceCameraPosition(_newVCamPos.position, _newVCamPos.rotation);
        _doorConsole.ChangeStat(TopDownConsole.Stat.OnLine);
        _meshCableToDoor.material.SetFloat("_EnergieOn", 1);
        _virtualCamera.enabled = false;
        _isInAnimation = false;
        _onComplet?.Invoke();
        
    }

    private void Update() {
        if (!_isInAnimation) return;
        _timer += Time.deltaTime;
        if (_timer >= _animationTime) {
            Complete();
        }
    }
}