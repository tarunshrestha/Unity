using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameInfoDisplay : MonoBehaviour
{
    public static GameInfoDisplay instance;

    // Sliders for health & boost
    public Slider healthSlider;
    public Slider boostSlider;

    // Bomb UI
    public Button bombBtn;
    public Button bombTypeBtn;

    public TextMeshProUGUI currentAmmo, maxAmmo;
    public Image currentGunImage;
    public Button gunGrabButton;
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(float _health)
    {
        healthSlider.value = _health;
    }

    public void UpdateBoostBar(float _boost)
    {
        boostSlider.value = _boost;
    }

    public void UpdateBombBtn(int _bomb)
    {
        bombBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _bomb + "";
    }

    public void UpdateBombTypeBtn(string bombType)
    {
        if (bombType == "poisionBomb")
        {
            bombTypeBtn.transform.GetComponent<Image>().color = Color.yellow;
        }
        else if (bombType == "timeBomb")
        {
            bombTypeBtn.transform.GetComponent<Image>().color = Color.red;
        }
        else if( bombType == "empty")
        {
            bombTypeBtn.transform.GetComponent<Image>().color = Color.white;

        }
        else
        {
            bombTypeBtn.transform.GetComponent<Image>().color = Color.blue;
        }
    }
}
