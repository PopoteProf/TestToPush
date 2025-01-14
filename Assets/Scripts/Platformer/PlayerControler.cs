using System;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{

    public bool PlayerHaveController = true;
    public CharacterController Controller;
    public float MoveSpeed;
    public float SpriteSpeed;

    public float Gravity = -9.81f;
    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    public float JumpHeight = 1f;
    [SerializeField] private Animator _animator;

    [Space(5), Header("AutoClimb")] 
    [SerializeField] private Transform _climbRayCaster;
    [SerializeField] private float _climRayDistance= 0.5f;
    [SerializeField] private float _climbDistance= 0.3f;
    [SerializeField] private float _climbTime= 1f;
    [SerializeField] private float _climbTargetOffSet= 1f;
    [SerializeField] private AnimationCurve _climbYCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space(5), Header("AutoHighClimb")]
    [SerializeField] private float _highClimbTime= 1f;
    [SerializeField] private float _highClimbThreshold = 1;
    [SerializeField] private AnimationCurve _highClimbYCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private AnimationCurve _highClimbXZCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space(5), Header("DialogueSettings")]
    [SerializeField] private float _dialogueSetupMoveSpeed = 2;
    [SerializeField] private AnimationCurve _dialogueSetupAnimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    public Transform DialoguePlayerCamTransform;
    
    private bool _doAutoClimb;
    private bool _doAutohighClimb;
    private Vector3 _autoClimbStart;
    private Vector3 _autoClimbTarget;
    private float _autoclimbTimer;
    private bool _doDialogueSetUp;
    private float _dialogueSetupTime;
    private Vector3 _lookAtPos;

    public event EventHandler OnJump;
    public event EventHandler OnCimb;
    public event EventHandler OnLanding;
    


    // Est vrai si le player touche le sol
    protected bool _isGRounded;

    // La vitesse vertical du joueur
    protected Vector3 _velocity;

    // La direction de movement du joueur
    protected Vector3 _moveVector;

    protected virtual void Update()
    {
        
        if(_doDialogueSetUp)ManageADialogueSetup();
        if (!PlayerHaveController) return;
        if (_doAutoClimb) {
            ManageAutoClimb();
            return;
        }

        if (_doAutohighClimb) {
            ManageAutoHighClimb();
            return;
        }
        ManagerIsGrounded();
        ManageMovement();
        ManageAutoClimbDetection();
        
    }

    private void ManagerIsGrounded()
    {
        bool isground =Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if( !_isGRounded&& isground) OnLanding?.Invoke(this , EventArgs.Empty);
        _isGRounded = isground;
    }
    
    protected virtual void ManageMovement() {
        _moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
       

        if (_isGRounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump")&&_isGRounded ) {
            _velocity.y = JumpHeight;
            Debug.Log("Jump" );
            if(_animator)_animator.SetTrigger("Jump");
            OnJump?.Invoke(this , EventArgs.Empty);
        }

        _velocity.y += Gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift)) {
            Controller.Move(SpriteSpeed * Time.deltaTime * _moveVector);
        }
        else {
            Controller.Move(MoveSpeed * Time.deltaTime * _moveVector);
        }

        if(_animator)_animator.SetFloat("Speed", Controller.velocity.magnitude);
        
       if (Controller.velocity.magnitude > 0.2) {
           Vector3 forward = Controller.velocity;
           forward.y = 0;
           transform.forward = forward;
       }

       Controller.Move(_velocity * Time.deltaTime);
        
       if(_animator)_animator.SetBool("IsGrounded", _isGRounded);
    }

    private void ManageAutoClimbDetection() {
        Debug.DrawLine(_climbRayCaster.position, _climbRayCaster.position + _climbRayCaster.forward * _climRayDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(_climbRayCaster.position, _climbRayCaster.forward, out hit, _climRayDistance, GroundMask)) {
            RaycastHit hit2;
            Vector3 newRayCater = hit.point + new Vector3(0, 2, 0) + _climbRayCaster.forward*_climbDistance;
            if (Physics.Raycast(newRayCater, -transform.up, out hit2, 5,
                GroundMask)) {
                Debug.DrawLine(newRayCater, hit2.point, Color.yellow);
                if (!Physics.CheckCapsule(hit2.point + new Vector3(0, 0.6f, 0), hit2.point + new Vector3(0, 1.4f, 0), 0.2f)) {
                    Debug.DrawLine(hit2.point +_climbRayCaster.forward*0.2f,hit2.point +_climbRayCaster.forward*-0.2f, Color.green);
                    
                    
                    if (Physics.Raycast(
                        _climbRayCaster.position + new Vector3(0, 1, 0),
                        hit2.point + new Vector3(0, 1, 0) - _climbRayCaster.position + new Vector3(0, 1, 0),
                        (hit2.point + new Vector3(0, 1, 0) - _climbRayCaster.position + new Vector3(0, 1, 0))
                        .magnitude))
                        return;

                    if (hit2.point.y - transform.position.y > _highClimbThreshold) {
                        _doAutohighClimb = true;
                        if(_animator)_animator.SetBool("DoHighClimb", true);
                        OnCimb?.Invoke(this , EventArgs.Empty);
                    }
                    else
                    {
                        if(_animator)_animator.SetBool("DoClimbStep", true);
                        OnJump?.Invoke(this , EventArgs.Empty);
                        _doAutoClimb = true;
                    }

                    _velocity.y = 0;
                    _autoClimbStart = transform.position;
                    _autoClimbTarget = hit2.point + new Vector3(0, _climbTargetOffSet, 0);
                    _autoclimbTimer = 0;
                    
                    
                }
            }
            
        }
    }

    private void ManageAutoClimb() {
        _autoclimbTimer += Time.deltaTime;
        Vector3 pos =  Vector3.Lerp(_autoClimbStart, _autoClimbTarget, _autoclimbTimer / _climbTime);
        pos.y = Mathf.Lerp(_autoClimbStart.y, _autoClimbTarget.y, _climbYCurve.Evaluate(_autoclimbTimer / _climbTime));
        transform.position = pos;
        
        if (_autoclimbTimer >= _climbTime) {
            _doAutoClimb = false;
            if(_animator)_animator.SetBool("DoClimbStep", false);
        }
    }
    private void ManageAutoHighClimb() {
        _autoclimbTimer += Time.deltaTime;
        Vector3 pos =  Vector3.Lerp(_autoClimbStart, _autoClimbTarget, _highClimbXZCurve.Evaluate(_autoclimbTimer / _highClimbTime));
        pos.y = Mathf.Lerp(_autoClimbStart.y, _autoClimbTarget.y, _highClimbYCurve.Evaluate(_autoclimbTimer / _highClimbTime));
        transform.position = pos;
        
        if (_autoclimbTimer >= _highClimbTime) {
            _doAutohighClimb = false;
            if(_animator)_animator.SetBool("DoHighClimb", false);
        }
    }
    
    private void ManageADialogueSetup() {
        _autoclimbTimer += Time.deltaTime;
        transform.position =  Vector3.Lerp(_autoClimbStart, _autoClimbTarget, _dialogueSetupAnimationCurve.Evaluate(_autoclimbTimer / _dialogueSetupTime));
        Vector3 forward = _lookAtPos - transform.position;
        forward.y = 0;
        transform.forward = forward;
        if(_animator)_animator.SetFloat("Speed", _dialogueSetupMoveSpeed);
        if (_autoclimbTimer >= _dialogueSetupTime) {
            if(_animator)_animator.SetFloat("Speed",0);
            _doDialogueSetUp = false;
        }
    }

    public void SetUpForDialogue(Vector3 posTarget, float time, Vector3 lookAtPos) {
        PlayerHaveController = false;
        _doDialogueSetUp = true;
        _autoClimbTarget = posTarget;
        _autoClimbStart = transform.position;
        _dialogueSetupTime = time;
        _lookAtPos = lookAtPos;
        _autoclimbTimer = 0;
    }

    public void SetPlayerIsTalking(bool value) {
        if(_animator)_animator.SetBool("IsTalking" ,value);
    }

    public void EndDialogue()
    {
        PlayerHaveController = true;
    }

    public void Respawn()
    {
        PlayerHaveController = false;
        Controller.transform.position = PlatformerCheckPoint.RESPAWNPOINT.position;
        Invoke("EndDialogue",1);
        //transform.forward = PlatformerCheckPoint.RESPAWNPOINT.forward;
    }
}