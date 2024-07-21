using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public enum SkillTypes {boost, damageTest }
    public SkillTypes type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == SkillTypes.boost)
            {
                collision.transform.GetComponent<PlayerManager>().CurrentBoost("same", 200);
                //player.GetComponent<PlayerManager>().CurrentBoost("add",150 );
                //player.GetComponent<PlayerController>().speed = 10;

                //player.GetComponent<Rigidbody2D>().gravityScale = 10;
                //StartCoroutine(BoostOff());
            }


            //This is demo
            if (type == SkillTypes.damageTest)
            {
                collision.transform.GetComponent<PlayerManager>().TakeDamage(20);
            }
        }
    }

    //IEnumerator BoostOff()
    //{
    //    yield return new WaitForSeconds(5);
    //    //player.GetComponent<PlayerManager>().Boost = 5;
    //    //player.GetComponent<PlayerController>().speed = 10f;
    //    //player.GetComponent<Rigidbody2D>().gravityScale = 3;
    //}
}
