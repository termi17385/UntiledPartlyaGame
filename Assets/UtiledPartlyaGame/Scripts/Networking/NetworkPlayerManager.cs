// Creator: Josh Jones
// Creation Time: 2021/10/20 2:50 PM
// Kieran editing :)/
using Mirror;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UtiledPartlyaGame.Player;
using Serialization;
using UnityEngine.UI;

namespace UtiledPartlyaGame.Networking
{
	public class NetworkPlayerManager : NetworkBehaviour
	{
		// Streaming  assets is a folder that Unity creates that we can use.
		// To load/save data in, in the Editor it is in the project folder,
		// in a build, it is in the .exe's build folder
		private string FilePath => Application.streamingAssetsPath + "/gameData";
		[SerializeField] private SaveObject gameData = new SaveObject();
		[Header("Player Stats")]
		[SyncVar(hook = nameof(OnHealthChange))] public float health;
		[SyncVar(hook = nameof(OnSetColour))] public Color playerColour;
		[SyncVar(hook = nameof(OnSetName))] public String playerName;
		[SerializeField, SyncVar] private GameMode gameMode;
		[SerializeField] private Text playerNameText;
		[SerializeField] private TMP_Text playerLivesText;
		[SerializeField] private Material cachedMaterial;
		[SerializeField] private MeshRenderer cachedMesh;
		[SerializeField] private MeshRenderer cachedGunMesh;
		[SerializeField] private Material cyanMaterial;
		[SerializeField] private Material magentaMaterial;
		[SerializeField] private Material grayMaterial;
		[SerializeField] private Image backgroundImage;
		[SerializeField] private bool deathAnimationRun;
		[Space] [SyncVar(hook = nameof(OnPlayerKilled))] public bool isDead;

		[Header("Arrays of data to disable on death")]
		[SerializeField] private GameObject[] objectsToHide;
		[SerializeField] private Behaviour[] componentsToHide;
		[SerializeField] private Collider[] colliders;
		[Space]
		[SerializeField] private CharacterController cController;
		[SerializeField] private PlayerController pController;
		[SerializeField] private UIManager uiManager;
		
		public TMP_Text healthText;
		
		private const float MAX_HEALTH = 100;
		private const int MAX_PLAYERLIVES = 3;
		private MouseLook mLook;
		
		[SerializeField, SyncVar]private bool masterPlayer;

		private void Start()
		{
			if(isServer)
			{
				if(isLocalPlayer)
				{
					gameMode = GameManager.instance.MatchSettings;
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
							gameMode = nPlayer.gameMode;
							break;
						}
					}
				}
			}
			
			if(isLocalPlayer)
			{
				CmdSetColour();
				CmdSetName();
				CmdSetHealth();
				SetupGameModeVariables(gameMode);
				deathAnimationRun = false;
				//CmdSetLives();
			}

			isDead = false;
			
			if(isLocalPlayer)
			{
				cController = GetComponent<CharacterController>();
				mLook = GetComponent<MouseLook>();
				
				uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
				uiManager.DisplayGameMode(gameMode);
			}
			else if (!isLocalPlayer)
			{
				uiManager.enabled = false;
			}
		}

		private void SetupGameModeVariables(GameMode _gameMode)
		{
			Debug.Log($"gamemode is {_gameMode}");
			switch(_gameMode)
			{
				case GameMode.Normal:
					break;
				case GameMode.SuperSpeed:
					GetComponent<PlayerController>().CmdSuperSpeedModeSetup();
					break;
				case GameMode.InstantDeath:
					GetComponent<GunTest>().CmdInstantDeath(100);
					break;
				default:
					break;
			}
		}

		[Command]
		private void CmdSetColour()
		{
			// This is how we read the string data from a file.
			string json = File.ReadAllText(FilePath + ".json");
			// This is how you convert the Json back to a data type.
			// The Generic is requited for making sure the returnded data is the same as the passed in.
			gameData = JsonUtility.FromJson<SaveObject>(json);
			Debug.Log(gameData.playerColor);
			playerColour = gameData.playerColor;
		}

		[Command]
		private void CmdSetName()
		{
			// This is how we read the string data from a file.
			string json = File.ReadAllText(FilePath + ".json");
			// This is how you convert the Json back to a data type.
			// The Generic is requited for making sure the returnded data is the same as the passed in.
			gameData = JsonUtility.FromJson<SaveObject>(json);
			playerName = gameData.playerName;
		}

		private void OnSetColour(Color _old, Color _new)
		{
			if(cachedMaterial == null || backgroundImage == null)
			{
				Debug.LogWarning("PLEASE SET MATERIAL TO CHANGE IN INSPECTOR!!! ---> cachedMaterial = " + cachedMaterial);
			}
			else
			{
				Debug.Log(_new);
				if(_new == Color.magenta)
				{
					//cachedMaterial = magentaMaterial;
					cachedMesh.material = magentaMaterial;
					cachedGunMesh.material = magentaMaterial;
				}
				else if(_new == Color.gray)
				{
					// cachedMaterial = grayMaterial;
					cachedMesh.material = grayMaterial;
					cachedGunMesh.material = grayMaterial;
				}
				if(_new == Color.cyan)
				{
					//cachedMaterial = cyanMaterial;
					cachedMesh.material = cyanMaterial;
					cachedGunMesh.material = cyanMaterial;
				}
				backgroundImage.color = _new;
			}
		}
		private void OnSetName(String _old, String _new)
		{
			if(playerNameText == null)
			{
				Debug.LogWarning("PLEASE SET TEXT BOX TO CHANGE IN INSPECTOR!!! ---> cachedMaterial = " + playerNameText);
			}
			else
			{
				playerNameText.text = _new;
			}
		}
		private void OnSetLives(int _old, int _new)
		{
			if(playerNameText == null)
			{
				Debug.LogWarning("PLEASE SET TEXTBOX TO CHANGE IN INSPECTOR!!! ---> cachedMaterial = " + playerNameText);
			}
			else
			{
				if(_new <= 0)
				{
					Debug.Log("Player Died ---> " + gameObject, gameObject);
				}
				playerLivesText.text = _new.ToString();
			}
		}
	

		private void Update()
		{
			if(isLocalPlayer)
			{
				uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);

				if(health <= 0 && !deathAnimationRun)
				{
					CmdKillPlayer();
					deathAnimationRun = true;
				}
			}
		}

		// todo: this needs to be handled better and changed
		private void OnHealthChange(float _old, float _new)
		{
			//if (isLocalPlayer)
			CmdUpdateHealth(_new);
		}
		
		// todo: this needs to be handled better and changed
		private void OnColourSetup(float _old, float _new)
		{
			if (isLocalPlayer)
				CmdUpdateHealth(_new);
		}

		[Command(requiresAuthority = false)] public void CmdKillPlayer()
		{
			// foreach(var obj in objectsToHide) obj.SetActive(false);
			// foreach(var comp in componentsToHide) comp.enabled = false;
			// foreach(var col in colliders) col.enabled = false;
			// cController.enabled = false;
			// pController.isDead = true;
			// GetComponent<GunTest>().isDead = true;
			RpcKillPlayer();
		}

		[ClientRpc] public void RpcKillPlayer()
		{
			foreach(var obj in objectsToHide) obj.SetActive(false);
			foreach(var comp in componentsToHide) comp.enabled = false;
			foreach(var col in colliders) col.enabled = false;
			cController.enabled = false;
			pController.isDead = true;
			GetComponent<GunTest>().isDead = true;
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
				
				CmdSetHealth();
			}
		}

		[Command]
		private void CmdSetHealth() => health = MAX_HEALTH;
		//
		// [Command]
		// private void CmdSetLives() => playerLives = MAX_PLAYERLIVES;
		
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
				//isDead = true;
			}
		}

		// todo: this needs to be handled better and changed
		/// <summary> Damages the player. </summary>
		/// <param name="_dmg"> How much damage. </param>
		/// <returns>If player dies = true.</returns>
		[Command(requiresAuthority = false)] public void CmdTakeDamage(float _dmg)
		{
			// if(isLocalPlayer)
			{
				uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
				healthText.text = $"{health}/{MAX_HEALTH}";

				health -= _dmg;

				print("Take Damage health = " + health);
				if(health <= 0)
				{
					health = 0;
					CmdKillPlayer();
					//CmdPlayerStatus(true);
				}
			}
		}

		// todo: this needs to be handled better and changed
		[Command(requiresAuthority = false)]
		public void CmdUpdateHealth(float _val)
		{
			if(isLocalPlayer) 
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