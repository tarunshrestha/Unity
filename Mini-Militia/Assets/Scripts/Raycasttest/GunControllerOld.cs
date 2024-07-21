using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GunControllerOld : MonoBehaviour
{

    
    public GameObject bulletspark;
    public float life = 3f;
    
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed, hitforce;
    //[SerializeField] GameObject Soldier;
    [SerializeField] Transform bulletpos; 
    [SerializeField] Transform bulletpos1;
    private Rigidbody2D rb;
    private float enemylife=5f;
    //public EnemyLife dusman;
    [SerializeField] float shootDelayTime;
    bool bulletJustFired;

  
   
    public float time;
 

    public float range = 100f; // for debug.drawray
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {

        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Shoot();
            
          

        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene("GunMngmt");
        //}

    }

    public void Shoot()
    {
        if (bulletJustFired)
        {
            return;
        }
        Vector2 dir = (bulletpos1.position - bulletpos.position).normalized; //(Direction of bullet and raycast)
        GameObject goli = Instantiate(bullet, bulletpos.position, transform.rotation);

        //Rotating bullet to the direction of gun
        Vector3 look = goli.transform.InverseTransformPoint(bulletpos1.position);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        goli.transform.Rotate(0, 0, angle);

        //Destroy(goli, 5f);

        goli.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
        RaycastHit2D hit;
        
        hit = Physics2D.Raycast(bulletpos.transform.position, dir);

        if (hit.collider != null)
        {
           
            //Debug.Log("collision of raycast with = " + hit.transform.name);
            if (hit.collider.tag == "Player")
            {
                


                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() !=null)
                {
                    
                    float distance = hit.distance;
                     time = distance / bulletSpeed;
                    Debug.Log("distance between raycast and object = " + hit.distance);
                   
                    //enemylife--;
                    //if (enemylife == 0)
                    //{
                    //    Destroy(hit.collider.gameObject, time);
                       
                    //}
                    
                    //Debug.Log("life="+ enemylife);
                    hit.collider.GetComponent<PlayerManager>().TakeDamage(5);
                    
                }
                
            }
            Debug.DrawRay(bulletpos.transform.position, dir * range, Color.green);
           
        }
        StartCoroutine(Showspark());
        bulletJustFired = true;
        StartCoroutine(SetBulletJustFireToFalse());
    }
    IEnumerator Showspark()// spark in bullet
    {
        bulletspark.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        bulletspark.SetActive(false);
    }
    IEnumerator SetBulletJustFireToFalse()
    {
        yield return new WaitForSeconds(shootDelayTime);
        bulletJustFired = false;
    }
   
}
