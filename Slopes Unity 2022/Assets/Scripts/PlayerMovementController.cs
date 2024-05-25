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

    int SignOrZero(float num) => num == 0 ? 0 : num > 0 ? 1 : -1;
    private bool IsTurningOrStopping(float horizontal) => SignOrZero(_movementInput) != SignOrZero(horizontal);

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (IsTurningOrStopping(horizontal)) { _groundedInfo.ClearGround(); }
        _movementInput = horizontal;
        _groundedInfo.UpdateMomentum(_movementInput);
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
