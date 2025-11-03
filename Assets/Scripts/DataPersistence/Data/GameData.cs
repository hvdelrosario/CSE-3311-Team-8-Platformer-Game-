using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameData()
    {
        playerPosition = Vector3.zero;
    }
}
