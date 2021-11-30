// Creator: Josh Jones
// Creation Time: 2021/10/20 2:50 PM
using Mirror;
using System;
using TMPro;
using UnityEngine;
using UtiledPartlyaGame.Player;

namespace UtiledPartlyaGame.Networking
{
	public class NetworkPlayerManager : NetworkBehaviour
	{
		[Header("Player Stats")]
		[SyncVar(hook = nameof(OnHealthChange))] public float health;
		[SerializeField, SyncVar] private int lives;
		public float stamina;
		
		[Space] [SyncVar(hook = nameof(OnPlayerKilled))] public bool isDead;
		
		[Header("Arrays of data to disable on death")]
		[SerializeField] private GameObject[] objectsToHide;
		[SerializeField] private Behaviour[] componentsToHide;
		[SerializeField] private Collider[] colliders;
		[Space]
		[SerializeField] private CharacterController cController;
		[SerializeField] private PlayerController pController;
		[SerializeField] private UIManager uiManager;
		
		public TextMesh healthText;
		
		private const float MAX_STAMINA = 50; 
		private const float MAX_HEALTH = 100;
		private MouseLook mLook;
		[SerializeField, SyncVar]private bool masterPlayer;

		private void Start()
		{
			if(isServer)
			{
				if(isLocalPlayer)
				{
					lives = GameManager.instance.PlayerLives;
					masterPlayer = this;
				}
			}
			else
			{
				if(isLocalPlayer)
				{
					var players = FindObjectsOfType<NetworkPlayerManager>();
					foreach(var nPlayer in players)
					{
						if(nPlayer.masterPlayer)
						{
							lives = nPlayer.lives;
							break;
						}
					}
				}
			}
			
			//if(isLocalPlayer) CmdSetHealth();
			RpcSetHealth();
			
			isDead = false;
			if(isLocalPlayer)
			{
				SetStamina();
				
				cController = GetComponent<CharacterController>();
				mLook = GetComponent<MouseLook>();
				
				uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
				uiManager.DisplayStat(stamina, MAX_STAMINA, StatType.Stamina);
				
				
			}
			else if (!isLocalPlayer)
			{
				uiManager.enabled = false;
			}
		}

		private void Update()
		{
			if(isLocalPlayer) uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
		}

		// todo: this needs to be handled better and changed
		private void OnHealthChange(float _old, float _new)
		{
			RpcUpdateHealth(_new);
		}
		
		// todo: when rigged models are obtained redo this method properly
		private void OnPlayerKilled(bool _old, bool _new)
		{
			if(_new == true)
			{
				foreach(var obj in objectsToHide) obj.SetActive(false);
				foreach(var comp in componentsToHide) comp.enabled = false;
				foreach(var col in colliders) col.enabled = false;

				cController.enabled = false;
				pController.isDead = true;
			}
			else
			{
				Respawn();
				foreach(var obj in objectsToHide) obj.SetActive(true);
				foreach(var comp in componentsToHide) comp.enabled = true;
				foreach(var col in colliders) col.enabled = true;

				cController.enabled = true;
				pController.isDead = false;
				
				RpcSetHealth();
			}
		}

		// todo: this needs to be handled better and changed
		[ClientRpc]
		public void RpcSetHealth() => health = MAX_HEALTH; 
		public void SetStamina() => stamina = MAX_STAMINA; 
		
		private void Respawn()
		{
			// gets the positions
			if(isLocalPlayer)
			{
				var pPosition = transform.position;
				var sPoint = CustomNetworkManager.Instance.GetStartPosition();

				// sets the positions to the new spawn point and keeps it 1 above the ground
				pPosition.x = sPoint.position.x;
				pPosition.y = 1; // todo: higher elevation spawn points might exist eventually
				pPosition.z = sPoint.position.z;
					
				// resets the players rotation
				transform.localRotation = sPoint.rotation;
				transform.position = pPosition;
			}
		}

		// todo: this needs to be handled better and changed
		[Command]
		public void CmdDamagePlayer(float _dmg)
		{
			health -= _dmg;
			if(health <= 0)
			{
				health = 0;
				isDead = true;
			}
		}

		// todo: this needs to be handled better and changed
		[ClientRpc]
		public void RpcTakeDamage(float _dmg)
		{
			if(isLocalPlayer) uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
			healthText.text = $"{health}/{MAX_HEALTH}";
			
			health -= _dmg;
			if(health <= 0)
			{
				health = 0;
				CmdPlayerStatus(true);
			}
		}

		// todo: this needs to be handled better and changed
		[ClientRpc]
		public void RpcUpdateHealth(float _val)
		{
			healthText.text = $"{_val}/{MAX_HEALTH}";
		}
		
		/*[Command]
		public void CmdSetHealth() => health = maxHealth;
		
		[ClientRpc]
		public void RpcDamagePlayer(float _dmg)
		{
			health -= _dmg;
			Debug.Log("damage done: " + _dmg + "to id of: " + name + "health left: " + health);
		}

		[Command]
		public void CmdCheckPlayerHealth()
		{
			if(health <= 0)
			{
				CmdPlayerStatus(true);
				health = 0;
			}
		}*/

		public void ButtonDeath()
		{
			CmdPlayerStatus(true);
		}

		public void ButtonRespawn()
		{
			CmdPlayerStatus(false);
		}

		public void DamageSelf(int _damage = 10)
		{
			
			CmdDamagePlayer(_damage);
		}

		// todo: this needs to be handled better and changed
		[Command]
		public void CmdPlayerStatus(bool _value)
		{
			isDead = _value;
		} 
		
		// Turn on if you want.
		// private void OnGUI()
		// {
		// 	if(!isLocalPlayer) return;
		// 	if(GUI.Button(new Rect(100, 100, 100, 50), "Death"))
		// 	{
		// 		ButtonDeath();	
		// 	}
		// 		
		// 	if(GUI.Button(new Rect(100, 200, 100, 50), "Respawn"))
		// 	{
		// 		ButtonRespawn();
		// 	}
		//
		// 	if(GUI.Button(new Rect(100, 300, 100, 50), "Damage Player"))
		// 	{
		// 		DamageSelf();
		// 	}
		// }
	}
}