using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

namespace Dungeoneer
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")] [SerializeField]
        private string fileName = "gameData.json";
        

        private static FileDataHandler _dataHandler;
        [SerializeField] private bool useEncryption = true;
        [SerializeField] private bool useDataPersistence = true;
        private static string playerName;
        public static DataPersistenceManager Instance { get; private set; }
        private static GameData _gameData;
        private static List<IDataPersistence> _dataPersistenceObjects;

        private void Awake()
        {
            //Make sure we dont have more than one script in our scene.
            if (Instance != null)
            {
                Debug.LogError("There can only be one DataPersistanceManager!");
            }
            else
            {
                // Make sure we dont destroy this object when we load a new scene.
                DontDestroyOnLoad(gameObject);
                // Instance reference is this script.
                Instance = this;
                // Create a new FileDataHandler with the path and filename. And find all objects that implement IDataPersistence.
                _dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
                if (useDataPersistence)
                    _dataPersistenceObjects = FindAllDataPersistenceObjects();
            
                // Load the game.
                LoadGame();
            }
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            // Find all objects that implement IDataPersistence.
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public static void NewGame()
        {
            playerName = GameObject.FindWithTag("Canvas").GetComponent<UIManager>().GetNewPlayerName();
            // Create a new GameData object.
            _gameData = new GameData(playerName);
            Debug.Log("PLAYER NAME : " + _gameData.playerData.playerName);
            LoadGame(_gameData);
            SaveGame();
        }

        public static void LoadGame()
        {
            // Load the game data from the file.
            _gameData = _dataHandler.Load();

            // If there is no save data, start a new game.
            if (_gameData == null)
            {
                Debug.Log("No save data found. Starting new game.");
                NewGame();
            }

            if (_dataPersistenceObjects != null && _dataPersistenceObjects.Count != 0)
            {
                foreach (var dataPersistenceObject in _dataPersistenceObjects)
                {
                    dataPersistenceObject.LoadData(_gameData);
                } 
            }
            // Load the data into all objects that implement IDataPersistence.
            
            
            UIManager.SetVersionText(_gameData.version + " | " + DateTime.Today.ToShortDateString());
        }

        public static void LoadGame(GameData gameData)
        {
            // Load the game data from the file.
            _gameData = gameData;

            // If there is no save data, start a new game.
            if (_gameData == null)
            {
                Debug.Log("No save data found. Starting new game.");
                NewGame();
            }

            if (_dataPersistenceObjects != null && _dataPersistenceObjects.Count != 0)
            {
                foreach (var dataPersistenceObject in _dataPersistenceObjects)
                {
                    dataPersistenceObject.LoadData(_gameData);
                } 
            }
            // Load the data into all objects that implement IDataPersistence.
            
            
            UIManager.SetVersionText(_gameData.version + " | " + DateTime.Today.ToShortDateString());
        }

        public static void SaveGame(string customPlayerName = "")
        {
            _gameData = new GameData();
            if(customPlayerName != "")
                _gameData.playerData.playerName = customPlayerName;
            
            // Save the data from all objects that implement IDataPersistence.
            if (_dataPersistenceObjects != null)
            {
                foreach (var dataPersistenceObject in _dataPersistenceObjects)
                {
                    dataPersistenceObject.SaveData(_gameData);
                }
            }

            // Save the game data to the file.
            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit()
        {
            // Pretty self explanatory.
            SaveGame();
        }
    }
}