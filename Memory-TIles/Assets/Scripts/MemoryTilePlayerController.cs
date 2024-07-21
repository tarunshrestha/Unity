using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryTilePlayerController : MonoBehaviour
{
    public bool canplay;
    [SerializeField] Vector2 move;
    [SerializeField] FixedJoystick joystick;
    Rigidbody2D rb;
    Animator animator;
    public float speed = 10;
    public bool isAlive;
    
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canplay)
        {
            PlayerController();
        }

    }

    void FixedUpdate()
    {
        
    }

    void PlayerController()
    {
        move.x = joystick.Horizontal;
        move.y = joystick.Vertical;


        if (move.x != 0 && move.y != 0)
        {
            animator.SetBool("walk", true);
            rb.velocity = new Vector2(move.x * speed, move.y * speed);
            PLayerRotation();
        }
        else
        {
            animator.SetBool("walk", false);
        }
    }
    void PLayerRotation()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fall")
        {
            Debug.Log("Touched");
            animator.SetBool("walk", false);
            isAlive = false;
            canplay = false;
        }
    }
}
