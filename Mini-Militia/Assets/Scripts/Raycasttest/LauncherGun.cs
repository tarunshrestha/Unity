using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LauncherGun : MonoBehaviour
{


    public GameObject bulletspark;
  //  public GameObject blood;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    //[SerializeField] GameObject Soldier;
    [SerializeField] Transform bulletpos, bulletpos1;
    [SerializeField] Transform[] bulletFinalDirPos;
    [SerializeField] float shootDelay;
    private Rigidbody2D rb;
    //[SerializeField] Animator anim;
    //private float enemylife = 5f;
    //public EnemyLife dusman;
    float bulletlifetime;
    private bool CanShoot = true;
    [SerializeField] int Max_Ammo, CurrentAmmo, BulletCapacity;

    [SerializeField] float Timeforreload;
    private bool isReloading = true;

    private int BulletsForReload;
    private float time;


    public float range;// for debug.drawray
    public PlayerNetwork playerNetwork;
    void Start()
    {
        CurrentAmmo = BulletCapacity;
        //Time.timeScale = 0.1f;
        bulletlifetime = range / bulletSpeed;
        UpdateGunINfo();
        GameInfoDisplay.instance.currentGunImage.sprite = GetComponent<GunInfo>().gunSprite;
       // Debug.Log(GetComponent<GunInfo>().guntype);
    }




    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    reload();
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene("GunMngmt");
        //}

        if (CurrentAmmo <= 0 && isReloading == true)
        {
            Debug.Log("NO AMMO");
            reload();
        }

        //Vector2 dir = (bulletpos1.position - bulletpos.position).normalized; //(Direction of bullet and raycast)
        //if (Input.GetMouseButton(0) && CanShoot == true && CurrentAmmo != 0)
        //{
        //    //if (Max_Ammo <= 0)
        //    //{
        //    //    return;
        //    //}
        //    Shoot();

        //   // anim.SetTrigger("shoot");

        //}



    }

    //void Shoot()
    //{

    //    Vector2 dir = (bulletpos1.position - bulletpos.position).normalized; //direction of bullet
    //    RaycastHit2D hit;

    //    hit = Physics2D.Raycast(bulletpos.transform.position, dir);

    //    if (hit.collider != null)
    //    {

    //        Debug.Log("collision of raycast with = " + hit.transform.name);
    //        if (hit.collider.tag == "enemy")
    //        {



    //            if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
    //            {

    //                float distance = hit.distance;
    //                time = distance / bulletSpeed;
    //                Debug.Log("distance between raycast and object = " + hit.distance);

    //                enemylife--;
    //                if (enemylife == 0)
    //                {
    //                    Destroy(hit.collider.gameObject, time);

    //                }

    //                Debug.Log("life=" + enemylife);


    //            }

    //        }
    //        Debug.DrawRay(bulletpos.transform.position, dir * range, Color.green);

    //    }

    //    Vector2 direction2 = (bulletpos2.position - bulletpos.position).normalized; 
    //    RaycastHit2D hit2 = Physics2D.Raycast(bulletpos.transform.position, direction2 );

    //    if (hit2.collider != null)
    //    {

    //        Debug.Log("collision of 2nd raycast with: " + hit2.collider.gameObject.name);

    //    }
    //    Debug.DrawRay(bulletpos.transform.position, direction2 * range, Color.blue);
    //    StartCoroutine(Showspark());
    //}
    void reload()
    {
        StartCoroutine(reloadtime());

    }
    public void RequestShoot()
    {
        if (!CanShoot || CurrentAmmo == 0)
            return;
        Shoot();
        Debug.Log("hellooooo");
        playerNetwork.RequestShootServerRpc();


    }
   public void Shoot()
    {
        BulletsForReload = BulletCapacity - CurrentAmmo; // no. of bullets added to the gun after reload
        CurrentAmmo--;
        for (int i = 0; i < bulletFinalDirPos.Length; i++)
        {

            StartCoroutine(Showspark());
            StartCoroutine(bulletdelay());
            Vector2 dir = (bulletFinalDirPos[i].position - bulletpos.position).normalized; //direction of bullet
            float angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
            Debug.Log("Angle = " + angle);
            if (dir.x < 0)
            {
                angle += 180;
            }
            GameObject goli = Instantiate(bullet, bulletpos.position, Quaternion.Euler(0,0,angle));

            Destroy(goli, bulletlifetime);

            goli.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
            //RaycastHit2D hit;

            //hit = Physics2D.Raycast(bulletpos.transform.position, dir);

            //if (hit.collider != null)
            //{
            //    Debug.Log(hit.distance);
            //    Debug.Log("collision of raycast with = " + hit.transform.name);
            //    if (hit.collider.tag == "enemy" && hit.distance < range)
            //    {



            //        if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            //        {

            //            float distance = hit.distance;
            //            time = distance / bulletSpeed;
            //            Debug.Log("distance between raycast and object = " + hit.distance);

            //            enemylife--;
            //            if (enemylife == 0)
            //            {
            //                Destroy(hit.collider.gameObject, time);

            //            }

            //            Debug.Log("life=" + enemylife);


            //        }

            //    }

            //}
            Debug.DrawRay(bulletpos.transform.position, dir * range, Color.green);


        }
        UpdateGunINfo();
        GetComponent<GunAnim>().StartGunAnimation();

    }
    IEnumerator reloadtime()
    {
        BulletsForReload = BulletCapacity - CurrentAmmo;
        if (Max_Ammo < BulletsForReload)

        {
            BulletsForReload = Max_Ammo;
        }
        isReloading = false;
        yield return new WaitForSeconds(Timeforreload);
        //CurrentAmmo = Mathf.Min(Max_Ammo, CurrentAmmo + BulletsForReload) ;
        //Max_Ammo = Mathf.Max(0, Max_Ammo - BulletsForReload);
        CurrentAmmo = CurrentAmmo + BulletsForReload;
        UpdateGunINfo();
        Max_Ammo = Max_Ammo - BulletsForReload;
        //CurrentAmmo = BulletCapacity;
        // Max_Ammo =Mathf.Max(0,Max_Ammo - BulletCapacity);
        // Max_Ammo =  Max_Ammo - BulletCapacity;

        // Max_Ammo = Max_Ammo - BulletsForReload;

        isReloading = true;
    }
    public void UpdateGunINfo()
    {
        GameInfoDisplay.instance.currentAmmo.text = CurrentAmmo.ToString();
        GameInfoDisplay.instance.maxAmmo.text = Max_Ammo.ToString();
    }
    IEnumerator bulletdelay()
    {
        CanShoot = false;
        yield return new WaitForSeconds(shootDelay);
        CanShoot = true;
    }
    IEnumerator Showspark()// spark in bullet
    {

        bulletspark.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        bulletspark.SetActive(false);

    }


}
