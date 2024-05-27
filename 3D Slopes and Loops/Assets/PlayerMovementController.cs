using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float AccelerationFactor = 500;
    public float RotationSpeed = 30;
    public float MaximumVelocity = 24;
    private Rigidbody _rigidbody;
    private float _movementInput;
    private float _rotationInput;
    private SlopedGroundController _slopeInfo;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _slopeInfo = GetComponent<SlopedGroundController>();
    }

    // Update is called once per frame
    void Update()
    {
        _movementInput = Input.GetAxis("Vertical");
        _rotationInput = Input.GetAxis("Horizontal");
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += _rotationInput * RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);
    }

    void FixedUpdate()
    {
        _rigidbody.AddForce(_movementInput * AccelerationFactor * Time.fixedDeltaTime * _slopeInfo.Forward);
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, MaximumVelocity);
    }
}
