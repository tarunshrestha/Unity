using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBirdTile : MonoBehaviour
{
    public bool pipe;
    AudioSource outSound;
    // Start is called before the first frame update
    void Start()
    {
        pipe = false;
        outSound = transform.GetChild(0).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "line")
        {
            Debug.Log("Touched");
            outSound.Play();
            ShakeCamera();
            if (collision.transform.parent.GetComponent<JumpBirdPlayer>().canplay)
            {
                collision.transform.parent.GetComponent<JumpBirdPlayer>().isTileTouch = true;

                //------------------------------Box collider------------------------------------------
                collision.transform.parent.GetChild(2).GetComponent<BoxCollider2D>().enabled = false;
                collision.transform.parent.GetChild(3).GetComponent<BoxCollider2D>().enabled = false;

                //------------------------------Sprite Renderer------------------------------------------
                collision.transform.parent.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                collision.transform.parent.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;

                //------------------------------Particle System------------------------------------------
                collision.transform.parent.GetChild(5).GetComponent<ParticleSystem>().Play();
            }
            pipe=true;

        }
    }

    
    void ShakeCamera()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("shake");
    }

   

}
