using Unity.VisualScripting;
using UnityEngine;

public class SlopedGroundController : MonoBehaviour
{
    public LayerMask GroundLayers;
    public float SnapDistance = 0.15f;
    public bool ShowDebugInfo = true;
    public bool IsGrounded = false;
    public Vector3 Up { get; private set; } = Vector3.up;
    public Vector3 Forward => Vector3.RotateTowards(Up, CameraForward, 90 * Mathf.Deg2Rad, float.MaxValue);
    public Vector3 Right => Vector3.Cross(Up, Forward);
    public Vector3 CameraForward = Vector3.forward;
    public GameObject PlayerModel;
    private Rigidbody _rigidbody;
    private float _colliderExtents; 
    
    private void ClearGround()
    {
        Up = Vector3.up;
    }
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliderExtents = GetComponentInChildren<Collider>().bounds.extents.y;
        ClearGround();
    }

    void FixedUpdate()
    {
        IsGrounded = Physics.Raycast(transform.position, -Up, out RaycastHit hit, _colliderExtents + SnapDistance, GroundLayers);
        if(IsGrounded) 
        { 
            SetPlayerUp(hit.normal); 
        
        }
        else { ClearGround(); }
        if (ShowDebugInfo)
        {
            if (IsGrounded) { Debug.DrawLine(transform.position, hit.point, Color.blue, .25f); }
            Debug.DrawRay(transform.position, Up, Color.cyan, .25f);
            Debug.DrawRay(transform.position, _rigidbody.velocity, Color.green, .25f);
            Debug.DrawRay(transform.position, Right, Color.green, .25f);
        }
    }
    
    private void SetPlayerUp(Vector3 newUp)
    {
        Up = newUp;
        Matrix4x4 rotationMatrix = new Matrix4x4();
        rotationMatrix.SetColumn(0, Right);
        rotationMatrix.SetColumn(1, Up);
        rotationMatrix.SetColumn(2, Forward);
        rotationMatrix.SetColumn(3, new Vector4(0, 0, 0, 1));
        Quaternion targetRotation = rotationMatrix.rotation;
        PlayerModel.transform.rotation = targetRotation;
    }
}
