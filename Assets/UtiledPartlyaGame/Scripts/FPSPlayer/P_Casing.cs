using System.Collections;
using UnityEngine;

namespace UtiledPartlyaGame.FPSPlayer
{
    public class P_Casing : MonoBehaviour
    {

        private int deathTime = 40;
        void Start()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(2,0,0),ForceMode.Impulse);
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            yield return new WaitForSeconds(deathTime);
            Destroy(gameObject);
        }

    }
}