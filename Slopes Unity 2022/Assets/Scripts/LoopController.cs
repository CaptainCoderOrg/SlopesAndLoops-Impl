using UnityEngine;
public class LoopController : MonoBehaviour
{
    public EdgeCollider2D LeftRamp;
    public EdgeCollider2D RightRamp;
    public EdgeCollider2D TopRamp;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (TopRamp.Distance(collider).normal.x < 0)
        {
            LeftRamp.enabled = true;
            RightRamp.enabled = false;
        }
        else
        {
            RightRamp.enabled = true;
            LeftRamp.enabled = false;
        }
    }

}
