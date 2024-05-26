using System;
using UnityEngine;

public class SlopedGroundController : MonoBehaviour
{
    public bool ShowDebugInfo = true;
    [SerializeField]
    private LayerMask _groundLayer;
    [field: SerializeField]
    public float SnapDistance { get; set; } = 0.25f;
    private float _colliderHeight;
    public bool IsGrounded = true;
    public float FallThreshold = 2;
    public Vector2 Up { get; private set; } = Vector2.up;
    public Vector2 Right { get; private set; } = Vector2.right;
    public float TargetMomentum = -1;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;

    public void UpdateTargetMomentum(float targetMomentum)
    {
        bool isStoppingOrTurning = SignOrZero(TargetMomentum) != SignOrZero(targetMomentum);
        if (isStoppingOrTurning) { ClearGround(); }
        TargetMomentum = SignOrZero(targetMomentum);
    }

    public void ClearGround()
    {
        Up = Vector2.up;
        Right = Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _colliderHeight = _collider.bounds.extents.y;
    }

    void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude < FallThreshold) { ClearGround(); }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, _colliderHeight + SnapDistance, _groundLayer);
        if (!IsUpsideDown) { SnapTo(hit.collider); }
        IsGrounded = hit.collider != null;        
        if (IsGrounded) { SetPlayerUp(hit.normal); }
        else { ClearGround(); }

        // Draw Debug information in Scene View
        if (ShowDebugInfo)
        {
            if (IsGrounded) { Debug.DrawLine(transform.position, hit.point, Color.blue, .25f); }
            Debug.DrawRay(transform.position, Up, Color.cyan, .25f);
            Debug.DrawRay(transform.position, _rigidbody.velocity * .5f, Color.green);
        }
    }
    private bool IsUpsideDown => Vector2.Angle(Up, Vector2.down) < 15;
    private int SignOrZero(float num) => num == 0 ? 0 : num > 0 ? 1 : -1;
    private void SetPlayerUp(Vector2 newUp)
    {
        const float rotateThreshold = 80;
        // Don't change the rotation on sharp corners (let's the player launch into the air)
        if (Up == newUp || Vector2.Angle(Up, newUp) > rotateThreshold) { return; }

        // Sets the player's Up and Right directions, and rotates the player to match
        Up = newUp;
        Right = Quaternion.Euler(0, 0, -90) * Up;
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, Up));

        if (TargetMomentum == SignOrZero(_rigidbody.velocity.x))
        {
            // If the target momentum is the same as the direction the player is actually moving,
            // snap the player's current velocity to match their direction of travel.
            _rigidbody.velocity = _rigidbody.velocity.magnitude * TargetMomentum * Right;
        }
    }

    private void SnapTo(Collider2D ground)
    {
        // If there is no ground, clear the ground
        if (ground == null) { ClearGround(); }
        // If we are upside down, do not snap
        else if (IsUpsideDown) { return; }
        else
        {
            // Find the distance to the ground
            ColliderDistance2D distance = ground.Distance(_collider);
            // If we are already touching the ground, do nothing
            if (distance.isOverlapped) { return; }
            // Otherwise, move the player to the collider
            transform.position += (Vector3)(distance.normal * distance.distance);
            if (ShowDebugInfo) { Debug.DrawLine(distance.pointA, distance.pointB, Color.red, 1); }
        }
    }
}
