using UnityEngine;

public class GroundedInfo : MonoBehaviour
{
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _snapDistance = 1;
    public bool IsGrounded = true;
    public Vector2 Up { get; private set; } = Vector2.up;
    public Vector2 Left { get; private set; } = Vector2.left;
    public float Direction = -1;
    public float FallThreshold = 2;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    public void ClearGround()
    {
        Up = Vector2.up;
        Left = Vector2.left;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody.velocity.magnitude < FallThreshold) { ClearGround(); }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, _snapDistance, _groundLayer);
        IsGrounded = hit.collider != null;
        SnapTo(hit.collider);
        if (IsGrounded)
        {
            Debug.DrawLine(transform.position, hit.point, Color.blue, .25f);
            SetPlayerUp(hit.normal);
        }
        // Debug.DrawRay(transform.position, -Up * hit.distance, Color.blue, .25f);
        Debug.DrawRay(transform.position, Up, Color.cyan, .25f);
        Debug.DrawRay(transform.position, _rigidbody.velocity, Color.green);
    }

    private void SetPlayerUp(Vector2 newUp)
    {
        const float rotateThreshold = 80;
        if (Up == newUp || Vector2.Angle(Up, newUp) > rotateThreshold) { return; }
        Up = newUp;
        Left = Quaternion.Euler(0, 0, 90) * Up;
        _rigidbody.velocity = _rigidbody.velocity.magnitude * Direction * -Left;
    }
    
    private void SnapTo(Collider2D ground)
    {
        if (ground == null) { ClearGround(); }
        else
        {
            ColliderDistance2D distance = ground.Distance(_collider);
            if (distance.isOverlapped) { return; }
            Debug.DrawLine(distance.pointA, distance.pointB, Color.red, 1);
            transform.position += (Vector3)(distance.normal * distance.distance);
        }
    }

}
