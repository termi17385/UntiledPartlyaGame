using System;
using TMPro;
using UnityEngine;

namespace UtiledPartlyaGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameMode matchSetting;
        [SerializeField] private int map0Or1;
    
        public static GameManager instance;
        public GameMode MatchSettings => matchSetting;
        public int GameMap => map0Or1;

        private void Start() => Setup();
        public void Setup()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        public void ChooseNormalMatchSettings() => matchSetting = GameMode.Normal;
        public void ChooseSuperSpeedMatchSettings() => matchSetting = GameMode.SuperSpeed;
        public void ChooseOneShotOneKillMatchSettings() => matchSetting = GameMode.InstantDeath;
        public void SetMap(int _amt) => map0Or1 = _amt;
    }
    public enum GameMode {Normal, SuperSpeed, InstantDeath };
}