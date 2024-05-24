using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float MaxVelocity = 24;
    public float BoostVelocity = 16;
    public float AccelerationFactor = 5000;
    private GroundedInfo _groundedInfo;
    private float _movementInput = 0;
    private Rigidbody2D _rigidBody;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _groundedInfo = GetComponent<GroundedInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Sign(_movementInput) != Mathf.Sign(horizontal))
        {
            _groundedInfo.Clear();
        }
        _movementInput = horizontal;
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, _groundedInfo.Up));
    }

    void FixedUpdate()
    {
        float boost = 1;
        if (_rigidBody.velocity.magnitude > BoostVelocity)
        {
            boost = 2;
        }
        _rigidBody.AddForce(_movementInput * AccelerationFactor * boost * Time.fixedDeltaTime * -_groundedInfo.Left);
        _rigidBody.velocity = Vector2.ClampMagnitude(_rigidBody.velocity, MaxVelocity);
    }
}
