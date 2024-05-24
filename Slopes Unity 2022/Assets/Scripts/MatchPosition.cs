using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPosition : MonoBehaviour
{
    public Vector3 Offset;
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.position + Offset;
    }
}
