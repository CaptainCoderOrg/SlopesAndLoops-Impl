using UnityEngine;
namespace CaptainCoder.SlopesAndLoops
{
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

        void Update()
        {
            if (Input.GetButtonDown("Jump") && _slopeInfo.IsGrounded)
            {
                _isJumping = true;
            }
        }

        void FixedUpdate()
        {
            if (_isJumping)
            {
                _slopeInfo.IsGrounded = false;
                _isJumping = false;
                _rigidBody.AddForce(_slopeInfo.Up * JumpPower);
            }
        }
    }
}