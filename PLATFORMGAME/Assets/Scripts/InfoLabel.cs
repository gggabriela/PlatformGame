using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoLabel : MonoBehaviour
{
    
    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float _MoveSpeed;

    [Range(0.0f, 10.0f)]
    [SerializeField]
    private float _MoveForce;


    void Update()
    {
        transform.position += Vector3.up * Mathf.Sin(Time.time * _MoveSpeed) * _MoveForce * Time.deltaTime;
    }
}
