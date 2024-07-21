using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerController : MonoBehaviour
{
    
    Rigidbody2D rb;
    // Raycast
    public GameObject groundRayObject;
    [SerializeField] bool canplay = true;

    // Joystick
    [SerializeField]  float speed = 30; // movement speed
    [SerializeField] FloatingJoystick movementJoystick;
    [SerializeField] FixedJoystick aimJoystick;
    [SerializeField] Vector2 move ;

    public bool canBoost = true;
    [SerializeField] private bool groundTouched; // ground touched

    //Boost Section
    [SerializeField] float fly = 80; //range of jump
    //public float boost; //boost bar
    //private float fullBoost = 5; //boost limit
    [SerializeField]private bool restoreBoost; //restore boost
    float reduceBoost = 20;


    // Trails
    [SerializeField] bool boostOff; // leg boost off
    GameObject trail;
    public GameObject RightTrail;
    public GameObject LeftTrail;

    IEnumerator aa;
    [SerializeField] bool canRotate;

    // Aim
    public bool doubleGun; // if there is  gun in both hand
    [SerializeField] GameObject hand;
    [SerializeField] GameObject backHand;
    [SerializeField] GameObject head;
    float headRotationZ;

    // Bomb Section
    public GameObject bombLauncher;
    public GameObject bombMark;
    public Vector3 shootDirection;
    //public int bombs = 3;

    // Animation
    Animator anim;

    public GunController gunController;
    public GunManager gunManager;
    bool isLocalPlayer;
    PlayerNetwork playerNetwork;
    [SerializeField] GameObject playerForCamera;

    // Player Mangaer
    PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = transform.GetComponent<PlayerManager>();
        playerNetwork = GetComponent<PlayerNetwork>();
        transform.localScale = new Vector3(1, 1, 1) * 0.3731576f;
        rb = GetComponent<Rigidbody2D>();
        boostOff = true;
        canRotate = true;
        anim = GetComponent<Animator>();
        movementJoystick = GameObject.Find("MovementFloatingJoystick").GetComponent<FloatingJoystick>();
        aimJoystick = GameObject.Find("AimJoystick").GetComponent<FixedJoystick>();
        if (GetComponent<NetworkObject>().IsLocalPlayer)
        {
            isLocalPlayer = true;
            Camera.main.GetComponent<CameraFollow>().mainPlayer = this.gameObject;
            Camera.main.GetComponent<CameraFollow>().playerAim = playerForCamera;
            DeligateBombBtn();
        }
        gunController.playerNetwork = playerNetwork;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            rb.gravityScale = 0;
            return;
        }
        if (canplay)
        {
            if (GetComponent<PlayerManager>().Life <= 0 || transform.position.y < -60)
            {
                canplay = false;
                playerManager.TakeDamage(100);
            }
            if (canBoost)
            {
                // Boost Key
                Boost();

                //Restore Boost
                if (restoreBoost && 100 > playerManager.Boost)
                {
                    playerManager.CurrentBoost("add", 7 * Time.deltaTime);
                    //boost += 0.55f * Time.deltaTime;
                }

                //Ground Touched
                if (groundTouched)
                {
                    //move.y = 0;
                    canRotate = false;
                    boostOff = true;
                    //restoreBoost = true;
                }



                JoystickMovement(); // Movement
                                    //BoostBar(); // For Boost
                RayCastGroundHitDistance(); // For player Distance to ground
            }
            if (aimJoystick.Horizontal != 0 && aimJoystick.Vertical != 0)
            {
                AimRotation();
            }

            // Health Recovery
            if (playerManager.Life < 100 && playerManager.Life !<= 0)
            {
                playerManager.RecoverHealth(10);
            }
        }
        else 
        {
            if(anim.GetBool("backwalk") == true || anim.GetBool("walk") == true)
            {
                anim.SetBool("walk", false);
                anim.SetBool("backwalk", false);
            }
            
        }


    }

    void FixedUpdate()
    {
        if (canplay)
        {
            if (move.x != 0)
            {
                if (groundTouched)
                {

                    Vector2 _newpos = new Vector2(rb.position.x + move.x * speed * Time.deltaTime, rb.position.y);
                    rb.position = Vector2.Lerp(rb.position, _newpos, 5 * Time.fixedDeltaTime);
                }
                else
                {
                    if( move.y > 0.1f)
                    {
                        rb.AddForce(new Vector2(move.x * 40, move.y));
                    }
                    else
                    {
                        rb.AddForce(new Vector2(move.x * 10, move.y));
                        Debug.Log("Down");
                    }
                }
            }
            if (move.y > 0)
            {
                rb.AddForce(new Vector2(move.x, move.y * fly));
            }

            //Debug.Log($"speed: x={move.x}, y={move.y}");

            // Rotation
            PlayerRotation();
            
            
        }



    }

    // Joystick Controllers
    void JoystickMovement()
    {
        move.x = movementJoystick.Horizontal; // For movements
        if (playerManager.Boost > 0.5) //for Boosting 
        {
            if (groundTouched)
            {
                if (playerManager.Boost > 0.5f)
                {
                    move.y = movementJoystick.Vertical;
                }
            }
            else
            {
                if (move.y <= 0)
                {
                    //return;
                    move.y = 0.01f;
                }
                else
                {
                    move.y = movementJoystick.Vertical;
                }
                
            }
        }
        else
        {
            move.y = 0.01f;
            canRotate = false;
        }

        // Pull to Boost
        float moveDistance = Vector2.Distance(Vector2.zero, new Vector2(movementJoystick.Horizontal, movementJoystick.Vertical));
        if (moveDistance >= 0.95 && reduceBoost == 8)
        {
            reduceBoost = 13;
        }
        else
        {
            reduceBoost = 8;
        }


        //Move Animation
        Vector3 aimPosition = new Vector3(aimJoystick.Horizontal, aimJoystick.Vertical, 0) * 10000;
        if (move.x != 0 || move.y != 0)
        {
            if (transform.localScale.x > 0 && move.x > 0)
            {
                anim.SetBool("walk", true);
                anim.SetBool("backwalk", false);
            }
            else if (transform.localScale.x < 0 && move.x < 0)
            {
                anim.SetBool("walk", true);
                anim.SetBool("backwalk", false);

            }
            else if (move.y > 0.2)
            {
                anim.SetBool("walk", true);
                anim.SetBool("backwalk", false);
            }
            else if (transform.localScale.x < 0 && move.x > 0)
            {
                anim.SetBool("backwalk", true);
                anim.SetBool("walk", false);

            }
            else if (transform.localScale.x > 0 && move.x < 0)
            {
                anim.SetBool("backwalk", true);
                anim.SetBool("walk", false);
            }
            else
            {
                anim.SetBool("backwalk", false);
                anim.SetBool("walk", false);
            }

        }
        else
        {
            //anim.SetBool("idle", true);
            anim.SetBool("backwalk", false);
            anim.SetBool("walk", false);
        }
    }

    void Boost() // For Flying
    {
        if ((movementJoystick.Vertical > 0) && playerManager.Boost >= 0) // boost reduction and functions
        {
            //boost = (boost < 0) ? 0 : boost;
          

            if (movementJoystick.Vertical > 0 && !groundTouched)
            {
                restoreBoost = false;
                boostOff = false;
                playerManager.CurrentBoost("reduce", reduceBoost * Time.deltaTime);
                //boost -= reduceBoost * Time.deltaTime;
            }
            else
            {
                boostOff = true;
                restoreBoost = true;
            }
        }
        else
        {
            boostOff = true;
            move.y = 0f;
            restoreBoost = true;
        }

        if (boostOff) // Boost trails
        {
            RightTrail.SetActive(false);
            LeftTrail.SetActive(false);
        }
        else
        {
            RightTrail.SetActive(true);
            LeftTrail.SetActive(true);
        }

        if (playerManager.Boost <= 0) // Boost trail off
        {
            boostOff = true;
        }
    }

    private void PlayerRotation()
    {
        float hAxis = move.x;
        float vAxis = move.y;
        if (canRotate)
        {

            if (vAxis < 0.2f)
            {
                vAxis = 0.2f;
            }
            float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0f, 0, -zAxis);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1 * Time.deltaTime);
        }
        else
        {
            Quaternion rot = Quaternion.Euler(0f, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 2 * Time.deltaTime);
        }
    }
    void AimRotation()
    {
        // pull to shoot
        float distance = Vector2.Distance(Vector2.zero, new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical));
        if (distance >= 0.75)
        {
            //Debug.Log("Shoot");
            //gunController.Shoot();
            //GetComponent<PlayerNetwork>().RequestShootServerRpc();
            //gunController.RequestShoot();
            gunManager.Shoot();
        }

        Vector3 aimPosition = new Vector3(aimJoystick.Horizontal, aimJoystick.Vertical, 0) * 10000;

        // Front Hand Aim
        Vector3 look = hand.transform.InverseTransformPoint(aimPosition);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        hand.transform.Rotate(0, 0, angle - 41);

        // Back Hand Aim
        if (doubleGun)
        {
            Vector3 backAim = backHand.transform.InverseTransformPoint(aimPosition);
            float backAngle = Mathf.Atan2(backAim.y, backAim.x) * Mathf.Rad2Deg;
            backHand.transform.Rotate(0, 0, backAngle - 41);
        }

        //Head Aim
        Vector3 headLook = head.transform.InverseTransformPoint(aimPosition);
        float headAngle = Mathf.Atan2(headLook.y, headLook.x) * Mathf.Rad2Deg;
        headRotationZ = Mathf.Clamp((headRotationZ + headAngle + 90), -50, 36);
        //head.transform.Rotate(0, 0, headAngle);
        head.transform.localEulerAngles = new Vector3(0, 0, headRotationZ);

        if (transform.position.x > aimPosition.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void RayCastGroundHitDistance() // For ground check
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(groundRayObject.transform.position, -Vector2.up);
        Debug.DrawRay(groundRayObject.transform.position, -Vector2.up * raycastHit.distance, Color.red);
        //Debug.Log(raycastHit.distance + "m of ground distance.");
        //Debug.Log($"Distance: " + raycastHit.distance);
        if(raycastHit.distance <= 2)
        {
            if(canRotate) 
            {
                canRotate = false;
            }
            if (raycastHit.distance <= 0 && raycastHit.collider.tag == "ground") 
            {
                //move.y = 0;
                groundTouched = true;
                boostOff = true;
                //speed = 5;
                //restoreBoost = true;
                if (aa != null)
                {
                    StopCoroutine(aa);
                }

            }
            else
            {
                //speed = 10;
                groundTouched = false;
                restoreBoost = false;
                aa = MakeGroundTouchFalse();
                StartCoroutine(aa);
            }
        }
        else
        {
            groundTouched = false;

            if (canRotate == false)
            {
                canRotate = true;
            }
        }
        
    }

    IEnumerator MakeGroundTouchFalse()
    {
        yield return new WaitForSeconds(0.1f);
        //canRotate = true;
    }

    public void ThrowBomb()
    {
        if (playerManager.TotalBomb != 0 && canplay)
        {
            if (playerManager.bombType == "poisionBomb" && playerManager.PoisionBomb > 0)
            {
                bombLauncher.GetComponent<BombManager>().bombType = BombManager.BombTypes.Poision;
               
            }
            else
            {
                bombLauncher.GetComponent<BombManager>().bombType = BombManager.BombTypes.Granade;
            }

            Vector3 throwDirection = (bombMark.transform.GetChild(0).transform.position - bombMark.transform.position).normalized;
            //Debug.Log(throwDirection);
            GameObject newBomb = Instantiate(bombLauncher, bombMark.transform.position, transform.rotation) as GameObject;
            newBomb.GetComponent<Rigidbody2D>().AddForce(throwDirection * 1.5f, ForceMode2D.Impulse);
            playerManager.BombReduce(1);
        }
    }

    void CheckBombandThrow()
    {
        if (playerManager.TotalBomb != 0)
        {
            ThrowBomb();
            playerNetwork.ThrowBombServerRpc();
        }
    }

    void DeligateBombBtn()
    {
        GameObject.Find("BombBtn").GetComponent<Button>().onClick.AddListener(() =>
        CheckBombandThrow());
    }

}
