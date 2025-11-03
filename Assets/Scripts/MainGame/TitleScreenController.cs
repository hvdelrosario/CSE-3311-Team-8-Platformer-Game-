using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class TitleScreenController : Menu
{
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if(!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
        }
    }
    public void OnNewGameClicked(string sceneName)
    {
        saveSlotsMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnContinueClicked(string sceneName)
    {
        DisableMenuButtons();
        SceneManager.LoadSceneAsync(sceneName);
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
    }

    public void DeactivateMenu() 
    {
        this.gameObject.SetActive(false);
    }
}
