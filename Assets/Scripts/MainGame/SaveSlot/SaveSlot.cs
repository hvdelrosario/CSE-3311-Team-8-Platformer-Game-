using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI heartsText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI saveSlotNumberText;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearDataButton;

    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;


    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }
    public void SetData(GameData data)
    {
        // Set the save slot number (always displayed)
        if (saveSlotNumberText != null)
        {
            saveSlotNumberText.text = "Save Slot: " + profileId;
        }

        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearDataButton.interactable = false;
        }
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearDataButton.interactable = true;

            // Display Hearts
            if (heartsText != null)
            {
                heartsText.text = "Hearts: " + data.playerHealth.ToString();
            }

            // Display Timer (format as minutes:seconds)
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(data.timePassed / 60f);
                int seconds = Mathf.FloorToInt(data.timePassed % 60f);
                timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            }

            // Display Level (scene name)
            if (levelText != null)
            {
                string levelName = GetLevelDisplayName(data.currentSceneName);
                levelText.text = "Level: " + levelName;
            }
        }
    }

    private string GetLevelDisplayName(string sceneName)
    {
        // Convert scene names to friendly display names
        switch (sceneName)
        {
            case "TutorialLevel":
                return "Tutorial";
            case "Level1":
                return "1";
            case "Level2":
                return "2";
            case "Level3":
                return "3";
            default:
                return sceneName;
        }
    }

    public string GetProfileId() 
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearDataButton.interactable = interactable;
    }

}
