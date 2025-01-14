using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class CarController : MonoBehaviour
{
    public bool CanControl;
    public bool IsAccelerate;
    public float OnStopDrag;
    public Rigidbody RB;
    [Header("Control")] 
    public float AccelerationPower = 10;
    public float MaxSpeed = 10;
    public AnimationCurve AccelerationSpeedCurve;
    public float RotationPower = 10;
    public float DragPower = 2;
    public float UpPower = 10;
    public AnimationCurve RotationContactFactor;
    public AnimationCurve RotationPoweroverSpeedCurve;
    public float slideCompensationPower = 2;
    [Header("SuspentionParte")] public AnimationCurve SuspentionMultiplyerCurve;
    public float SuspentionPower = 2;
    public float SuspentionMaxLenght = 1;
    public Transform[] Suspentions;
    public Transform[] FrontWeels;
    public float Gravity = 1;
    [Header("Weels")]
    public Transform[] Weels;
    public float WeelyOffset;
    [Header("PSWeels")] public ParticleSystem[] PSWeels;
    public float MinSpeedPSOn = 1;

    [Space(5)]
    public int WeelContact;

    [Header("Audio")] public AudioSource AsMotorSound;
    public AnimationCurve PitchOverSpeedCurve;


    public bool IsSlowerThan(float value) => RB.linearVelocity.magnitude < value;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float rot =0;
        float accel = 0;
        
        
        if (CanControl) {
            rot = Input.GetAxisRaw("Horizontal");
            accel = (WeelContact / 4f) * Input.GetAxisRaw("Vertical");
            
            IsAccelerate = Input.GetAxisRaw("Vertical")>0.2f;
        }

        WeelContact= 0;
        RaycastHit hit;
        Ray ray;
        for (int i = 0; i < Suspentions.Length; i++) 
        {
            Transform suspention = Suspentions[i];
            ray = new Ray(suspention.position, suspention.forward);
            Debug.DrawLine(suspention.position, suspention.position+suspention.forward*SuspentionMaxLenght,Color.green);
            if (Physics.Raycast(ray, out hit, SuspentionMaxLenght)) {
                WeelContact++;
                Debug.DrawLine(suspention.position, hit.point,Color.red);
                float forcePower = 1-(hit.distance / SuspentionMaxLenght);
                Vector3 force = -suspention.forward * (forcePower * SuspentionPower*AccelerationSpeedCurve.Evaluate(forcePower));
                RB.AddForceAtPosition(force, suspention.position);

                if (FrontWeels.Contains(suspention))
                {

                    float rotSpeedFactor = RotationPoweroverSpeedCurve.Evaluate(RB.linearVelocity.magnitude / MaxSpeed);
                    RB.AddForceAtPosition(rot*RotationPower*suspention.right *RotationContactFactor.Evaluate(forcePower)*rotSpeedFactor, suspention.position);
                }

                Weels[i].transform.position = hit.point-suspention.forward*WeelyOffset;
                
                if (RB.linearVelocity.magnitude > MinSpeedPSOn) PSWeels[i].Play();
                else PSWeels[i].Stop();
            }
            else
            {
                PSWeels[i].Stop();
                Weels[i].transform.position =suspention.position+suspention.forward*SuspentionMaxLenght-suspention.forward*WeelyOffset;
            }
            
        }

        
        accel *= AccelerationSpeedCurve.Evaluate(Mathf.Clamp01(RB.linearVelocity.magnitude / MaxSpeed));
        

        if (WeelContact == 0) {
            Quaternion testqua = new Quaternion();
            testqua.SetFromToRotation(transform.up,  Vector3.up);
            testqua.ToAngleAxis(out float angle , out Vector3 axis);
            RB.AddTorque((angle*Mathf.Deg2Rad)*axis*UpPower);
        }


        if (CanControl) RB.linearDamping = WeelContact / 4f * DragPower;
        else RB.linearDamping = OnStopDrag;
        RB.AddForce(transform.forward*accel * AccelerationPower);

        //float slideComensation = Vector3.Dot(RB.velocity.normalized, transform.forward);
        //Debug.Log(slideComensation);
        //Debug.DrawLine(transform.position, transform.position+transform.up*slideComensation, Color.yellow);
        //float slideComensationLateral = Vector3.Dot(RB.velocity.normalized, transform.right);
        //Debug.Log(slideComensationLateral);
        //Debug.DrawLine(transform.position, transform.position+transform.right*slideComensationLateral, Color.red);
        //RB.AddForce(transform.right*slideComensation*slideCompensationPower);
        //RB.AddTorque(transform.up*rot*RotationPower);

        RB.AddForce(Vector3.down*Gravity);
        //Debug.Log(RB.velocity.magnitude);

        if (CanControl) {
            float value = PitchOverSpeedCurve.Evaluate(RB.linearVelocity.magnitude / MaxSpeed);
            if (WeelContact == 0) value += 0.4f;
            AsMotorSound.pitch = value;
           
        }
        else
        {
            
        }
    }
}
