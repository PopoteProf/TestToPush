using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdManage : MonoBehaviour
{
    [SerializeField] private BirdFly[] _birds;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private float _maxDelay = 0.5f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            StartBirdFly();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetBirds();
        }
    }

    [ContextMenu("StartFly")]
    private void StartBirdFly() {
        foreach (var bird in _birds) {
            if (bird == null) continue;
            bird.SetReadyToFlyAway(_dir, Random.Range(0,_maxDelay));
        }
    }
    [ContextMenu("RestBird")]
    private void ResetBirds() {
        foreach (var bird in _birds) {
            if (bird == null) continue;
            bird.ResetDefaultPos();
        }
    }
}