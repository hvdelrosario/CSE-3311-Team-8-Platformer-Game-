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
        if(playerHealth <= 0)
        {
            SceneManager.LoadScene("EndScreen");
        }
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Sandbox_Movement");
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
        if(collider.gameObject.CompareTag("DeathZone"))
        {
            StartCoroutine(fallenOff());
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
        if (gameManager.GetComponent<GameManager>().mostRecentCheckpoint == -1)
        {
            respawnCoordinate = gameManager.GetComponent<GameManager>().startPosition;
        }
        else
        {
            respawnCoordinate = gameManager.GetComponent<GameManager>().checkpoints[gameManager.GetComponent<GameManager>().mostRecentCheckpoint].transform.position;
        }
        //Accounting for player clipping into the checkpoint
        respawnCoordinate += new Vector3(0, 0.5f, 0);
        transform.position = respawnCoordinate;
        yield return null;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
        this.playerHealth = data.playerHealth;
    }
    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
        data.playerHealth = this.playerHealth;
    }
}
