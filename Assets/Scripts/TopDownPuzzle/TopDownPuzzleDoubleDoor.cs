using UnityEngine;

public class TopDownPuzzleDoubleDoor : MonoBehaviour
{
    [SerializeField] private Transform _closePos;
    [SerializeField] private Transform _openPos;
    [SerializeField] private Transform _door;
    [SerializeField] private float _openingSpeed =1.5f;

    public bool IsOpen { get => _lock1 && _lock2; }
    private bool _isUnlock;

    private bool _lock1;
    private bool _lock2;
    private float time;

    private void Start() {
        if (IsOpen) {
            _door.position = _openPos.position;
            time = 1;
        }
        else {
            _door.position = _closePos.position;
            time = 0;
        }
    }

    private void Update() {
        if (IsOpen||_isUnlock) {
            time += _openingSpeed*Time.deltaTime ;
            _isUnlock = true;
        }
        else
        {
            time -=  _openingSpeed*Time.deltaTime ;
        }

        time = Mathf.Clamp01(time);

        _door.position = Vector3.Lerp(_closePos.position, _openPos.position, time);
        
    }
    
    

    public void SetLock1Stat(bool value) {
        _lock1 = value;
    }
    public void SetLock2Stat(bool value) {
        _lock2 = value;
    }
}