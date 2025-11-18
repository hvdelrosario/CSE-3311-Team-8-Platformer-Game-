using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public int playerHealth;
    public int checkpointID;
    public string currentSceneName;
    public bool isGameOver; // Track if player died and went to Game Over screen
    public float timePassed; // Track total time played
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameData()
    {
        playerPosition = Vector3.zero;  // Will be set by GameManager for new games
        playerHealth = 5; // Default to max health
        checkpointID = -1;
        currentSceneName = "TutorialLevel"; // Default starting scene
        isGameOver = false;
        timePassed = 0f;
    }
}
