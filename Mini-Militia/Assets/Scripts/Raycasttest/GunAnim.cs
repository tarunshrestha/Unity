using System.Collections;
using UnityEngine;

public class GunAnim : MonoBehaviour
{
    private bool isAnimating = false;
    private Vector3 originalRotposition;
    public Transform Gunpos;
    public float SpeedOfRecoil;
    public float MaxAngleforRecoil;
    
    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log("first= " + originalRotposition);
        originalRotposition = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {


        //if (Input.GetMouseButtonDown(0))
        //{
        //    StartGunAnimation();
        //}
        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    originalRotposition = transform.eulerAngles;
        //    StartGunAnimation();
            
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    StopGunAnimation();
        //}

    }

    //void GunAnimation()
    //{

       
    //    transform.Rotate(Vector3.forward, SpeedOfRecoil * Time.deltaTime);
     
    //}

    public void StartGunAnimation()
    {
        isAnimating = true;
        StopAllCoroutines();
        StartCoroutine(GunAnima());

    }

    //void StopGunAnimation()
    //{
        
    //    isAnimating = false;
       
       
    //}

    IEnumerator GunAnima()
    {
        //Gunpos.localEulerAngles = originalRotposition;
        //GunAnimation();
        Vector3 newAngle = originalRotposition;
        newAngle.z += Random.Range(0, MaxAngleforRecoil);
        //Gunpos.localEulerAngles = newAngle;

        //transform.Rotate(Vector3.back, SpeedOfRecoil * Time.deltaTime);
        //Debug.Log("New Angle: " + newAngle.z);
        while(transform.localEulerAngles.z < newAngle.z)
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + SpeedOfRecoil * Time.deltaTime);
            yield return null;
        }
        isAnimating = false;

        Gunpos.localEulerAngles = originalRotposition;

    }
}