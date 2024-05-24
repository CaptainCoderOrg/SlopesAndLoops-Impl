using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundedInfo : MonoBehaviour
{
    public LayerMask GroundLayer;
    public float Distance = 1;
    public bool IsGrounded = true;
    public Vector2 Up { get; private set; } = Vector2.up;
    public Vector2 Left { get; private set; } = Vector2.left;
    public float Direction = -1;
    private Vector2 _previousUp = Vector2.up;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;

    public void Clear()
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
        _previousUp = Up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Up, Distance, GroundLayer);
        Debug.DrawRay(transform.position, -Up * hit.distance, Color.blue, 1);
        if (hit.collider != null)
        {
            Up = hit.normal;
            Left = Quaternion.Euler(0, 0, 90) * Up;
            ColliderDistance2D distance = hit.collider.Distance(_collider);
            SnapToGround(distance);
        }
        else
        {
            Clear();
            IsGrounded = false;
        }
        Debug.DrawRay(transform.position, Up, Color.cyan, 1);
        Debug.DrawRay(transform.position, _rigidbody.velocity, Color.green);
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
            _rigidbody.velocity = _rigidbody.velocity.magnitude * Direction * -Left;
            return true;
        }
        return false;
    }

}
