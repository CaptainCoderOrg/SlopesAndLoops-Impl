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
    public float TargetMomentum = -1;
    // The player is considered upside down if their "Up" is within 15 degrees of world down
    public bool IsUpsideDown => Vector2.Angle(Up, Vector2.down) < 15;
    // The player is considered vertical if they are moving beyond a 90 degree angle 
    public bool IsVertical => Vector2.Angle(Up, Vector2.down) <= 90;
    public Vector2 Up { get; private set; } = Vector2.up;
    public Vector2 Right { get; private set; } = Vector2.right;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _colliderHeight = _collider.bounds.extents.y;
    }

    void FixedUpdate()
    {
        // If the player is If the player is not moving fast enough, they fall off walls and steep slopes
        if (IsVertical && _rigidbody.velocity.magnitude < FallThreshold) { ClearGround(); }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, _colliderHeight + SnapDistance, _groundLayer);
        // Use raycast to determine if we are touching the ground
        IsGrounded = hit.collider != null;

        // If the player is touching the ground and is not upside down, snap them to the ground
        if (IsGrounded && !IsUpsideDown) { SnapTo(hit.collider); }

        // If the player is touching the ground, adjust the player's Up direction to match the surface of the ground
        if (IsGrounded) { SetPlayerUp(hit.normal); }
        // Otherwise, clear the ground
        else { ClearGround(); }

        // Draw Debug information in Scene View
        if (ShowDebugInfo)
        {
            if (IsGrounded) { Debug.DrawLine(transform.position, hit.point, Color.blue, .25f); }
            Debug.DrawRay(transform.position, Up, Color.cyan, .25f);
            Debug.DrawRay(transform.position, _rigidbody.velocity * .5f, Color.green);
        }
    }

    public void UpdateTargetMomentum(float targetMomentum)
    {
        // If the incoming momentum is different than the previous direction, the player is either stopping or turning
        // and clear their ground connection. This allows the player to turn off walls rather than changing directions
        // when they are running up / down a wall.
        bool isStoppingOrTurning = TargetMomentum != SignOrZero(targetMomentum);
        if (isStoppingOrTurning) { ClearGround(); }
        TargetMomentum = SignOrZero(targetMomentum);
    }

    public void ClearGround()
    {
        // Reset the player's Up and Right
        Up = Vector2.up;
        Right = Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

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
        // Find the distance to the ground
        ColliderDistance2D distance = ground.Distance(_collider);
        // If we are already touching the ground, do nothing
        if (distance.isOverlapped) { return; }
        // Otherwise, move the player to the collider
        transform.position += (Vector3)(distance.normal * distance.distance);
    }

    // Helper function for determining if a float is positive, negative, or zero
    private static int SignOrZero(float num) => num == 0 ? 0 : num > 0 ? 1 : -1;
}
