using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPosition : MonoBehaviour
{
    public GameObject Target;
    public Vector3 Offset;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Target.transform.position + Offset;
        gameObject.transform.LookAt(Target.transform.position);
    }
}
