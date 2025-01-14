using Cinemachine;
using UnityEngine;


public class VirtualCameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camToActivate;
    [SerializeField] private CinemachineVirtualCamera _camToDesable;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            if (_camToActivate) _camToActivate.Priority = 5;
            else Debug.LogWarning(" Il n'y a pas de Virtual camera a activé!", this);
            if (_camToDesable) _camToDesable.Priority = 1;
            else Debug.LogWarning(" Il n'y a pas de Virtual camera à désactivé!", this);
        }
    }
}