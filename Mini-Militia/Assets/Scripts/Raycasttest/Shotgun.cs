﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shotgun : MonoBehaviour
{


    public GameObject bulletspark;
   
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
   
    [SerializeField] Transform bulletpos, bulletpos1;
    [SerializeField] Transform[] bulletFinalDirPos;
    [SerializeField] float shootDelay;
    private Rigidbody2D rb;
    [SerializeField] Animator anim;
    private float enemylife = 5f;
    
    float bulletlifetime;
    private bool CanShoot = true;
    [SerializeField] int Max_Ammo, CurrentAmmo , BulletCapacity;
   
    [SerializeField] int Timeforreload;
    private bool isReloading = true;
    
    private int BulletsForReload;
    private float time;

   // public GameObject Sniper;
    public float range;// for debug.drawray
    private LineRenderer bulletTrail;
   // public Transform lineMaxPos;
    void Start()
    {
        CurrentAmmo = BulletCapacity;
       
        bulletlifetime = range / bulletSpeed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
       

void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            reload();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GunMngmt");
        }

        if (CurrentAmmo <= 0  && isReloading == true)  
        {
            Debug.Log("NO AMMO");
            reload();
        }

       
        if (Input.GetMouseButton(0) && CanShoot == true && CurrentAmmo != 0)
        {
            
            Shoot();

            anim.SetTrigger("shoot");

        }
    
     

    }

    void reload()
    {
        StartCoroutine(reloadtime());
      
    }
   public void Shoot()
    {

        for (int i = 0; i < bulletFinalDirPos.Length; i++)
        {

            BulletsForReload = BulletCapacity - CurrentAmmo; // no. of bullets added to the gun after reload
            CurrentAmmo--;
            StartCoroutine(Showspark());
            StartCoroutine(bulletdelay());
            Vector2 dir = (bulletFinalDirPos[i].position - bulletpos.position).normalized; //direction of bullet
            GameObject goli = Instantiate(bullet, bulletpos.position, transform.rotation);

           Destroy(goli, bulletlifetime);

            goli.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
            RaycastHit2D hit;

            hit = Physics2D.Raycast(bulletpos.transform.position, dir);

          

            if (hit.collider != null)
            {
                //LineRenderer bulletTrail = goli.GetComponent<LineRenderer>();
                //bulletTrail.SetPosition(0, bulletpos.position);
                //bulletTrail.SetPosition(1, hit.point);

                Debug.Log(hit.distance);
                Debug.Log("Hitpoint= "+hit.point);
                Debug.Log("collision of raycast with = " + hit.transform.name);
                if (hit.collider.tag == "enemy" && hit.distance < range)
                {

                    

                    if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                    {

                        float distance = hit.distance;
                        time = distance / bulletSpeed;
                        Debug.Log("distance between raycast and object = " + hit.distance);

                       
                        //one problem is here, eta bullet le nalagdai gameobject/enemy lai damage lagxa AND tala ko if statement for destruction will only work on last bullet
                        if (enemylife == 0)
                        {
                            Destroy(hit.collider.gameObject, time);

                        }

                        StartCoroutine(damageTime(time));


                    }

                }

            }
            //else
            //{
            //    LineRenderer bulletTrail = goli.GetComponent<LineRenderer>();
            //    bulletTrail.SetPosition(0, bulletpos.position);
            //    bulletTrail.SetPosition(1, lineMaxPos.position);
            //}
           // Debug.DrawRay(bulletpos.transform.position, dir * range, Color.green);

           
        }

        GetComponent<GunAnim>().StartGunAnimation();

    }
    
    IEnumerator damageTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        enemylife--;
        Debug.Log("life=" + enemylife);
    }


    IEnumerator reloadtime()
    {
        BulletsForReload = BulletCapacity - CurrentAmmo;
        if(Max_Ammo<BulletsForReload)

        {
            BulletsForReload = Max_Ammo;
        }
        isReloading = false;
        yield return new WaitForSeconds(Timeforreload);
        //CurrentAmmo = Mathf.Min(Max_Ammo, CurrentAmmo + BulletsForReload) ;
        //Max_Ammo = Mathf.Max(0, Max_Ammo - BulletsForReload);
        CurrentAmmo = CurrentAmmo + BulletsForReload;
        Max_Ammo = Max_Ammo - BulletsForReload;
        //CurrentAmmo = BulletCapacity;
        // Max_Ammo =Mathf.Max(0,Max_Ammo - BulletCapacity);
        // Max_Ammo =  Max_Ammo - BulletCapacity;

        // Max_Ammo = Max_Ammo - BulletsForReload;

        isReloading = true;
    }
    IEnumerator bulletdelay()
    {
       CanShoot = false;
        yield return new WaitForSeconds(shootDelay);
        CanShoot = true;
    }
    IEnumerator Showspark() // spark in bullet
    {
       
            bulletspark.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            bulletspark.SetActive(false);

    }


}
