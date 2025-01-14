using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlatformerCheckPoint : MonoBehaviour
{


    [SerializeField] private bool _isDefaultSpawn;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private MeshRenderer _displayRenderer;
    [SerializeField][ColorUsage(true , true)] private Color _defaultColor = Color.cyan;
    [SerializeField][ColorUsage(true , true)] private Color _activateColor = Color.green;
    [SerializeField] private float _animationTime = 0.5f;
    
    
    public static Transform RESPAWNPOINT;

    private float _timer;
    private bool _doAniamtion;
    private void Start() {
        if (_isDefaultSpawn && _respawnPoint != null) {
            SetRespawnActive();
        }
        _displayRenderer.material.color = _defaultColor;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") &&!_doAniamtion)
        {
            SetRespawnActive();
        }
    }

    private void SetRespawnActive() {
        RESPAWNPOINT = _respawnPoint;
        _doAniamtion = true;
    }

    private void Update() {
        if (_doAniamtion) {
            _timer += Time.deltaTime;
            _displayRenderer.material.color =Color.Lerp(_defaultColor , _activateColor, _timer/_animationTime);
            if (_timer >= _animationTime) {
                _doAniamtion = false;
                _displayRenderer.material.color = _activateColor;
                enabled = false;
            }
        }
    }
}