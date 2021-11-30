using System;
using UnityEngine;
using System.IO;

namespace UtiledPartlyaGame.Menus
{
    public class MainMenu : MonoBehaviour
    {
        // Streaming  assets is a folder that Unity creates that we can use.
        // To load/save data in, in the Editor it is in the project folder,
        // in a build, it is in the .exe's build folder
        private string FilePath => Application.streamingAssetsPath + "/gameData";
        
        [SerializeField] private SaveObject jsonToSave;

        private void OnEnable()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
        }

        public void SetName(String _name)
        {
            jsonToSave.playerName = _name;
        }

        public void SaveJson()
        {
            string json = JsonUtility.ToJson(jsonToSave);
            File.WriteAllText(FilePath + ".json", json);
        }
        
        public void SavePlayerAsMagenta()
        {
            jsonToSave.playerColor = Color.magenta;
            SaveJson();
        }

        public void SavePlayerAsCyan()
        {
            jsonToSave.playerColor = Color.cyan;
            SaveJson();
        }

        public void SavePlayerAsGray()
        {
            jsonToSave.playerColor = Color.gray;
            SaveJson();
        }

    }
    [Serializable] public class SaveObject
    {
        public String playerName;
        public Color playerColor;
    }
}