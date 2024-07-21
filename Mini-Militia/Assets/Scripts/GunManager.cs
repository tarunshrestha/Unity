using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GunInfo gunInfo;
    public GunController forArGun,forShotGun;
    public SniperShoot forSniper;
    public LauncherGun forLauncher;
    // Start is called before the first frame update
    void Start()
    {
        gunInfo = GetComponent<GunInfo>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        //Debug.Log(gunInfo.guntype);
        if(gunInfo.guntype == GunInfo.GunName.ArGun)
        {
           // Debug.Log(" ArGun is shooting");
            forArGun.RequestShoot();
            forArGun.UpdateGunINfo();
        }
        else if (gunInfo.guntype == GunInfo.GunName.Sniper)
        {

            forSniper.RequestShoot();
            forSniper.UpdateGunINfo();
          //  Debug.Log(" Sniper is shooting");
        }
        else if (gunInfo.guntype == GunInfo.GunName.ShotGun)
        {
            forShotGun.RequestShoot();
        }

        else if (gunInfo.guntype == GunInfo.GunName.Launcher)
        {
            forLauncher.RequestShoot();
        }
    }

    public void ShootforClient()
    {
        if (gunInfo.guntype == GunInfo.GunName.ArGun)
        {
            forArGun.Shoot();
        }
        else if (gunInfo.guntype == GunInfo.GunName.Sniper)
        {
            forSniper.Shoot();
        }
        else if(gunInfo.guntype == GunInfo.GunName.ShotGun)
        {
            forShotGun.Shoot();
        }
        else if (gunInfo.guntype == GunInfo.GunName.Launcher)
        {
            forLauncher.Shoot();
        }
    }
    public void UpdateGunINfo()
    {
        if (gunInfo.guntype == GunInfo.GunName.ArGun)
        {
            forArGun.UpdateGunINfo();
        }
        else if (gunInfo.guntype == GunInfo.GunName.Sniper)
        {
            forSniper.UpdateGunINfo();
        }
        else if (gunInfo.guntype == GunInfo.GunName.ShotGun)
        {
            forShotGun.UpdateGunINfo();
        }
        else if (gunInfo.guntype == GunInfo.GunName.Launcher)
        {
            forLauncher.UpdateGunINfo();
        }
    }


}
