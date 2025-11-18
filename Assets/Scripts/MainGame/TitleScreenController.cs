using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class TitleScreenController : Menu
{
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadGameButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
    
    public void OnNewGameClicked(string sceneName)
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked(string sceneName)
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueClicked(string sceneName)
    {
        DisableMenuButtons();
        
        // Load the most recent save and its scene
        DataPersistenceManager.instance.LoadGameAndScene();
    }

    public void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void ActivateMenu() 
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu() 
    {
        this.gameObject.SetActive(false);
    }
}
