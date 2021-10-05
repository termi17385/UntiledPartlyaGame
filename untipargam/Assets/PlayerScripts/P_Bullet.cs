using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;

namespace Player
{
    public class P_Bullet : MonoBehaviour
    {
        private Rigidbody rb;
        private int bulletVelocity = 100;
        private float spawnTime, deathTime;
        //[SerializeField] private ParticleSystem ParticlePuff;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            deathTime = Time.time + 3;
            rb.velocity = transform.forward * bulletVelocity;
        }

        private void Update()
        {
            // kills on miss
            if (Time.time > deathTime)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
			if (other.transform.GetComponent<HealthManager>())
				other.transform.GetComponent<HealthManager>().ChangeHP(-1);
            //Instantiate(ParticlePuff, transform.position, Quaternion.identity);
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            float die = UnityEngine.Random.Range(0, 0.3f);
            yield return new WaitForSeconds(die); 
            //Instantiate(ParticlePuff, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

