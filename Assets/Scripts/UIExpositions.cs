using System.Diagnostics;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class UIExpositions : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera1;
    [SerializeField] private CinemachineVirtualCamera _camera2;
    [SerializeField] private CinemachineVirtualCamera _camera3;

    [SerializeField] private Button _bpButton1;
    [SerializeField] private Button _bpButton2;
    [SerializeField] private Button _bpButton3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _bpButton1.onClick.AddListener(CallCam1);
        _bpButton2.onClick.AddListener(CallCam2);
        _bpButton3.onClick.AddListener(CallCam3);
    }

    

    private void CallCam1() => ChangeCamera(1);
    private void CallCam2() => ChangeCamera(2);
    private void CallCam3() => ChangeCamera(3);
    
        
    

    private void ChangeCamera(int id) {
        if (_camera1 != null) _camera1.Priority = 1;
        if (_camera2 != null) _camera2.Priority = 1;
        if (_camera3 != null) _camera3.Priority = 1;
        
        switch (id) {
            case 1 :
                if (_camera1 == null) return;
                _camera1.Priority = 5;
                break;
            case 2 :
                if (_camera2 == null) return;
                _camera2.Priority = 5;
                break;
            case 3 :
                if (_camera3 == null) return;
                _camera3.Priority = 5;
                break;
        }
    }
}
