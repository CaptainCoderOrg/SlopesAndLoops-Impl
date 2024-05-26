using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    public float JumpPower = 4500;
    private SlopedGroundController _slopeInfo;
    private Rigidbody2D _rigidBody;
    private bool _isJumping;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _slopeInfo = GetComponent<SlopedGroundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
        }
    }

    void FixedUpdate()
    {
        if (_isJumping && _slopeInfo.IsGrounded)
        {
            _slopeInfo.IsGrounded = false;
            _rigidBody.AddForce(_slopeInfo.Up * JumpPower);
            _isJumping = false;
        }        
    }
}
