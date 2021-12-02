using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UtiledPartlyaGame.Player
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI IMAGE ELEMENTS")]
        [SerializeField] private Image barHealth;
        [SerializeField] private Image NormalGameMode;
        [SerializeField] private Image SuperspeedGameMode;
        [SerializeField] private Image InstantDeathGameMode;

        [Header("UI TEXT ELEMENTS")]
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textMatchStatus;
    
        // Methods for handling the bars
        // Methods for handling the Text
        public string SetMatchStatus(MatchStatus _status) => textMatchStatus.text = Enum.GetName(typeof(MatchStatus), _status);

        /// <summary> Handles displaying the value of
        /// the given stat type to the players UI  </summary>
        /// <param name="_val">the changeable value sent</param>
        /// <param name="_maxVal">the const max value sent</param>
        /// <param name="_statType">what stat we are sending data for</param>
        public void DisplayStat(float _val, float _maxVal, StatType _statType)
        {
            TextMeshProUGUI text = null;
            Image image = null;
        
            // Handles choosing which UI element(s) to Update
            switch(_statType)
            {
                case StatType.Health:  text = textHealth; image = barHealth; break;
                default:               throw new ArgumentOutOfRangeException(nameof(_statType), _statType, null);
            }
        
            // makes sure we dont try to update nothing
            if(text == null || image == null) return;

            // sets the value of the UI elements
            var clampedVal = Mathf.Clamp01(_val / _maxVal);
            text.text = $"{_val}/{_maxVal}";
            image.fillAmount = clampedVal;
        }

        public void DisplayGameMode(GameMode _gameMode)
        {
            switch(_gameMode)
            {
                case GameMode.Normal:
                    NormalGameMode.gameObject.SetActive(true);
                    break;
                case GameMode.SuperSpeed:
                    SuperspeedGameMode.gameObject.SetActive(true);
                    break;
                case GameMode.OneShotOneKill:
                    InstantDeathGameMode.gameObject.SetActive(true);
                    break;
                default:
                    NormalGameMode.gameObject.SetActive(true);
                    break;
            }
        }
    }
}

public enum MatchStatus
{
    InProgress,
    OverTime,
    MatchEnd,
    Preparing
}

public enum StatType
{
    Health
}