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
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Sign(_moveForce) != Mathf.Sign(horizontal))
        {
            GroundedInfo.Clear();
        }
        _moveForce = horizontal;
        Velocity = _rigidBody.velocity;
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, GroundedInfo.Up));
    }

    void FixedUpdate()
    {
        _rigidBody.AddForce(_moveForce * AccelerationFactor * Time.fixedDeltaTime * -GroundedInfo.Left);
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, MaxVelocity);
    }
}
