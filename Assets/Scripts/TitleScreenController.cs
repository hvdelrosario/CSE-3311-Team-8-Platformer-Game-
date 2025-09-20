using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToPlay(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
