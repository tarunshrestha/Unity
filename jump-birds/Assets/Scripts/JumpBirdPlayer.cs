using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class JumpBirdPlayer : MonoBehaviour
{
    public enum PlayerType { player1 , player2 }
    [SerializeField]PlayerType playerType;
    public Animator animator;
    public JumpBirdManager manager;
    Rigidbody2D Rb;


    public float speed;
    public float Jump = 4;
    public float gravity = 1.5f;

    public bool canplay;
    public bool isTileTouch;
    public bool key;
    Vector3 checkpoint;
    Vector3 startPos;

    AudioSource flapSound;
    AudioSource endSound;
    AudioSource winSound;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        checkpoint = startPos;
        StartPoint();
        flapSound = transform.GetChild(6).GetComponent<AudioSource>();
        endSound = transform.GetChild(7).GetComponent<AudioSource>();
        winSound = transform.GetChild(8).GetComponent<AudioSource>();

        manager = GameObject.FindObjectOfType<JumpBirdManager>();

    }

    // Update is called once per frame
    void Update()
    {
        Pipe();        

    }

    public void JumpBird1()
    {
        if (canplay)
        {
            flapSound.Play();
            BirdAnimationStart();
            Rb.gravityScale = gravity;
            Rb.velocity = new Vector3(speed, Jump, 0);
            if (manager.playBgSound)
            {
                manager.playBgSound = false;
            }

        }
    }

    public void JumpBird2()
    {
        if (canplay)
        {
            flapSound.Play();
            BirdAnimationStart();
            Rb.gravityScale = gravity;
            Rb.velocity = new Vector3(speed, Jump, 0);
            if (manager.playBgSound)
            {
                manager.playBgSound = false;
            }
        }
    }

    //---------------------------------------------------------------On Collision-----------------------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "end")
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            speed = -Mathf.Abs(speed);
            key = true;
            endSound.Play();
            if (canplay && Rb.velocity != Vector2.zero)
            {
                Vector3 newVelocity = GetComponent<Rigidbody2D>().velocity;
                newVelocity.x = speed;
                Rb.velocity = newVelocity;
            }
        }

        if (collision.gameObject.tag == "start")
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            speed = Mathf.Abs(speed); ;

            if (key)
            {
                winSound.Play();
                manager.DisablePlayer();
                canplay = false;
                StopPlayer();
                if (manager.playBgSound == false)
                {
                    manager.playBgSound = true;
                }
                if (playerType == PlayerType.player1)
                {
                    manager.WinnerPlayer1();
                    Debug.Log("Player 1 has won the Game................");
                }

                if (playerType == PlayerType.player2)
                {
                    manager.WinnerPlayer2();
                    Debug.Log("Player 2 has won the Game................");
                }
                key = false;
            }
            Vector3 newVelocity = GetComponent<Rigidbody2D>().velocity;
            newVelocity.x = speed;
            GetComponent<Rigidbody2D>().velocity = newVelocity;
        }
    }

    //---------------------------------------------------------------On trigger-----------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "checkpoint")
        {
            checkpoint = collision.transform.position;
        }
    }

    //---------------------------------------------------------------IEnumerator-----------------------------------------------------------------------------------

    IEnumerator Lost()
    {
        Debug.Log("Touched");

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        BirdAnimationStop();
        yield return new WaitForSeconds(0.3f);
        GetComponent<Rigidbody2D>().gravityScale = gravity;
        yield return new WaitForSeconds(2);
        transform.position = new Vector3(checkpoint.x, startPos.y, startPos.z);
        StopPlayer();
        canplay = true;
        GetComponent<BoxCollider2D>().enabled = true;
        manager.PlayerSpriteRegain();
        manager.PlayerColliderRegain();
        Debug.Log("LostCoroutine");
    }

    //---------------------------------------------------------------Extra-----------------------------------------------------------------------------------

    private void StartPoint()
    {
        BirdAnimationStop();
        GetComponent<Rigidbody2D>().gravityScale = 0;
        canplay = true;
        speed = Mathf.Abs(speed);
        key = false;
        isTileTouch = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void Pipe()
    {
        if (isTileTouch && canplay)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            isTileTouch = false;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Lost());
            canplay = false;
        }
    }

    private void StopPlayer()
    {
        BirdAnimationStop();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    //---------------------------------------------------------------Animator-----------------------------------------------------------------------------------

    private void BirdAnimationStart()
    {
        animator.SetBool("idle", true);
    }
    private void BirdAnimationStop()
    {
        animator.SetBool("idle", false);
    }

}
