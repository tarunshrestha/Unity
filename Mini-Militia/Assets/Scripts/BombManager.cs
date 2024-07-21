using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public enum BombTypes
    {
        Granade,
        Poision,
        Timer
    }
    public BombTypes bombType;
    public Vector3 LaunchOffset;
    ParticleSystem smokeParticle;
    bool Thrown;
    int damage = 10;
    string currentBomb = "";
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        smokeParticle = transform.Find("Smoke").GetComponent<ParticleSystem>();
        if (!Thrown)
        {
            if(bombType == BombTypes.Granade) // For Granade activation
            {
                transform.Find("grenade_bomb").gameObject.SetActive(true);
                transform.Find("grenade_bomb").GetComponent<CircleCollider2D>().enabled = false;
                StartCoroutine(GranadeBlastTimer());
                Debug.Log("Granade");
            }
            else if(bombType == BombTypes.Poision) // For Poision Bomb activation
            {
                transform.Find("PoisionBomb").gameObject.SetActive(true);
                transform.Find("PoisionBomb").GetComponent<CircleCollider2D>().enabled = false;
                StartCoroutine(PoisonBlastTimer());
                Debug.Log("Poision Bomb");
            }
            else if (bombType == BombTypes.Timer) // For Timer Bomb activation
            {

            }

            transform.GetComponent<CircleCollider2D>().enabled = false;
        }



    }

    // Update is called once per frame
    void Update()
    {
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (bombType == BombTypes.Granade)  // For granade collision
            {
                float radius = Vector3.Distance(collision.transform.position, transform.position);
                Debug.Log($"Radius:" + radius);
                if (radius <= 3)
                {
                    if (radius <= 1.85f)
                    {
                        collision.transform.GetComponent<PlayerManager>().TakeDamage(100);
                    }
                    else
                    {
                        //collision.transform.GetComponent<PlayerManager>().TakeDamage(50);
                    }
                }
                else
                {
                    collision.transform.GetComponent<PlayerManager>().TakeDamage(50);
                }
                Debug.Log("Damaged");
            }
        }
            
    }

    IEnumerator GranadeBlastTimer()
    {
        yield return new WaitForSeconds(0.01f);
        Debug.Log("timer");
        transform.Find("grenade_bomb").GetComponent<CircleCollider2D>().enabled =true;
        yield return new WaitForSeconds(3);
        transform.Find("Trail").gameObject.SetActive(false);
        transform.Find("grenade_bomb").GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<CircleCollider2D>().enabled = true;
        anim.SetBool("blast", true);
        Debug.Log("start");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator PoisonBlastTimer()
    {
        yield return new WaitForSeconds(0.01f);
        transform.Find("PoisionBomb").GetComponent<CircleCollider2D>().enabled = true;
        yield return new WaitForSeconds(3);
        transform.Find("Trail").gameObject.SetActive(false);
        transform.Find("PoisionBomb").gameObject.SetActive(false);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        smokeParticle.Play();
        yield return new WaitForSeconds(10);
        Destroy(gameObject);

    }
}
