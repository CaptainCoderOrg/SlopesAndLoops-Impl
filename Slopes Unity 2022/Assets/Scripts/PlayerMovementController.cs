using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float MaxVelocity = 24;
    public float BoostVelocity = 16;
    public float AccelerationFactor = 5000;
    public float BoostFactor = 3;
    public bool IsBoosting => _rigidbody.velocity.magnitude > BoostVelocity;
    private GroundedInfo _groundedInfo;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundedInfo = GetComponent<GroundedInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        _groundedInfo.UpdateTargetMomentum(horizontal);
    }

    void FixedUpdate()
    {
        float acceleration = AccelerationFactor * _groundedInfo.TargetMomentum;
        acceleration *= IsBoosting ? BoostFactor : 1;
        _rigidbody.AddForce(acceleration * Time.fixedDeltaTime * -_groundedInfo.Left);
        _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, MaxVelocity);
    }
}
