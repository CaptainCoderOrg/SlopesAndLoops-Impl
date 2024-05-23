using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPosition : MonoBehaviour
{
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x = Target.position.x;
        position.y = Target.position.y;
        transform.position = position;
    }
}
