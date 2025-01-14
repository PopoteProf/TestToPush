using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarControler : MonoBehaviour
{

    [SerializeField] private float _accelerationPower = 10f;
    [SerializeField] private float _rotationPower = 10f;
    
    private Rigidbody _rigidbody;
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    void Update() {
        float acceleration =Input.GetAxisRaw("Vertical");
        float rotation = Input.GetAxisRaw("Horizontal");
        
        _rigidbody.AddForce(acceleration*_accelerationPower*Time.deltaTime*transform.forward);
        _rigidbody.AddTorque(transform.up *rotation*_rotationPower*Time.deltaTime);
    }
}
