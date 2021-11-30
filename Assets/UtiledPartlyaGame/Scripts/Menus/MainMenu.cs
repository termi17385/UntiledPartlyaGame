using System;
using UnityEngine;

namespace UtiledPartlyaGame.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Color playerColor;

        [SerializeField] private SaveObject jsonToSave;
        
        public void SetName(String _name)
        {
            jsonToSave.playerName = _name;
        }

        public void SavePlayerAsMagenta()
        {
            jsonToSave.playerColor = Color.magenta;
        }
        
        public void SavePlayerAsCyan()
        {
            jsonToSave.playerColor = Color.cyan;
        }
        
        public void SavePlayerAsGray()
        {
            jsonToSave.playerColor = Color.cyan;
        }

        [Serializable]
        private class SaveObject
        {
            public String playerName;
            public Color playerColor;
        }
    }
}