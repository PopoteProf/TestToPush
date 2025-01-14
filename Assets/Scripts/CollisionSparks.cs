using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollisionSparks : MonoBehaviour
{
    public GameObject SparksObject;
    public float MinPowerSparks = 2;
    [Range(0,1)]public float ColisionVolume = 1;
    public AudioClip[] CollisionClips;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > MinPowerSparks)
        {
           GameObject go = Instantiate(SparksObject, collision.contacts[0].point, transform.rotation);
           Destroy(go, 2);
        }
    }
}
