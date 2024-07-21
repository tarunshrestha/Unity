using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotate : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject head;
    float headRotationZ;
    // Start is called before the first frame update
    void Start()
    {
        headRotationZ = head.transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 look = transform.InverseTransformPoint(mousePosition);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        //angle -= 41;
        transform.Rotate(0, 0, angle-41);
        Vector3 headLook = head.transform.InverseTransformPoint(mousePosition);
        float headAngle = Mathf.Atan2(headLook.y, headLook.x) * Mathf.Rad2Deg;
        headRotationZ = Mathf.Clamp((headRotationZ + headAngle+90), -50, 36);
        //head.transform.Rotate(0, 0, headAngle);
        head.transform.localEulerAngles = new Vector3(0, 0, headRotationZ);
        if (player.transform.position.x > mousePosition.x)
        {
            player.transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            player.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
