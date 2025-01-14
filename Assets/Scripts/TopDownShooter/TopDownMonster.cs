using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Activator))]
public class TopDownMonster : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _monsterHP = 3;

    [Space(5), Header("LinkSetup")] [SerializeField]
    private float _linkSetupMaxRotion = 30;

    [SerializeField] private float _jumpSpeed = 1;
    [SerializeField] private float _linkYModifier = 3;

    [Space(5), Header("LinkSetup")] [SerializeField]
    private float _attackDistance;

    [SerializeField] private float _attackLayerTransitionTime = 0.5f;

    [SerializeField] private Rigidbody[] _ragdolRigidbody;
    [SerializeField] private float _attackDamage = 2;

    private Collider _collider;
    private NavMeshAgent _navMeshAgent;
    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;



    [SerializeField] private Transform _target;

    private float _linkSpeed;
    private bool _doLink;
    private bool _doLinkSetup;
    private Vector3 _linkStartPos;
    private float _timer;

    private bool _isDead;
    private bool _IsPause;


    
    public void SetTarget(Transform target) => _target = target;
    private void Start()
    {
        //_animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;

        GameStaticManager.OnPauseChange += SetPause;
        foreach (var rb in _ragdolRigidbody)
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if (_isDead||_IsPause) return;
        if (_navMeshAgent.isOnOffMeshLink)
        {
            if (!_doLinkSetup && !_doLink) StartLink(_navMeshAgent.currentOffMeshLinkData);
            if (_doLinkSetup) DoLinkSetUp(_navMeshAgent.currentOffMeshLinkData);
            if (_doLink) DoLink(_navMeshAgent.currentOffMeshLinkData);
        }
        else {
            ManageMovement();
        }
        ManageAttack();
    }

    public void ManageAttack()
    {
        if (Vector3.Distance(transform.position, _navMeshAgent.destination) < _attackDistance) {
            _animator.SetLayerWeight(1,
                Mathf.Clamp01(_animator.GetLayerWeight(1) + _attackLayerTransitionTime * Time.deltaTime));
            _animator.SetBool("Attack", true);
        }
        else
        {
            _animator.SetLayerWeight(1,
                Mathf.Clamp01(_animator.GetLayerWeight(1) * _attackLayerTransitionTime * Time.deltaTime));
            _animator.SetBool("Attack", false);

        }
    }

    public void Attack() {
        GameStaticManager.SetPlayerTakeDamageHpChange(_attackDamage);
    }

    public void TakeHit(Vector3 pointHit, Vector3 hitVector)
    {

        _monsterHP--;
        if (_monsterHP <= 0) {
            _collider.enabled = false;
            _navMeshAgent.enabled = false;
            _animator.enabled = false;
            _isDead = true;
            GameStaticManager.OnPauseChange -= SetPause;
            foreach (var rb in _ragdolRigidbody)
            {
                rb.isKinematic = false;
                rb.AddForce(hitVector, ForceMode.Impulse);
            }
        }
    }

    private void ManageMovement()
    {
        _navMeshAgent.SetDestination(_target.position);
        Vector3 worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && _navMeshAgent.remainingDistance > _navMeshAgent.radius;


        // Update animation parameters
        _animator.SetBool("move", shouldMove);
        //_animator.SetFloat ("velx", velocity.x);
        if (shouldMove)
        {
            _animator.SetFloat("vely", velocity.y, 0.3f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat("vely", 0, 0.2f, Time.deltaTime);
        }


        NavMeshHit hit;

        // Check all areas one length unit ahead.
        if (!_navMeshAgent.SamplePathPosition(NavMesh.AllAreas, 1.0F, out hit))
        {


            if ((hit.mask == 16))
            {
                _animator.SetFloat("InWater", 1, 0.2f, Time.deltaTime);
                _navMeshAgent.speed = 2.8f;
            }
            else
            {
                _animator.SetFloat("InWater", 0, 0.2f, Time.deltaTime);
                _navMeshAgent.speed = 3.5f;
            }
        }


        worldDeltaPosition.y = 0;
        transform.rotation = Quaternion.LookRotation(worldDeltaPosition.normalized);

    }

    private void DoLink(OffMeshLinkData data) {
        _timer += Time.deltaTime;
        float yMod;
        float t = _timer / _linkSpeed;
        if (_linkStartPos.y > data.endPos.y) yMod = _linkStartPos.y + _linkYModifier;
        else yMod = data.endPos.y + _linkYModifier;
        Vector3 newPos = Vector3.Lerp(_linkStartPos, data.endPos, t);
        newPos.y = Mathf.Lerp(Mathf.Lerp(_linkStartPos.y, yMod, t), Mathf.Lerp(yMod, data.endPos.y, t), t);
        transform.position = newPos;

        _animator.SetBool("Jump", true);
        _navMeshAgent.nextPosition = transform.position;
        if (_timer >= _linkSpeed)
        {
            _navMeshAgent.nextPosition = data.endPos;
            transform.position = data.endPos;
            _animator.SetBool("Jump", false);
            _navMeshAgent.CompleteOffMeshLink();
            _doLink = false;
            _navMeshAgent.updateRotation = true;
            _navMeshAgent.velocity = Vector3.zero;

        }
    }

    private void StartLink(OffMeshLinkData data) {
        _linkSpeed = Vector3.Distance(data.startPos, data.endPos) / _jumpSpeed;
        _timer = 0;
        _doLinkSetup = true;
        _linkStartPos = transform.position;
        _navMeshAgent.updateRotation = false;
        _animator.SetFloat("vely", 0);

    }

    private void DoLinkSetUp(OffMeshLinkData data) {
        Vector3 linkdir = data.endPos - data.startPos;
        linkdir.y = 0;
        linkdir.Normalize();

        if (Vector3.Dot(transform.forward, linkdir) < 0.9f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(linkdir),
                _linkSetupMaxRotion * Time.deltaTime);
            return;
        }

        _doLinkSetup = false;
        _doLink = true;
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        if (_doLink || _doLinkSetup) return;
        transform.position = _navMeshAgent.nextPosition;
    }

    public void SetPause(object sender ,bool value) {
        _IsPause = value;
        if (value) _navMeshAgent.speed = 0;
        else _navMeshAgent.speed = 3;
        
    }

    
    
}