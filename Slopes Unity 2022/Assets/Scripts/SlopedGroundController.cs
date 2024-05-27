using UnityEngine;

public class SlopedGroundController : MonoBehaviour
{
    ///<summary>Layers that are considered ground</summary>
    [SerializeField]
    private LayerMask _groundLayer;
    ///<summary>Shows debug information in the Scene View</summary>
    public bool ShowDebugInfo = true;
    ///<summary>The maximum distance the player must be from the ground to be snapped</summary>
    [field: SerializeField]
    public float SnapDistance { get; set; } = 0.15f;
    ///<summary>True if the player is touching the ground and false otherwise</summary>
    public bool IsGrounded = true;
    ///<summary>The minimum velocity required to run on a vertical wall</summary>
    public float FallThreshold = 4;
    ///<summary>The desired direction of movement: -1 is left, 1 is right, 0 is no direction</summary>
    public int TargetMomentum = -1;
    ///<summary>The player is considered upside down if their "Up" is within 15 degrees of world down</summary>
    public bool IsUpsideDown => Vector2.Angle(Up, Vector2.down) < 15;
    ///<summary>The player is considered vertical if they are moving beyond a 90 degree angle </summary>
    public bool IsVertical => Vector2.Angle(Up, Vector2.down) <= 90;
    ///<summary>The player's current "Up" direction</summary>
    public Vector2 Up { get; private set; } = Vector2.up;
    ///<summary>The player's current "Right" direction (-90 degrees from Up)</summary>
    public Vector2 Right { get; private set; } = Vector2.right;
    ///<summary>The distance from the center of the object to the bottom of the collider</summary>
    private float _colliderExtents;
    ///<summary>A cached instance of the current Collider</summary>
    private Collider2D _collider;
    ///<summary>A cached instance of the current rigidbody</summary>
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _colliderExtents = _collider.bounds.extents.y;
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, _colliderExtents + SnapDistance, _groundLayer);
        // Use raycast to determine if we are touching the ground
        IsGrounded = hit.collider != null;

        // If the player is touching the ground and is not upside down, snap them to the ground
        if (IsGrounded && !IsUpsideDown) { SnapTo(hit.collider); }

        // If the player is touching the ground, adjust the player's Up direction to match the surface of the ground
        if (IsGrounded) { SetPlayerUp(hit.normal); }
        // Otherwise, clear the ground
        else { ClearGround(); }

        // If the player is running up a wall (or upside down) and they are moving too slow, they fall.
        if (IsGrounded && IsVertical && _rigidbody.velocity.magnitude < FallThreshold) { ClearGround(); }

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

    private void ClearGround()
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
        if (ShowDebugInfo)
        {
            Debug.DrawLine(distance.pointA, distance.pointB, Color.red, 1);
        }
    }

    // Helper function for determining if a float is positive, negative, or zero
    private static int SignOrZero(float num) => num == 0 ? 0 : num > 0 ? 1 : -1;
}
