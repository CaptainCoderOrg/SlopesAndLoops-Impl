using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    public float JumpPower = 500;
    private GroundedInfo _groundedInfo;
    private Rigidbody2D _rigidBody;
    private bool _isJumping;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _groundedInfo = GetComponent<GroundedInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && _groundedInfo.IsGrounded)
        {
            _isJumping = true;
        }
    }

    void FixedUpdate()
    {
        if (_isJumping)
        {
            _groundedInfo.IsGrounded = false;
            _rigidBody.AddForce(_groundedInfo.Up * JumpPower);
            _isJumping = false;
        }        
    }
}
