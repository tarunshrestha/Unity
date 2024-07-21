using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class SniperShoot : MonoBehaviour
{

    public GameObject bulletspark;


    public GameObject blood; 
    [SerializeField] Transform bulletpos, bulletpos1;
    [SerializeField] Transform[] bulletFinalDirPos;
    [SerializeField] float shootDelay;
    private Rigidbody2D rb;
    //  [SerializeField] Animator anim;
    private float enemylife = 5f;

    //float bulletlifetime;
    private bool CanShoot = true;
    [SerializeField] int Max_Ammo, CurrentAmmo, BulletCapacity;

    [SerializeField] int Timeforreload;
    private bool isReloading = true;

    private int BulletsForReload;
    private float time;

    public GameObject Sniper;
    public float range;// for debug.drawray
    private LineRenderer bulletTrail;
    public Transform lineMaxPos;
    public PlayerNetwork playerNetwork;
    void Start()
    {
        CurrentAmmo = BulletCapacity;
        UpdateGunINfo();
        GameInfoDisplay.instance.currentGunImage.sprite = GetComponent<GunInfo>().gunSprite;
        Debug.Log("GunGrabbed at start = " + GetComponent<GunInfo>().guntype);

        //anim = GetComponent<Animator>();
        bulletTrail = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    reload();
        //}


        if (CurrentAmmo <= 0 && isReloading == true)
        {
            Debug.Log("NO AMMO");
            reload();
        }


        //if (Input.GetMouseButton(0) && CanShoot == true && CurrentAmmo != 0)
        //{

        //    Shoot();



        //}

    }
    public void RequestShoot()
    {
        if (!CanShoot || CurrentAmmo == 0)
            return;
        Shoot();
       
        playerNetwork.RequestShootServerRpc();
     
    }
    void reload()
    {
        StartCoroutine(reloadtime());

    }
        public void Shoot()
    {
        BulletsForReload = BulletCapacity - CurrentAmmo; // no. of bullets added to the gun after reload
        CurrentAmmo--;
        for (int i = 0; i < bulletFinalDirPos.Length; i++)
        {
            //anim.SetTrigger("shoot");


            StartCoroutine(Showspark());
            StartCoroutine(bulletdelay());
            Vector2 dir = (bulletFinalDirPos[i].position - bulletpos.position).normalized;
            // Vector2 dir = (bulletpos1.position - bulletpos.position).normalized; 
            //direction of bullet

            // GameObject goli = Instantiate(bullet, bulletpos.position, transform.rotation);

            // Destroy(goli, bulletlifetime);

            // goli.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
            RaycastHit2D hit;

            hit = Physics2D.Raycast(bulletpos.transform.position, dir);



            if (hit.collider != null)
            {

                LineRenderer bulletTrail = Sniper.GetComponent<LineRenderer>();
                bulletTrail.enabled = true;

                bulletTrail.SetPosition(0, bulletpos.position);
                bulletTrail.SetPosition(1, hit.point);
                GameObject bloodSpark = Instantiate(blood, hit.point, Quaternion.identity);


               // Debug.Log("point of bullet collision= " + (hit.point));
                StartCoroutine(bulletDisappear());
               // Debug.Log("Distance of raycast " + hit.distance);
               // Debug.Log("collision of raycast with = " + hit.transform.name);
                if (hit.collider.tag == "enemy" && hit.distance < range)
                {



                    if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                    {

                        float distance = hit.distance;
                        time = distance / 10000f;
                        Debug.Log("distance between raycast and object = " + hit.distance);

                        //enemylife = 0;

                        //{
                        //    Destroy(hit.collider.gameObject, time);

                        //}
                        StartCoroutine(damageTime(time, hit));
                        StartCoroutine(bloodSparkaftercertaintime(time, hit));
                        Debug.Log("life=" + enemylife);


                    }

                }

            }
            else
            {

                LineRenderer bulletTrail = Sniper.GetComponent<LineRenderer>();
                bulletTrail.enabled = true;
                // bulletTrail.useWorldSpace = false;
                bulletTrail.SetPosition(0, bulletpos.position);
                bulletTrail.SetPosition(1, lineMaxPos.position);
                Debug.Log("Max position of bullet= " + bulletpos.localPosition + (dir * range));
                StartCoroutine(bulletDisappear());
            }
            Debug.DrawRay(bulletpos.transform.position, dir * range, Color.green);


        }
        UpdateGunINfo();
        GetComponent<GunAnim>().StartGunAnimation();

    }
    IEnumerator bloodSparkaftercertaintime(float _time, RaycastHit2D _hit)
    {
        Debug.Log("Delay before blood spark: " + _time);
        yield return new WaitForSeconds(_time);
        GameObject bloodSpark = Instantiate(blood, _hit.point, Quaternion.identity);
        Destroy(bloodSpark, 1f);
    }
    IEnumerator damageTime(float _time, RaycastHit2D _hit)
    {
        yield return new WaitForSeconds(_time);
        _hit.collider.GetComponent<PlayerManager>().TakeDamage(50);
    }
    IEnumerator bulletDisappear()
    {
       // Debug.Log("Disappear BUllet");

        yield return new WaitForSeconds(.1f);
        bulletTrail.enabled = false;
       // Debug.Log("Disappear BUllet........");
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
    IEnumerator Showspark() // spark in bullet
    {

        bulletspark.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        bulletspark.SetActive(false);

    }

}
