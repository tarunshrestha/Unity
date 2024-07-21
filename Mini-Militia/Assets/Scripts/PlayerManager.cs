using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Mini HealthBar
    [SerializeField] Slider miniHealthSlider;


    //Health Section
    float life = 100;
    public float Life
    {
        get { return life; }
    }

    float boost = 100;

    // Boost Section
    public float Boost
    {
        get { return boost; }
    }

    // Bomb Section 
    [SerializeField] int bomb = 30;
    [SerializeField] int poisionBomb = 2;
    [SerializeField] int timeBomb = 2;
    public int Bomb
    {
        get { return bomb; }
    }

    public int PoisionBomb
    {
        get { return poisionBomb; }
    }

    public int TimeBomb
    {
        get { return timeBomb; }
    }

    public int TotalBomb
    {
        get { return bomb + poisionBomb + timeBomb; }
    }

    // Bomb Types
    public string bombType = "";
    int bombChangeClicked = 0;

    // Start is called before the first frame update
    void Start()
    {
        miniHealthSlider = transform.Find("Canvas").Find("miniHealthBar").GetComponent<Slider>();
        if (GetComponent<NetworkObject>().IsLocalPlayer)
        {
            DeligateBombTypeBtn();
            UpdateBombDisplay();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (miniHealthSlider.transform.rotation != Quaternion.identity)
        {
            miniHealthSlider.transform.rotation = Quaternion.identity;
        }
        BombChangeAuto();

    }

    public void TakeDamage(float _damage)
    {
        if (GetComponent<NetworkObject>().IsLocalPlayer)
        {
            life -= _damage;
            UpdateLifeDisplay();
            //UpdateMiniHealthBarDisplay();

            GetComponent<PlayerNetwork>().UpdateMiniHealthBarServerRpc(life);

        }
    }

    public void RecoverHealth(float _recover)
    {
        if (GetComponent<NetworkObject>().IsLocalPlayer)
        {
            life += _recover;
            UpdateLifeDisplay();
            UpdateMiniHealthBarDisplay();
        }
    }

    public void CurrentBoost(string condition, float boostReturn)
    {
        if (GetComponent<NetworkObject>().IsLocalPlayer)
        {
            if(condition == "same")
            {
                boost = boostReturn;
            }
            else if(condition == "reduce")
            {
                boost -= boostReturn;
            }
            else
            {
                boost += boostReturn;
            }
            
            UpdateBoostDisplay();
        }
    }

    public void BombReduce(int _throw)
    {
        if (GetComponent<NetworkObject>().IsLocalPlayer)
        {
            if (bombType == "timeBomb")
            {
                timeBomb -= _throw;

            }
            else if (bombType == "poisionBomb")
            {
                poisionBomb -= _throw;

            }
            else
            {
                bomb -= _throw;
            }
            UpdateBombDisplay();
            //UpdateMiniHealthBarDisplay();
        }
    }

   

   

    //void DeligateBombBtn()
    //{
    //    GameObject.Find("BombBtn").GetComponent<Button>().onClick.AddListener(() => ThrowBomb(bomb));
    //}
    void BombChangeAuto()
    {
        if (poisionBomb == 0 && bombType == "poisionBomb")
        {
            if (bomb != 0)
            {
                bombType = "granade";
            }
            else
            {
                bombType = "timeBomb";
            }
            UpdateBombDisplay();
        }
        else if (timeBomb == 0 && bombType == "timeBomb")
        {
            if (bomb != 0)
            {
                bombType = "granade";
            }
            else
            {
                bombType = "poisionBomb";
            }
            UpdateBombDisplay();
        }
        else 
        {
            if (bomb == 0)
            {
                if (poisionBomb != 0)
                {
                    bombType = "poisionBomb";
                }
                else if (timeBomb != 0)
                {
                    bombType = "timebomb";
                }
                else
                {
                    bombType = "";
                }
                UpdateBombDisplay();
            }
                
        }


    }
    private void ChangeBombType()
    {
        if (poisionBomb == 0)
        {
            bombChangeClicked+= 2;
        }
        else
        {
            bombChangeClicked++;
        }

        if (bombChangeClicked == 1 && poisionBomb > 0)
        {
            bombType = "poisionBomb";
            Debug.Log($"Type : {bombType}");
        }
        else if (bombChangeClicked == 2 && timeBomb > 0)
        {
            bombType = "timeBomb";
            Debug.Log($"Type : {bombType}");
            if (bomb == 0)
            {
                bombChangeClicked = 0;
            }
        }
        else if (TotalBomb == 0)
        {
            bombType = "empty";
        }
        else
        {
           bombType = "granade";
           bombChangeClicked = 0;
            Debug.Log($"Type : {bombType}");
        }

        
        Debug.Log($" bombChangeClicked :{bombChangeClicked}");
        GameInfoDisplay.instance.UpdateBombTypeBtn(bombType);
    }

    public void DeligateBombTypeBtn()
    {
        GameObject.Find("BombTypeBtn").GetComponent<Button>().onClick.AddListener(() => ChangeBombType());

    }


    // Display Updating Section
    void UpdateLifeDisplay()
    {
        //Debug.Log("Health : " + life);
        GameInfoDisplay.instance.UpdateHealthBar(life);
        UpdateMiniHealthBarDisplay();
    }

    void UpdateBoostDisplay()
    {
        //Debug.Log("Boost:" + boost);
        GameInfoDisplay.instance.UpdateBoostBar(boost);
    }

    void UpdateBombDisplay()
    {
        GameInfoDisplay.instance.UpdateBombBtn(TotalBomb);
        GameInfoDisplay.instance.UpdateBombTypeBtn(bombType);
    }

    void UpdateMiniHealthBarDisplay()
    {
        GetComponent<PlayerNetwork>().UpdateMiniHealthBarServerRpc(life);
    }
    public void UpdateMiniHealthBar(float _life)
    {
        miniHealthSlider.value = _life;
    }

}
