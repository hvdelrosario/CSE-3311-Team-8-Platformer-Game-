using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;

    [SerializeField] private bool overrideProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";
    
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private bool fileLoaded = false;
    private bool isTransitioningToNewLevel = false;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Data Persistence Manager in the scene! Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is disabled! All data will be lost when you close the game.");
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        
        if (isTransitioningToNewLevel)
        {
            // We're in a new level - spawn at start, then save the new scene data
            isTransitioningToNewLevel = false;
            
            if (this.gameData != null)
            {
                Debug.Log("Transitioned to new level: " + scene.name + " (playerPos reset to Vector3.zero, checkpoint = -1)");
                
                // Apply the reset data to all objects (playerPosition is Vector3.zero, checkpoint is -1)
                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                {
                    dataPersistenceObj.LoadData(gameData);
                }
                
                // NOW update the scene name after LoadData has been called
                this.gameData.currentSceneName = scene.name;
                
                // Save immediately so the new scene is recorded
                SaveGame();
            }
        }
        else
        {
            // Normal scene load - just load the data
            LoadGame();
            
            // Clear Game Over flag after loading and save
            if (this.gameData != null && this.gameData.isGameOver)
            {
                this.gameData.isGameOver = false;
                SaveGame();
                Debug.Log("Game Over flag cleared after loading");
            }
        }
    }

    public void LoadGameAndScene()
    {
        if (disableDataPersistence)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData != null && !string.IsNullOrEmpty(this.gameData.currentSceneName))
        {
            fileLoaded = true;
            Debug.Log("LoadGameAndScene: Loading scene '" + this.gameData.currentSceneName + "' with checkpoint " + this.gameData.checkpointID + " at position " + this.gameData.playerPosition + ", isGameOver: " + this.gameData.isGameOver);
            Debug.Log("Current scene is: " + SceneManager.GetActiveScene().name);
            
            // Load the scene the player was in
            // OnSceneLoaded will handle calling LoadGame() which will apply the data
            if (SceneManager.GetActiveScene().name != this.gameData.currentSceneName)
            {
                Debug.Log("Switching from scene '" + SceneManager.GetActiveScene().name + "' to '" + this.gameData.currentSceneName + "'");
                SceneManager.LoadScene(this.gameData.currentSceneName);
            }
            else
            {
                Debug.Log("Already in correct scene, just loading data");
                // If we're already in the correct scene, just load the data
                this.dataPersistenceObjects = FindAllDataPersistenceObjects();
                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                {
                    dataPersistenceObj.LoadData(gameData);
                }
                
                // Clear Game Over flag after loading
                if (this.gameData.isGameOver)
                {
                    this.gameData.isGameOver = false;
                    SaveGame();
                }
            }
        }
        else
        {
            Debug.LogWarning("LoadGameAndScene: No valid game data found or scene name is empty. GameData null? " + (this.gameData == null) + ", Scene name: " + (this.gameData != null ? this.gameData.currentSceneName : "N/A"));
        }
    }

    public void SetGameOverState(bool isGameOver)
    {
        if (this.gameData != null)
        {
            this.gameData.isGameOver = isGameOver;
        }
    }

    public void ChangeSelectedProfileId(string newProfileId) 
    {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        Debug.Log("Selected profile changed to: " + newProfileId);
    }

    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);
        InitializeSelectedProfileId();
        LoadGame();
    }

    public void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.Log("Overriding selected profile ID to: " + this.selectedProfileId);
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is disabled! Not loading data.");
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null && initializeDataIfNull)
        {
            Debug.Log("No data found. Initializing data to defaults.");
            NewGame();
        }
        // if no data can be loaded, then don't continue
        if (this.gameData == null)
        {
            Debug.LogWarning("No game data found. A new game needs to be started before data can be loaded.");
            return;
        }
        else
        {
            fileLoaded = true;
            Debug.Log("LoadGame: Loaded data with checkpoint " + this.gameData.checkpointID + " in scene " + this.gameData.currentSceneName);
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is disabled! Not loading data.");
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("No game data found. A new game needs to be started before data can be saved.");
            return;
        }

        Debug.Log("SaveGame called - collecting data from all persistence objects...");
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
        
        Debug.Log("SaveGame completed - checkpoint " + gameData.checkpointID + " saved to disk in scene " + gameData.currentSceneName);

    }

    public void ResetPositionForNewLevel()
    {
        if (this.gameData != null)
        {
            this.gameData.playerPosition = Vector3.zero;
            this.gameData.checkpointID = -1;
            isTransitioningToNewLevel = true;
            Debug.Log("Player position and checkpoint reset for new level transition");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();

    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool IsFileLoaded()
    {
        return fileLoaded;
    }

    public bool HasGameData()
    {
        return gameData != null;
    }
    
    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
