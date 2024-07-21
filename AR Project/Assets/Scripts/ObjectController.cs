using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public GameObject Player;
    Animator anim;
    bool canjump = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump()
    {
        if (canjump)
        {
            //GetComponent<Rigidbody2D>().velocity = new Vector3(0, 5, 0);
            anim.SetTrigger("Jump");
            canjump = false;
        }
    }
}
