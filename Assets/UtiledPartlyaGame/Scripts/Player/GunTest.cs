// Creator: Josh Jones
// Creation Time: 2021/10/20 3:18 PM

using Mirror;
using System;
using UnityEngine;
using UtiledPartlyaGame.Networking;

namespace UtiledPartlyaGame.Player
{
	public class GunTest : NetworkBehaviour
	{
		[Header("Input Variables")]
		[SerializeField] private Joystick mobileRightJoystick;
		[SerializeField] private KeyCode [] shootKey = new[] { KeyCode.Mouse0 };
		[SerializeField] private InputMethod inputMethod = InputMethod.MouseAndKeyboard;
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
		[SerializeField] public bool isDead;
		private float nextFire;

		[Header("Inspector")] [SerializeField,Tooltip("If you want to draw shooting Raycast in the Inspector")] private bool drawRay = false;
		[SerializeField] private float mobileShootVariable = 0.5f;

	#region StartUpdate
		private void Start()
		{
			CheckInputPlatform();
			playerManager = GetComponent<NetworkPlayerManager>();
		}

		private void Update()
		{
			if(isDead) return;
			
			Shoot();
			if (drawRay)DrawRay();
		}
		#endregion

			
		private void DrawRay()
		{
			Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
			Debug.DrawRay(rayOrigin,cam.transform.forward * 100, Color.green);
		}

	#region InputMethodChecks
		/// <summary> Checks if player on a mobile platform </summary>
		private void CheckInputPlatform()
		{
		#if UNITY_IOS || UNITY_ANDROID
			    inputMethod = InputMethod.Mobile;
		#else
			inputMethod = InputMethod.MouseAndKeyboard;
		#endif
		}
	#endregion
	#region Shooting

		/// <summary> Setting up Instant Kill Mode. </summary>
		[Command(requiresAuthority = false)]
		public void CmdInstantDeath(int _maxHealth)
		{
			damage = _maxHealth;
		}
		
		public void Shoot()
		{
			// if mouse is pressed and the time between shots is higher then next fire
			if(isLocalPlayer && Time.time > nextFire)
			{
				if(inputMethod == InputMethod.MouseAndKeyboard)
				{
					for(int i = 0; i < shootKey.Length; i++)
					{
						if(Input.GetKey(shootKey[i]))
						{
							FireGun();
							return;
						}
					}
				}
				else if(inputMethod == InputMethod.Mobile)
				{
					if(Math.Abs(mobileRightJoystick.Vertical) > mobileShootVariable)
						FireGun();
				}
			}
		}

		private void FireGun()
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

		[Command]
		public void CmdHitTarget(uint _id)
		{
			var getTarget = CustomNetworkManager.FindPlayer(_id).GetComponent<NetworkPlayerManager>();

			getTarget.CmdTakeDamage(damage);
			if (getTarget.health <=0)
				getTarget.CmdKillPlayer();;
		}
	#endregion
		
	}
}
