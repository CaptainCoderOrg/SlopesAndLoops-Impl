using UnityEngine;
namespace CaptainCoder.SlopesAndLoops
{
    public class LoopController : MonoBehaviour
    {
        public Transform FirstEntry;
        public EdgeCollider2D FirstRamp;
        public Transform SecondEntry;
        public EdgeCollider2D SecondRamp;

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerMovementController>() == null) { return; }
            float firstDistance = Vector2.Distance(FirstEntry.position, collider.gameObject.transform.position);
            float secondDistance = Vector2.Distance(SecondEntry.position, collider.gameObject.transform.position);
            if (firstDistance < secondDistance)
            {
                FirstRamp.enabled = true;
                SecondRamp.enabled = false;
            }
            else
            {
                SecondRamp.enabled = true;
                FirstRamp.enabled = false;
            }
        }

    }
}