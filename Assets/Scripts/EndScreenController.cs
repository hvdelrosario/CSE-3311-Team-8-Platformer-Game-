using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenController : MonoBehaviour
{
   
    void Start()
    {

    }

    void Update()
    {

    }

    public void ReturnToTitle(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}