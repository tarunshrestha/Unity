using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarChasingPlayerController : MonoBehaviour
{
    public enum PlayerType { player1, player2 };
    public PlayerType playerType;
    [SerializeField] private float Speed = 4;
    public Animator Animator;
    AudioSource carScratch;


    public bool canplay;
    Rigidbody2D rb;
    public Joystick js;
    float rotationSpeed = 8;
    public TextMeshProUGUI debugText;
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        rb = GetComponent<Rigidbody2D>();
        carScratch = transform.Find("scratch").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (canplay && (js.Horizontal != 0 || js.Vertical != 0))
        {
            float _horizontal = js.Horizontal;
            float _vertical = js.Vertical;

            rb.AddForce(new Vector3(_horizontal * Speed*Time.deltaTime, _vertical * Speed*Time.deltaTime, 0));
            DirectionPlayer();

        }

    }

    void StartGame()
    {
        transform.Find("Trails").gameObject.SetActive(false);
    }

    void DirectionPlayer()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle , Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    //--------------------------------------Collision---------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (canplay)
            {
                BlastPlayer();
                if (playerType == PlayerType.player1)
                {
                    GameObject.FindObjectOfType<CarChasingManager>().WonPlayer2();
                }
                if (playerType == PlayerType.player2)
                {
                    GameObject.FindObjectOfType<CarChasingManager>().WonPlayer1();
                }

            }

        }
    }

        public void StartPlayer()
    {
        canplay = true;
        GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("Trails").gameObject.SetActive(true);
        transform.Find("blast").GetComponent<SpriteRenderer>().enabled = false;
    }

    void BlastPlayer()
    {
        //GameObject.FindObjectOfType<CarChasingManager>().isSlow = true;
        //carScratch.Stop();
        rb.velocity = Vector3.zero;
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("blast").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("blast").GetComponent<Animator>().Play("blast");
        //GameObject.FindObjectOfType<CarChasingManager>().GameOver();
        transform.Find("Trails").gameObject.SetActive(false);
        
    }

    


}
