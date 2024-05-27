using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float AccelerationFactor = 500;
    public float RotationSpeed = 30;
    private Rigidbody _rigidbody;
    private float _movementInput;
    private float _rotationInput;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        _rigidbody.AddForce(_movementInput * AccelerationFactor * Time.fixedDeltaTime * transform.forward);
    }
}
