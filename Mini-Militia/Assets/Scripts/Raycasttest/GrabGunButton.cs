using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GrabGunButton : MonoBehaviour
{
    public bool isGrabbing = false;
    public GameObject parentofgun;
    private GameObject grabbedObject;
   // public GameObject gunGrabbutton;
    public Transform gungrabpos;
    //public Transform grabPos1, grabPos2;
    //private float range = 1;
    // Update is called once per frame

    private void Start()
    {
        GameInfoDisplay.instance.gunGrabButton.gameObject.SetActive(false);
        if (GetComponent<NetworkObject>().IsLocalPlayer)
        {
            GunGrabbuttondeligate();
        }
    }


    void Update()
    {
        //if (isGrabbing == true)
        //{
            
        //    grabbedObject.transform.SetParent(parentofgun.transform);
        //    grabbedObject.transform.position = gungrabpos.position;

        //    isGrabbing = false;

        //}
        //else
        //{
        //  //  Debug.Log("gun is not grabbed");
        //    // grabbedObject.transform.SetParent(null);
        //}

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "gun")
        {
           
            GameInfoDisplay.instance.gunGrabButton.gameObject.SetActive(true);
            Debug.Log("Gun is colliding");
            grabbedObject = collision.transform.parent.gameObject;
            Debug.Log("Grabbed object = " + grabbedObject);

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "gun")
        {
            GameInfoDisplay.instance.gunGrabButton.gameObject.SetActive(false);
            Debug.Log("Gun grabbed is out of reach");
            grabbedObject = null;
            Debug.Log("Grabbed object = " + grabbedObject);
        }
    }
    public void GrabGun()
    {
        GameObject oldGun = GetComponent<PlayerController>().gunManager.gameObject;
        GameObject newGun = grabbedObject;
        //Grab the gun
        newGun.transform.SetParent(parentofgun.transform);
        newGun.transform.position = gungrabpos.position;
        newGun.transform.localEulerAngles = new Vector3(0, 0, 0);
        newGun.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        newGun.transform.localScale = new Vector3(1, 1, 1);
        newGun.GetComponent<GunManager>().UpdateGunINfo();




        //Drop Old Gun and remove oldgun property from player controller
        oldGun.transform.SetParent(null);
        oldGun.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        oldGun.GetComponent<Rigidbody2D>().gravityScale = 1;

        //Assign new grabbed gun to player controller
        GetComponent<PlayerController>().gunManager = newGun.GetComponent<GunManager>();

        //Disable the collision of grabbed gun
        newGun.transform.Find("GunTriggerwithplayer").GetComponent<CapsuleCollider2D>().enabled = false;
        //Enable the collision of old gun
        oldGun.transform.Find("GunTriggerwithplayer").GetComponent<CapsuleCollider2D>().enabled = true;

        



        Debug.Log("Gun grabbed!");

    }
    void GunGrabbuttondeligate()
    {
        GameInfoDisplay.instance.gunGrabButton.onClick.AddListener(() => GrabGun());
    }
}



