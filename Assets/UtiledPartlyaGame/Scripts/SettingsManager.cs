using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UtiledPartlyaGame
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayMatchSettings;
        [SerializeField] private TextMeshProUGUI displayMap;
        // Update is called once per frame
        void Update()
        {
            if(GameManager.instance != null)
            {
                displayMatchSettings.text = GameManager.instance.MatchSettings.ToString();

                if(GameManager.instance.GameMap == 0)
                {
                    displayMap.text = $"Map 0";
                }
                else
                {
                    displayMap.text = $"Map 1";
                }
            }
            else
            {
                displayMap.text = $"Map 1";
                displayMatchSettings.text = $"Lives: 1";
            }
        }
    }
}