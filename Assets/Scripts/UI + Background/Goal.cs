using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Save current game state (including timer) before transitioning
            if (DataPersistenceManager.instance != null && DataPersistenceManager.instance.HasGameData())
            {
                // First save the current state (timer, health, etc.)
                DataPersistenceManager.instance.SaveGame();
                Debug.Log("Saved game before level transition");
                
                // Then reset position for new level (keeps timer intact)
                DataPersistenceManager.instance.ResetPositionForNewLevel();
            }
            
            Debug.Log("Level complete - transitioning to next level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
