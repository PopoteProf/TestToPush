using System;
using UnityEngine;
using UnityEngine.Rendering;

public class TopDownPlayerProjectile : MonoBehaviour {
    [SerializeField] public bool _isFlying = true;
    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private LayerMask _collisionLayerMask;
    [SerializeField] private GameObject _prefabImpact;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _physicsImpacetPower =1;
    [SerializeField] private GameObject _prefabsPsBlood;
    private Vector3 _lastPos;
    private float _yVelocity;

    public void Start() {
        _lastPos = transform.position;
    }

    private void Update() {
        if (!_isFlying) return;
        
        _yVelocity += _gravity * Time.deltaTime;
        transform.position += _moveSpeed * Time.deltaTime * transform.forward+new Vector3(0,_yVelocity, 0);
        
        if (_lastPos != Vector3.zero) {
            transform.forward = transform.position - _lastPos;
            RaycastHit[] hits;

            Vector3 offset = (_lastPos - transform.position).normalized;
            hits =Physics.RaycastAll(new Ray(_lastPos+offset, transform.position - _lastPos),
                Vector3.Distance(_lastPos, transform.position), _collisionLayerMask);

            if (hits.Length != 0) {
                foreach (var hit in hits) {
                    if (hit.collider.CompareTag("Monster")) {
                        hit.collider.GetComponent<TopDownMonster>().TakeHit(hit.point, transform.forward*_physicsImpacetPower);
                        GameObject ps = Instantiate(_prefabsPsBlood, hit.point, Quaternion.identity);
                        ps.transform.forward = hit.normal;
                        Destroy(gameObject);
                        _isFlying = false;
                        return;
                    } 
                }

                foreach (var hit in hits)
                {
                    if (hit.transform.CompareTag("Player")) continue;
                    GameObject ps = Instantiate(_prefabImpact, hits[0].point, Quaternion.identity);
                    ps.transform.forward = hits[0].normal;
                    Destroy(gameObject);
                    _isFlying = false;
                }
               
            }
            
        }
        _lastPos = transform.position;
    }
    
    
}