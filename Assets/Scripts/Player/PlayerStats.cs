using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class PlayerStats : MonoBehaviour, IDataPersistence
{
    public int playerHealth;
    public int maxHealth;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    public GameObject gameManager;
    public Vector3 respawnCoordinate;
    void Awake()
    {
        playerHealth = 5;
        maxHealth = 5;
    }
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth > maxHealth)
        {
            playerHealth = maxHealth;
        }
        if(playerHealth <= 0)
        {
            // Mark as Game Over and save before going to EndScreen
            if (DataPersistenceManager.instance != null)
            {
                // Set Game Over flag in the save data
                DataPersistenceManager.instance.SetGameOverState(true);
                DataPersistenceManager.instance.SaveGame();
                Debug.Log("Game Over - saving with isGameOver flag");
            }
            SceneManager.LoadScene("EndScreen");
        }
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("TutorialLevel");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Note do this to get a child of a collision
        Collider2D targetArea = collision.GetContact(0).collider;
        // Debug.Log(targetArea.gameObject.name);
        if(targetArea.gameObject.CompareTag("EnemyHitbox"))
        {
            StartCoroutine(gotHit());
        }
        else if(collision.gameObject.CompareTag("Checkpoint"))
        {
            gameManager.GetComponent<GameManager>().updateCheckpoint(collision.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("DeathZone") || collider.gameObject.CompareTag("Spikes"))
        {
            StartCoroutine(fallenOff());
        }
        if(collider.gameObject.CompareTag("FinishZone"))
        {
            StartCoroutine(fallenOff());
        }
        else if(collider.gameObject.CompareTag("Powerup"))
        {
            gameManager.GetComponent<GameManager>().timePassed -= collider.gameObject.GetComponent<TimerPowerup>().timeCut;
            Destroy(collider.gameObject);
        }
    }
    public IEnumerator gotHit()
    {
        playerHealth -= 1;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }

    public IEnumerator fallenOff()
    {
        playerHealth -= 1;
        GameManager gm = gameManager.GetComponent<GameManager>();
        
        if (gm.mostRecentCheckpoint == -1)
        {
            respawnCoordinate = gm.startPosition;
            Debug.Log("Respawning at start position: " + respawnCoordinate);
        }
        else
        {
            respawnCoordinate = gm.checkpoints[gm.mostRecentCheckpoint].transform.position;
            //Accounting for player clipping into the checkpoint
            respawnCoordinate += new Vector3(0, 0.5f, 0);
            Debug.Log("Respawning at checkpoint " + gm.mostRecentCheckpoint + " at position: " + respawnCoordinate);
        }
        transform.position = respawnCoordinate;
        yield return null;
    }

    public void LoadData(GameData data)
    {
        // Load the saved health (will be maxHealth if loading from Game Over)
        this.playerHealth = data.playerHealth;
        Debug.Log("LoadData: Player health set to " + this.playerHealth + ", isGameOver was: " + data.isGameOver);
    }
    public void SaveData(ref GameData data)
    {
        // If we're in Game Over state, save max health so player respawns with full health
        if (data.isGameOver)
        {
            data.playerHealth = this.maxHealth;
        }
        else
        {
            data.playerHealth = this.playerHealth;
        }
    }
}
