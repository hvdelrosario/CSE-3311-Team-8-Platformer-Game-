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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameData()
    {
        playerPosition = Vector3.zero;  // Will be set by GameManager for new games
        playerHealth = 5; 
        checkpointID = -1;
    }
}
