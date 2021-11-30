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
        [SerializeField] private Image barStamina, barAmmo, clock;

        [Header("UI TEXT ELEMENTS")]
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private TextMeshProUGUI textStamina, textAmmo, textTime, textMatchStatus;
    
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
                case StatType.Stamina: text = textStamina; image = barStamina; break;
                case StatType.Ammo:    text = textAmmo; image = barAmmo; break;
                default:               throw new ArgumentOutOfRangeException(nameof(_statType), _statType, null);
            }
        
            // makes sure we dont try to update nothing
            if(text == null || image == null) return;

            // sets the value of the UI elements
            var clampedVal = Mathf.Clamp01(_val / _maxVal);
            text.text = $"{_val}/{_maxVal}";
            image.fillAmount = clampedVal;
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
    Health,
    Stamina,
    Ammo
}