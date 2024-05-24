using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundedInfo : MonoBehaviour
{
    public LayerMask GroundLayer;
    public float Distance = 1;
    public Collider2D PlayerCollider;
    public bool IsGrounded = true;
    public Vector2 Up = Vector2.up;
    public Vector2 Left = Vector2.left;
    private Rigidbody2D _rigidbody;

    public void Clear()
    {
        Up = Vector2.up;
        Left = Vector2.left;
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, Distance, GroundLayer);
        Debug.Log(hit.distance);
        Debug.DrawRay(transform.position, -Up * hit.distance, Color.blue, 1);
        if (hit.collider != null)
        {
            Up = hit.normal;
            Left = Quaternion.Euler(0, 0, 90) * Up;
            ColliderDistance2D distance = hit.collider.Distance(PlayerCollider);
            SnapToGround(distance);
        }
        else
        {
            Up = Vector2.up;
            Left = Vector2.left;
            IsGrounded = false;
        }
        Debug.DrawRay(transform.position, Up, Color.cyan, 1);
        Debug.DrawRay(transform.position, Left, Color.green, 1);
    }

    private bool SnapToGround(ColliderDistance2D distance)
    {
        if (distance.isOverlapped)
        {
            IsGrounded = true;
            return true;
        }
        else if (IsGrounded)
        {
            Debug.DrawLine(distance.pointA, distance.pointB, Color.red, 1);
            Vector3 toMove = distance.normal * distance.distance;
            transform.position = transform.position + toMove;
            return true;
        }
        return false;
    }

}
