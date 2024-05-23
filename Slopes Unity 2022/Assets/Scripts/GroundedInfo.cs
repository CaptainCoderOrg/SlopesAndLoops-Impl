using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GroundedInfo : MonoBehaviour
{
    public LayerMask GroundLayer;
    public float Distance = 1;
    public Collider2D PlayerCollider;
    public bool IsGrounded = true;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Distance, GroundLayer);
        Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down*Distance, Color.yellow, 2);
        if (hit.collider != null)
        {
            ColliderDistance2D distance = Physics2D.Distance(hit.collider, PlayerCollider);
            CheckSnap(distance);
        }        
        else
        {
            IsGrounded = false;
        }
    }

    private bool CheckSnap(ColliderDistance2D distance)
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
