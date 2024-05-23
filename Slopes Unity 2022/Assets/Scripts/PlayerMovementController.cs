using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public GroundedInfo GroundedInfo;
    public float MaxVelocity = 5;
    public float AccelerationFactor = 1000;
    private float _moveForce = 0;
    private Rigidbody2D _rigidBody;
    public Vector2 Velocity;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveForce = Input.GetAxis("Horizontal");        
        Velocity = _rigidBody.velocity;
    }

    void FixedUpdate()
    {
        _rigidBody.AddForce(_moveForce * AccelerationFactor * Time.fixedDeltaTime * -Vector2.left);
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, MaxVelocity);
        if (GroundedInfo.IsGrounded)
        {
            _rigidBody.AddTorque(-_moveForce * AccelerationFactor * Time.fixedDeltaTime);
            _rigidBody.angularVelocity = Mathf.Clamp(_rigidBody.angularVelocity, -360*5, 360*5);
        }
    }
}
