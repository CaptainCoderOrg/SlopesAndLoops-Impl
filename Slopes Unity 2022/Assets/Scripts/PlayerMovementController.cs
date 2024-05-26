using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float MaxVelocity = 24;
    public float BoostVelocity = 16;
    public float AccelerationFactor = 5000;
    public float BoostFactor = 3;
    public bool IsBoosting => _rigidbody.velocity.magnitude > BoostVelocity;
    private SlopedGroundController _slopeInfo;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _slopeInfo = GetComponent<SlopedGroundController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        _slopeInfo.UpdateTargetMomentum(horizontal);
    }

    void FixedUpdate()
    {
        float acceleration = AccelerationFactor * _slopeInfo.TargetMomentum;
        acceleration *= IsBoosting ? BoostFactor : 1;
        _rigidbody.AddForce(acceleration * Time.fixedDeltaTime * _slopeInfo.Right);
        _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, MaxVelocity);
    }
}
