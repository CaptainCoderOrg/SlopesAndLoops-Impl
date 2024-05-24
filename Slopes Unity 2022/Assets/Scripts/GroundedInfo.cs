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
    private Vector2 _previousUp = Vector2.up;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    public void ClearGround()
    {
        _previousUp = Vector2.up;
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
        if (_rigidbody.velocity.magnitude < FallThreshold)
        {
            ClearGround();
        }
        _previousUp = Up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, _snapDistance, _groundLayer);
        Debug.DrawRay(transform.position, -Up * hit.distance, Color.blue, .25f);
        if (hit.collider is Collider2D ground)
        {
            Up = hit.normal;
            Left = Quaternion.Euler(0, 0, 90) * Up;
            ColliderDistance2D distance = ground.Distance(_collider);
            if (distance.isOverlapped) { IsGrounded = true; }
            else if (Up != _previousUp) { SnapToGround(distance); }
        }
        else
        {
            ClearGround();
            IsGrounded = false;
        }
        Debug.DrawRay(transform.position, Up, Color.cyan, .25f);
        Debug.DrawRay(transform.position, _rigidbody.velocity, Color.green);
    }

    private void SnapToGround(ColliderDistance2D distance)
    {
        Debug.DrawLine(distance.pointA, distance.pointB, Color.red, 1);
        Vector3 toMove = distance.normal * distance.distance;
        transform.position = transform.position + toMove;
        _rigidbody.velocity = _rigidbody.velocity.magnitude * Direction * -Left;
    }

}
