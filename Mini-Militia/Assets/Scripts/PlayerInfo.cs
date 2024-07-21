using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]int id;
    [SerializeField] string playerName;
    [SerializeField] float playerLife;
    [SerializeField] float playerBoost;
    public TMP_InputField playerNameInputField;

    public string PlayerName
    {
        get { return playerName; }
    }

    private void Awake()
    {
        playerNameInputField = GameObject.Find("PlayernameInput")?.GetComponent<TMP_InputField>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
