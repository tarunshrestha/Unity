using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator Anim;
  
    public Collider2D SplashCollider2;
    public Collider2D SplashCollider;
    public Collider2D CapsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
      //  Time.timeScale = 0.5f;
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Name of collided obj = " + collision.gameObject.name);
        if (collision.gameObject.tag == "missile" || collision.gameObject.tag == "Gun" || collision.gameObject.tag =="Player")
            return;
        Vector2 contactPoint = collision.ClosestPoint(transform.position);
        Debug.Log("contact point = " + contactPoint);
        rb.velocity = Vector2.zero;
        //GetComponent<PlayerManager>().TakeDamage(50);
        Anim.SetTrigger("explosion");
        CapsuleCollider.enabled = false;

        if (CapsuleCollider.enabled == false)
        {
            Debug.Log("caapsulecollider is off");
            SplashCollider.enabled = true;
            SplashCollider2.enabled = true;
        }

        Destroy(gameObject, 1f);




       

    }
    
}
