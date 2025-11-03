using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{

    [SerializeField] private TitleScreenController mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    [Header("Confirmation Popup")]
    [SerializeField]private ConfirmationPopupMenu confirmationPopupMenu;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();

    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        if (isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameAndLoadScene();
        }
        else if (saveSlot.hasData)
        {
            confirmationPopupMenu.ActivateMenu("Overwrite existing save data?",
            () =>
            {
                DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                DataPersistenceManager.instance.NewGame();
                SaveGameAndLoadScene();
            },
            () =>
{
                this.ActivateMenu(isLoadingGame);
            });
        }
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            SaveGameAndLoadScene();
        }
    }
    private void SaveGameAndLoadScene()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("Sandbox_Movement");
    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
       DisableMenuButtons();

        confirmationPopupMenu.ActivateMenu(
            "Are you sure you want to delete this saved data?",
            // function to execute if we select 'yes'
            () => {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                ActivateMenu(isLoadingGame);
            },
            // function to execute if we select 'cancel'
            () => {
                ActivateMenu(isLoadingGame);
            }
        );
    } 

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        mainMenu.DeactivateMenu();

        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        backButton.interactable = true;

        GameObject firstSelected = backButton.gameObject;

        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}