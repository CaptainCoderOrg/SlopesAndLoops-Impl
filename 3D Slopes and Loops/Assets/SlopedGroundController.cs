using Unity.VisualScripting;
using UnityEngine;

public class SlopedGroundController : MonoBehaviour
{
    public LayerMask GroundLayers;
    public float SnapDistance = 0.15f;
    public bool ShowDebugInfo = true;
    public bool IsGrounded = false;
    public Vector3 Up { get; private set; } = Vector3.up;
    public Vector3 Forward { get; private set; } = Vector3.forward;
    public GameObject PlayerModel;
    private Rigidbody _rigidbody;
    private float _colliderExtents; 
    
    private void ClearGround()
    {
        Up = Vector3.up;
        Forward = gameObject.transform.forward;
        PlayerModel.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliderExtents = GetComponentInChildren<Collider>().bounds.extents.y;
        ClearGround();
    }

    void FixedUpdate()
    {
        Vector3 debugUp = -Up;
        if(Physics.Raycast(transform.position, -Up, out RaycastHit hit, _colliderExtents + SnapDistance, GroundLayers))
        {
            IsGrounded = true;
            SetPlayerUp(hit.normal);
        }
        if (ShowDebugInfo)
        {
            Debug.DrawRay(transform.position, debugUp * (_colliderExtents + SnapDistance), Color.magenta, .25f);
            if (IsGrounded) { Debug.DrawLine(transform.position, hit.point, Color.blue, .25f); }
            Debug.DrawRay(transform.position, Up, Color.cyan, .25f);
            Debug.DrawRay(transform.position, Forward, Color.green, .25f);
            Debug.DrawRay(transform.position, _rigidbody.velocity, Color.green);
        }
    }

    private void SetPlayerUp(Vector3 newUp)
    {
        Up = newUp;
        // TODO: Rotate forward
        PlayerModel.transform.rotation = Quaternion.Euler(Vector3.SignedAngle(Vector3.up, Up, PlayerModel.transform.position), 0, 0);
    }
}
