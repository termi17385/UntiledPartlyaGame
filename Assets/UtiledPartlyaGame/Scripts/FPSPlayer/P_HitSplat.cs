using UnityEngine;

namespace UtiledPartlyaGame.FPSPlayer
{
    public class P_HitSplat : MonoBehaviour
    {
        private float deathtime;
    
        void Start()
        {
            deathtime = Time.time + 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time > deathtime)
            {
                Destroy(gameObject);
            }
        }
    }
}