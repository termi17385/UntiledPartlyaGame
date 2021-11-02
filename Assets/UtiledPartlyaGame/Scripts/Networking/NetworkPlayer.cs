using UnityEngine;
using Mirror;
using UtiledPartlyaGame.FPSPlayer;

namespace UtiledPartlyaGame.Networking
{
	/// <summary> Needs a change tbh. </summary>
	public class NetworkPlayer : NetworkBehaviour
	{
		//[SerializeField] private GameObject enemyToSpawn;

		private void Update()
		{
			// First determine if this function is being run on the local player.
			if(isLocalPlayer)
			{
				if(Input.GetKeyDown(KeyCode.E))
				{
					//CmdSpawnEnemy();
					print("Do a thing Pressed 'E'");
				}
			}
		}

		[Command] public void CmdSpawnEnemy()
		{
			// You first need to Instantiate...
			//GameObject newEnemy = Instantiate(enemyToSpawn);
			// ...and then you can spawn the enemy.
			//NetworkServer.Spawn(newEnemy);
		}
		
		// This is run via the network starting and the player connecting...
		// NOT Unity
		// It is run when the object is spawned via the networking system NOT when Unity
		// instantiates the object.
		public override void OnStartClient()
		{
			// This will run REGARDLESS if we are the local or remote player.
			// (isLocalPlayer) is true if this object is the client's local player otherwise it's false.
			
			P_JumpPing myJumpPing = gameObject.GetComponentInChildren<P_JumpPing>();
			myJumpPing.enabled = isLocalPlayer;
			
			P_FireWeapon myFireWeapon = gameObject.GetComponentInChildren<P_FireWeapon>();
			myFireWeapon.enabled = isLocalPlayer;
			
			P_Movement myMovement = gameObject.GetComponentInChildren<P_Movement>();
			myMovement.enabled = isLocalPlayer;

			CustomNetworkManager.AddPlayer(this);
		}

		public override void OnStopClient()
		{
			CustomNetworkManager.RemovePlayer(this);
		}
	}
}