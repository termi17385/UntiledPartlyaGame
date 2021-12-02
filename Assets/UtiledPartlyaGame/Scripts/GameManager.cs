using TMPro;

using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private int map0Or1;
    [SerializeField] private TextMeshProUGUI displayLives;
    [SerializeField] private TextMeshProUGUI displayMap;
    
    public static GameManager instance;
    public int PlayerLives => lives;
    public int GameMap => map0Or1;

    private void Start() => Setup();
    public void Setup()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        displayLives.text = $"Lives: {PlayerLives}";

        if(map0Or1 == 0)
        {
            displayMap.text = $"Map 0";
        }
        else
        {
            displayMap.text = $"Map 1";
        }
    } 
    public void SetLives(int _amt) => lives = _amt;
    public void SetMap(int _amt) => map0Or1 = _amt;
}
