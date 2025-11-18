using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] private Button lastSaveButton;
   
    void Start()
    {
        DisableButtonsDependingOnData();
    }

    void Update()
    {

    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            if (lastSaveButton != null)
            {
                lastSaveButton.interactable = false;
            }
        }
    }

    public void OnLastSaveClicked()
    {
        DisableMenuButtons();
        
        // Load the most recent save and its scene
        DataPersistenceManager.instance.LoadGameAndScene();
    }

    public void DisableMenuButtons()
    {
        if (lastSaveButton != null)
        {
            lastSaveButton.interactable = false;
        }
    }

    public void ReturnToTitle(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}