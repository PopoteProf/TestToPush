using Cinemachine;
using UnityEngine;

public class CorridorMoment : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera _vCam;
    public void EnterCorridor() {
        _vCam.enabled = true;
    }

    public void ExitCorridor() {
        _vCam.enabled = false;
    }
}