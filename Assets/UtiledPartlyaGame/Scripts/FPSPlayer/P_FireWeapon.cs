using UnityEngine;
using UnityEngine.UI;

namespace UtiledPartlyaGame.FPSPlayer
{
    public class P_FireWeapon : MonoBehaviour
    {
        [SerializeField] private GameObject BulletPrefab, CasingPrefab, ShotSpawner, CasingSpawner;
        [SerializeField] private float rateOfFire = 0.1f, shotGap;
        [SerializeField] private AudioSource bulletNoise;
        [SerializeField] private GameObject Head, Gun;
        [SerializeField] private int shotNum;
        [SerializeField] private float[] shotVariation;
        [SerializeField] private int variationMinimiser = 2;
        [SerializeField] private Image crosshair;
        [SerializeField] private int crosshairScaler = 7;
        [SerializeField] private ParticleSystem muzzleFlash;

        void Update()
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                Fire();
            }
            
            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                shotNum = 0;
            }
        }

        private void FixedUpdate()
        {
            RaycastHit hit;
            if(Physics.Raycast(Head.transform.position, Head.transform.forward, out hit, Mathf.Infinity))
            {
                //gun looks at where the player is looking
                Gun.transform.LookAt(hit.point);
                //Debug.Log("hit");
            }
            
            //go back to gun's rotation
            ShotSpawner.transform.rotation = Quaternion.Slerp(ShotSpawner.transform.rotation, Gun.transform.rotation, Time.deltaTime * variationMinimiser);
            //crosshair size & rotation
            crosshair.transform.localScale =  Vector3.Slerp(Vector3.one, new Vector3(1 + (crosshairScaler * shotNum), 1 + (crosshairScaler * shotNum), 1), Time.deltaTime/ 2);
            //crosshair.transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, 0, 180 * shotNum),
                //Time.deltaTime / 2);
        }

        public void Fire()
        {
            if (Time.time > shotGap)
            {
                //limits shot variation
                shotNum++;
                if (shotNum > 4)
                    shotNum = 4;
                
                //creates variation
                if (shotNum > 1)
                {
                    for (int i = 0; i < shotVariation.Length; i++)
                    {
                        shotVariation[i] = UnityEngine.Random.Range(-0.5f, 0.5f) * shotNum;
                    }
                    ShotSpawner.transform.Rotate(new Vector3( shotVariation[0],shotVariation[1],0));
                }
                
                //normal firing outputs
                GameObject bulletClone = Instantiate(BulletPrefab, ShotSpawner.gameObject.transform.position, ShotSpawner.transform.rotation);
                GameObject casing = Instantiate(CasingPrefab, CasingSpawner.gameObject.transform.position,
                    CasingSpawner.transform.rotation);
                bulletNoise.Play();
                muzzleFlash.Play();
                shotGap = Time.time + rateOfFire;
            }
        }
    }
}

