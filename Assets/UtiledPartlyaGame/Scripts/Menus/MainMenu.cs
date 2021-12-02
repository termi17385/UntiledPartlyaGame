using System;
using UnityEngine;
using System.IO;
using Serialization;

namespace UtiledPartlyaGame.Menus
{
    public class MainMenu : MonoBehaviour
    {
        // Streaming  assets is a folder that Unity creates that we can use.
        // To load/save data in, in the Editor it is in the project folder,
        // in a build, it is in the .exe's build folder
        
        //private string FilePath => Application.streamingAssetsPath + "/gameData";
        private string FilePath;

        
        [SerializeField] private SaveObject jsonToSave;

        private void OnEnable()
        {
        #if UNITY_IOS || UNITY_ANDROID
            FilePath = Application.persistentDataPath + "/gameData";
        #else
            FilePath = Application.streamingAssetsPath + "/gameData";
        #endif
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
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
}