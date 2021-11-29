// Creator: Josh Jones
// Creation Time: 2021/10/20 3:18 PM

using Mirror;
using UnityEngine;
using UtiledPartlyaGame.Networking;

namespace UtiledPartlyaGame.Player
{
	public class GunTest : NetworkBehaviour
	{
		[Header("Base Variables")]
		[SerializeField] private int damage = 10;
		[SerializeField] private float fireRate = .25f;
		[SerializeField] private float weaponRange = 50f;
		//[SerializeField] private float hitForce = 100f;
		
		[Header("Additional Variables")]
		[SerializeField] private Camera cam;
		[SerializeField] private Transform gunEnd;
		[SerializeField] private ParticleSystem muzzleFlash;
		[SerializeField] private AudioSource gunSound, shootSound;
		[SerializeField] private LayerMask ignorePlayer;
		[SerializeField] private NetworkPlayerManager playerManager;
		private float nextFire;

		private void Start()
		{
			playerManager = GetComponent<NetworkPlayerManager>();
		}

		private void Update()
		{
			Shoot();
		} 

		public void Shoot()
		{
			// if mouse is pressed and the time between shots is higher then next fire
			if(isLocalPlayer && Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFire)
			{
				// set the next fire
				nextFire = Time.time + fireRate;
				// sets the ray to the center of the screen/camera viewport
				Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
				RaycastHit hit;
				//playerManager.CmdStartParticles(); 
				muzzleFlash.Play();
				shootSound.Play();
				
				// shoot the target
				if(Physics.Raycast(rayOrigin, cam.transform.forward, out hit, weaponRange, ignorePlayer))
				{
					if(hit.collider.CompareTag("Player"))
					{
						gunSound.Play();
						var remotePlayerID = hit.collider.GetComponent<NetworkIdentity>().netId;
						CmdHitTarget(remotePlayerID);
						Debug.Log($"Player {hit.collider.gameObject.name} {remotePlayerID} hit!");
					}
				}
			}
		}

		[Command]
		public void CmdHitTarget(uint _id)
		{
			var getTarget = CustomNetworkManager.FindPlayer(_id).GetComponent<NetworkPlayerManager>();
			getTarget.RpcTakeDamage(damage);
		}
	}
}