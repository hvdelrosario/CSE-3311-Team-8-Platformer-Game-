using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class PlayerStats : MonoBehaviour
{
    public int playerHealth;
    public int maxHealth;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;

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
    }

    public IEnumerator gotHit()
    {
        playerHealth -= 1;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }


}
