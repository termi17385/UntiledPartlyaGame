using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int lives;
    [SerializeField] private TextMeshProUGUI displayLives;
    
    public static GameManager instance;
    public int PlayerLives => lives;

    private void Start() => Setup();
    public void Setup()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    
    private void Update() {  displayLives.text = $"Lives: {PlayerLives}"; } 
    public void SetLives(int _amt) => lives = _amt;
}    
