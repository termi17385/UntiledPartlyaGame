using TMPro;
using UnityEngine;

namespace UtiledPartlyaGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int lives;
        [SerializeField] private int map0Or1;
    
        public static GameManager instance;
        public int PlayerLives => lives;
        public int GameMap => map0Or1;

        private void Start() => Setup();
        public void Setup()
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        public void SetLives(int _amt) => lives = _amt;
        public void SetMap(int _amt) => map0Or1 = _amt;
    }
}