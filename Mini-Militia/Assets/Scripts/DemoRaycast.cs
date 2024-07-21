using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRaycast : MonoBehaviour
{
    public bool groundTouched;
    public GameObject groundRayObject;
    float groundDistance;
    float groundCheckBuffer = 0.1f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            rb.velocity = new Vector2 (rb.velocity.x, 5);
        }

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, float.MaxValue);

        //if (hit.collider != null)
        //{
        //    if (hit.collider.tag == "ground")
        //    {
        //        groundTouched = true;
        //        Debug.Log("Ground Touched! Distance: " + hit.distance);
        //    }
        //    else
        //    {
        //        groundTouched = false;
        //        Debug.Log("Hit something, but it's not ground. Distance: " + hit.distance);
        //    }
        //}
        //else
        //{
        //    groundTouched = false;
        //    Debug.Log("Ray did not hit anything.");
        //}

        RaycastHit2D raycastHit = Physics2D.Raycast(groundRayObject.transform.position, -Vector2.up);
        Debug.DrawRay(groundRayObject.transform.position, -Vector2.up * raycastHit.distance, Color.red);
        Debug.Log(raycastHit.distance);

    }
}
