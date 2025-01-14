using UnityEngine;

public class TopDownPuzzleDoor : MonoBehaviour
{
    [SerializeField] private Transform _closePos;
    [SerializeField] private Transform _openPos;
    [SerializeField] private Transform _door;
    [SerializeField] private float _openingSpeed =1.5f;

    public bool IsOpen;

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
        if (IsOpen) {
            time += _openingSpeed*Time.deltaTime ;
        }
        else
        {
            time -=  _openingSpeed*Time.deltaTime ;
        }

        time = Mathf.Clamp01(time);

        _door.position = Vector3.Lerp(_closePos.position, _openPos.position, time);
    }

    public void SetDoorStat(bool value) {
        IsOpen = value;
    }
}