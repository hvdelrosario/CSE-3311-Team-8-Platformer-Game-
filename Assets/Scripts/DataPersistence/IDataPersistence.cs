using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public interface IDataPersistence
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
