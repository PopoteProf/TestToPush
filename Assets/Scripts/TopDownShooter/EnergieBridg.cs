using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public  class EnergieBridg : MonoBehaviour
{
    [SerializeField] private Collider _bridgeCollider;
    [SerializeField] private MeshRenderer _cableMesh;
    [SerializeField] private MeshRenderer _bridgeMesh;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private float _fadeInTime= 2;
    


    private float _timer;
    private bool _activation;
    public void Start() {
        _bridgeCollider.enabled = false;
        _bridgeCollider.gameObject.SetActive(false);
        _cableMesh.material.SetFloat("_EnergieOn", 0);
        _bridgeMesh.material.SetFloat("_DisolveProgress", 0);
        _virtualCamera.enabled = false;
        
    }

    public void Activate() {
        _bridgeCollider.enabled = true;
        _bridgeMesh.gameObject.SetActive(true);
        _cableMesh.material.SetFloat("_EnergieOn", 1);
        _activation = true;
        _virtualCamera.enabled = true;
        GameStaticManager.SetPause(true);
    }

    private void Update() {
        if(!_activation)return;
        _timer += Time.deltaTime;
        _bridgeMesh.material.SetFloat("_DisolveProgress", _timer/_fadeInTime);
        if (_timer >= _fadeInTime) {
            _activation = false;
            _bridgeMesh.material.SetFloat("_DisolveProgress", 1);
            _virtualCamera.enabled = false;
            GameStaticManager.SetPause(false);
        }
    }
}