using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
    public GunName guntype;
    public Sprite gunSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum GunName
    {
        ArGun,
        ShotGun,
        Sniper,
        Launcher

    }
}
