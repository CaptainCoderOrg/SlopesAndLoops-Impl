using UnityEngine;

public class LoopToggler : MonoBehaviour
{
    public EdgeCollider2D First;
    public EdgeCollider2D Second;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        First.enabled = !First.enabled;
        Second.enabled = !Second.enabled;
    }

}
