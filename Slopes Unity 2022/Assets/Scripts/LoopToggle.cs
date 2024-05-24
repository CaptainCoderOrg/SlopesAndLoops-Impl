using UnityEngine;
public class LoopToggle : MonoBehaviour
{
    public EdgeCollider2D ToggleOn;
    public EdgeCollider2D ToggleOff;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        ToggleOn.enabled = true;
        ToggleOff.enabled = false;
    }

}
