using System;
using UnityEngine;

public class GroundedInfo : MonoBehaviour
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
    public Vector2 Left { get; private set; } = Vector2.left;
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
        Left = Vector2.left;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _colliderHeight = _collider.bounds.extents.y;
    }

    void Update()
    {
        if (_rigidbody.velocity.magnitude < FallThreshold || Up == Vector2.down) { ClearGround(); }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, _colliderHeight + SnapDistance, _groundLayer);
        IsGrounded = hit.collider != null;
        SnapTo(hit.collider);
        if (IsGrounded) { SetPlayerUp(hit.normal); }

        // Draw Debug information in Scene View
        if (ShowDebugInfo)
        {
            if (IsGrounded) { Debug.DrawLine(transform.position, hit.point, Color.blue, .25f); }
            Debug.DrawRay(transform.position, Up, Color.cyan, .25f);
            Debug.DrawRay(transform.position, _rigidbody.velocity * .5f, Color.green);
        }
    }

    int SignOrZero(float num) => num == 0 ? 0 : num > 0 ? 1 : -1;
    private void SetPlayerUp(Vector2 newUp)
    {
        const float rotateThreshold = 80;
        // Don't change the rotation on sharp corners (let's the player launch into the air)
        if (Up == newUp || Vector2.Angle(Up, newUp) > rotateThreshold) { return; }
        Up = newUp;
        Left = Quaternion.Euler(0, 0, 90) * Up;

        float momentum = SignOrZero(_rigidbody.velocity.x);
        if (TargetMomentum == momentum)
        {
            // Take the current velocity and snap it to the new rotation
            _rigidbody.velocity = _rigidbody.velocity.magnitude * TargetMomentum * -Left;
        }

        // Update the player character's rotation to match its new "Up"
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, Up));
    }

    private void SnapTo(Collider2D ground)
    {
        if (ground == null) { ClearGround(); }
        else
        {
            ColliderDistance2D distance = ground.Distance(_collider);
            if (distance.isOverlapped) { return; }
            transform.position += (Vector3)(distance.normal * distance.distance);
            if (ShowDebugInfo) { Debug.DrawLine(distance.pointA, distance.pointB, Color.red, 1); }
        }
    }
}
