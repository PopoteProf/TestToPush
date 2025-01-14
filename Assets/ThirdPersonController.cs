using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed =1.5f;
    [SerializeField] private float _runAcceleration = 0.2f;
    [SerializeField] private float _runSpeed =2.5f;
    [SerializeField] private float _cameraSpeed=1f;
    [SerializeField] private Transform _aimTarget;
    
    [SerializeField] private float _gravity =-9.8f;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _groundCheckerRadius = 0.4f;
    [SerializeField] private LayerMask _groundMask;

    [Header("Vertical Aim")] 
    [SerializeField] private float _minAimHeight = 0.75f;
    [SerializeField] private float _maxAimHeight = 2;
    [SerializeField] private float _heightCameraSpeed = 50;
    

    private CharacterController _characterController;
    private float _aimheight=1;

    private bool _isGrounded;
    private Vector3 _velocity;
    private float _currentMoveSpeed;
    void Start() {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        
        ManagerIsGrounded();
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xrot = Input.GetAxis("Mouse X");
        
        
        
        if (_isGrounded && _velocity.y < 0) {
            _velocity.y = -2f;
        }
        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
        
        Vector3 moveVec = transform.forward * y + transform.right * x;


        if (Input.GetKey(KeyCode.LeftShift)) {
            _currentMoveSpeed += _runAcceleration * Time.deltaTime;
        }
        else {
            _currentMoveSpeed -=_runAcceleration * Time.deltaTime;
        }
        _currentMoveSpeed = Mathf.Clamp(_currentMoveSpeed, _speed, _runSpeed);
        
        moveVec *= _currentMoveSpeed*Time.deltaTime;
        _characterController.Move(moveVec);
        
        transform.Rotate(Vector3.up, xrot*Time.smoothDeltaTime*_cameraSpeed);
        
        
        
        Vector3 animVec = transform.InverseTransformDirection(_characterController.velocity);
        _animator.SetFloat("X", animVec.x);
        _animator.SetFloat("Y", animVec.z);
        _animator.SetFloat("Rot", xrot);

        if (_aimTarget) {
            float yRot = Input.GetAxis("Mouse Y");
            _aimheight = Mathf.Clamp(_aimheight + yRot * Time.smoothDeltaTime * _heightCameraSpeed, _minAimHeight,
                _maxAimHeight);
            _aimTarget.localPosition = new Vector3(_aimTarget.localPosition.x, _aimheight, _aimTarget.localPosition.z);

        }

    }
    
    private void ManagerIsGrounded() {
        bool isground =Physics.CheckSphere(_groundChecker.position, _groundCheckerRadius, _groundMask);
        _isGrounded = isground;
    }
}
