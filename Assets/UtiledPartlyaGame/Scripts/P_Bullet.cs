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

        private float die;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            deathTime = Time.time + 3;
            die = UnityEngine.Random.Range(0, 0.2f);
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
            StartCoroutine(Death());
        }

        private IEnumerator Death()
        {

            yield return new WaitForSeconds(die);
            Destroy(gameObject);
        }
    }
}

