using UnityEngine;

public class RotateTowardTest : MonoBehaviour
{
    [SerializeField] private float _maxDegreesDelta;

    [SerializeField] private Transform _debugTransform;
    private Vector3 _newDir;
    private Quaternion quat;

    private void Start()
    {
        _newDir = transform.forward;
    }

    private void Update() {
        

        _debugTransform.rotation = Quaternion.RotateTowards(_debugTransform.rotation, Quaternion.LookRotation(transform.forward),
            _maxDegreesDelta * Time.deltaTime);
    }
}