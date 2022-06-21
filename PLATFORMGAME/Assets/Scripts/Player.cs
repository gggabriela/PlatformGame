using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    
    [SerializeField]private float _JumpForce;
    [SerializeField]private float _MoveForce;

    private Rigidbody2D _Rigidbody;
    public Rigidbody2D Rigidbody
    {
        get
        {
            if (_Rigidbody == null)
            {
                _Rigidbody = GetComponent<Rigidbody2D>();
            }
            return _Rigidbody;
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
        float torque = 0.0f;
        if (Input.GetKey((KeyCode.D)))
        {
            torque = -_MoveForce;
        }
        if (Input.GetKey((KeyCode.A)))
        {
            torque = _MoveForce;
        }

        Rigidbody.AddTorque(torque);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody.AddForce(Vector2.down * _JumpForce, ForceMode2D.Impulse);
        }
    }
}
