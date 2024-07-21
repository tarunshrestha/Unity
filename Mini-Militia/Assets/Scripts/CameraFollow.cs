using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerAim;
    public GameObject mainPlayer;
    public GameObject parentCamera;
    public float followSpeed= 20f;
    private int zoomPressed = 1;



    //UI
    public Button zoomBtn;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().orthographicSize = 5;

        zoomBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 1 + "x";
        
        parentCamera = transform.parent.gameObject;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerAim == null)
        {
            return;
        }

        //Limits Camera range
        if (transform.position.y < -60)
        {
            transform.position =new Vector3 (transform.position.x, -60, -10f);
        }

        // main parent following player
        Vector3 newParent = new Vector3(mainPlayer.transform.position.x, mainPlayer.transform.position.y, -10);
        parentCamera.transform.position = Vector3.Lerp(parentCamera.transform.position, newParent, 12 * Time.deltaTime);

        // PLayer and target offset
        Vector3 offset = playerAim.transform.position- mainPlayer.transform.position;
        Vector3 newpos = new Vector3(offset.x, offset.y, -10f); // for camera view

        // Camera local position 
        transform.localPosition = Vector3.Lerp(transform.localPosition, newpos, followSpeed * Time.deltaTime);


    }

    public void Zoom()
    {
        zoomPressed += 1;

        if (zoomPressed == 1)
        {
            GetComponent<Camera>().orthographicSize = 6;
            zoomBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 1 + "x";

        }
        else if (zoomPressed == 2)
        {
            GetComponent<Camera>().orthographicSize = 8;
            zoomBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 2 + "x";
        }
        else if (zoomPressed == 3)
        {
            GetComponent<Camera>().orthographicSize = 10;
            zoomBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 4 + "x";
        }
        else if (zoomPressed == 4)
        {
            GetComponent<Camera>().orthographicSize = 12;
            zoomBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 6 + "x";
            zoomPressed = 0;
        }

    }
}
