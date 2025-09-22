using UnityEngine;
using System.Collections;
public class PlayerStats : MonoBehaviour
{
    public float playerHealth;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerHealth = 10;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.CompareTag("EnemyHitbox"))
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
