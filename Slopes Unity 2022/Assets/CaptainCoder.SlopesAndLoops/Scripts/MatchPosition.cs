using UnityEngine;
namespace CaptainCoder.SlopesAndLoops
{
    public class MatchPosition : MonoBehaviour
    {
        public Vector3 Offset;
        public Transform Target;
        void Update()
        {
            transform.position = Target.position + Offset;
        }
    }
}