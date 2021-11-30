using UnityEngine;

namespace UtiledPartlyaGame.FPSPlayer
{
    public class P_JumpPing : MonoBehaviour
    {
        [SerializeField] private string floorTag = "Floor";
        public static bool grounded;
    
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(floorTag))
            {
                grounded = true;
            }
        }
    }
}