using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChasingEnemy : MonoBehaviour
{
    [SerializeField] GameObject[] player;
    enum EnemiesType { police1, police2 }
    [SerializeField] EnemiesType enemiesType;
    AudioSource policeAudio;
    //AudioSource engineAudio;
    bool playAudio;
    Rigidbody2D rb;
     bool canChase;
    public bool canplay;
    bool isDestroyed;
    bool cango = false;
    public float speed = 1f;
    public Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("Trails").gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        policeAudio = transform.GetChild(3).GetComponent<AudioSource>(); 
        //engineAudio = transform.GetChild(4).GetComponent<AudioSource>();
        playAudio = true;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (canplay) { 
            if (canChase)
            {
               // StopAllCoroutines();
                DirectionPlayer();

                transform.Translate(speed * Time.deltaTime, 0, 0);
                if((speed > 0) &&(speed  <= 4))
                {
                    speed += 0.5f * Time.deltaTime;
                }
            }
            else
            {
                StartCoroutine(StartGame());
                if ( enemiesType == EnemiesType.police1)
                {
                    rb.velocity = new Vector2(2, 0);
                }
                else
                {                    
                    if(cango)
                    {
                        rb.velocity = new Vector2(-2, 0);
                    }
                }

            
            }
        }
        else
        {
            policeAudio.Stop();
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        }
    }

    void DirectionPlayer()
    {
        float player1Distance = Vector3.Distance(transform.position, player[0].transform.position);
        float player2Distance = Vector3.Distance(transform.position, player[1].transform.position);
        //Vector2 dir = player.transform.position-transform.position;
        Vector2 dir = Vector2.zero;
        if(player1Distance < player2Distance)
        {
            dir = player[0].transform.position - transform.position;
        }
        else
        {
            dir = player[1].transform.position - transform.position;
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 8 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
           

            if (!isDestroyed)
            {
                GameObject.FindObjectOfType<CarChasingManager>().GameOver();
                rb.velocity = Vector3.zero;
                canplay = false;
                transform.Find("blast").GetComponent<Animator>().Play("blast");
                GetComponent<SpriteRenderer>().enabled = false;
                //transform.Find("Trails").gameObject.SetActive(false);
                isDestroyed = true;
            }
            //canplay = false;

        }
    }

    IEnumerator StartGame()
    {
        if ( enemiesType == EnemiesType.police1)
        {
            //engineAudio.Play();
            yield return new WaitForSeconds(1);
            //engineAudio.Stop();
            canChase = true;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            if (playAudio)
            {
                policeAudio.Play();
                playAudio = false;
            }
        }
        else
        {
            yield return new WaitForSeconds(5);
            cango = true;
            //engineAudio.Play();
            yield return new WaitForSeconds(1);
            //engineAudio.Stop();
            canChase = true;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            if (playAudio)
            {
                policeAudio.Play();
                playAudio = false;
            }
        }
        



    }
}
