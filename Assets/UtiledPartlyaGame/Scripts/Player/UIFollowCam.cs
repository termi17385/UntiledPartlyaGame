using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtiledPartlyaGame.Networking;

using NetworkPlayer = UtiledPartlyaGame.Networking;

namespace UtiledPartlyaGame.Player
{
    public class UIFollowCam : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private NetworkPlayer.NetworkPlayer localPlayer;
    
        private void Update()
        {
            localPlayer = CustomNetworkManager.LocalPlayer;
            if(localPlayer != null)
            {
                target = localPlayer.transform;
                transform.LookAt(target, Vector3.up);
            }
        } 
    }
}