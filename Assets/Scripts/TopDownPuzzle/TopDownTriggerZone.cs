using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class TopDownTriggerZone : MonoBehaviour
{
    public UnityEvent OnPlayerEnter = new UnityEvent();
    public UnityEvent OnPlayerExit = new UnityEvent();

    private Collider col;
    private void OnTriggerEnter(Collider other) {
        if (col == null) {
            col = other;
            OnPlayerEnter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (col == other) {
            col = null;
            OnPlayerExit.Invoke();
        }
    }
}