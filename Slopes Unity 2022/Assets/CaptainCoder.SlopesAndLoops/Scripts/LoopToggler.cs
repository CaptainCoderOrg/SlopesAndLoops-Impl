using UnityEngine;
namespace CaptainCoder.SlopesAndLoops
{
    public class LoopToggler : MonoBehaviour
    {
        public EdgeCollider2D Left;
        public EdgeCollider2D Right;

        public void OnCollisionEnter2D(Collision2D collider)
        {
            Left.enabled = !Left.enabled;
            Right.enabled = !Right.enabled;
        }

    }
}