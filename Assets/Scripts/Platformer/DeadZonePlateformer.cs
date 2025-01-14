using System;
using UnityEngine;

public class DeadZonePlateformer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            Debug.Log("IsDead");
            other.GetComponent<PlayerControler>().Respawn();
        }
    }
}