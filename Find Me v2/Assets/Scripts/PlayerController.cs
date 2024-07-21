using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public string playerName = "Dark";
    private bool canplay = true;
    private bool groundTouched;

    public float speed= 5;
    public float jump = 8;
    public float health = 4;
    public float fullHealth = 4;
    private float autoHeal = 0.01f;

    Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canplay == true)
        {
            //transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * jump, 0);
            transform.position += new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);

            if (Input.GetButtonDown("Jump") && groundTouched)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                groundTouched = false;

            }


            if (fullHealth > health)
            {
                health += autoHeal * Time.deltaTime;
                Debug.Log($"Health: {health}");

            }


            if (transform.position.x < -8.45)
            {
                transform.position = new Vector2(-8.45f, transform.position.y);
                HurtPlayer();

            }

        }

        if (health <= 0|| transform.position.y < -8.45)
        {
            canplay = false;
            Debug.Log("Player is dead...");
            StartCoroutine(DeathCount());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canplay)
        {
            if (collision.gameObject.tag == "spike")
            {
                HurtPlayer();
            }

            if(collision.gameObject.tag == "ground")
            {
                groundTouched = true;
            }
        }
        
    }

    private void HurtPlayer()
    {
        health -= 1 * Time.deltaTime;
        Debug.Log($"Hurt: {health}");
    }

    IEnumerator DeathCount()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Stage1");
    }
}
