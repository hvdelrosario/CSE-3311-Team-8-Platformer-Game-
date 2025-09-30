using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class PlayerStats : MonoBehaviour
{
    public float playerHealth;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private float maxHeight;
    private bool rising = true;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerHealth = 10;
        maxHeight = -1f;
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
        if(transform.position.y < maxHeight && maxHeight > 4 && rising)
        {
            Debug.Log("Peak height is " + maxHeight);
            rising = false;
        }
        if(transform.position.y > maxHeight)
        {
            rising = true;
        }
        maxHeight = transform.position.y;

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
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
