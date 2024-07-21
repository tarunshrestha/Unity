using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JumpBirdManager : MonoBehaviour
{
    public bool PlayersCanPlay;
    public GameObject Player1;
    public GameObject Player2;
    public TMP_Text winnerUI;
    public Button restartBtn;
    public Button startBtn;
    AudioSource bgSound;
    public bool playBgSound;


    //player[] allPlayer;
    // Start is called before the first frame update
    void Start()
    {
        PlayersCanPlay = true;
        winnerUI.gameObject.SetActive(false);
        restartBtn.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(true);
        bgSound = transform.GetChild(0).GetComponent<AudioSource>();
        playBgSound = true;
        bgSound.Play();

        //Player = GameObject.FindWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (!playBgSound )
        {
            bgSound.Stop();
        }


        if (!PlayersCanPlay)
        {
            //GameObject.FindObjectOfType<player>().canplay = false;
            Player1.GetComponent<JumpBirdPlayer>().canplay = false;
            Player2.GetComponent<JumpBirdPlayer>().canplay = false;
            Debug.Log("Players can't play.");
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

    }
    //-----------------------------------------Players disable-------------------------------------------------------
    public void DisablePlayer()
    {
        Player1.GetComponent<JumpBirdPlayer>().canplay = false;
        Player2.GetComponent<JumpBirdPlayer>().canplay = false;
    }


    //-----------------------------------------Players Sprite-------------------------------------------------------
    public void PlayerSpriteRegain()
    {
        if ((Player1.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled == false) || (Player1.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled == false))
        {
            Player1.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            Player1.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
        }

        if ((Player2.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled == false) || (Player2.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled == false))
        {
            Player2.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            Player2.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
        }

    }


    //-----------------------------------------Players collider-------------------------------------------------------
    public void PlayerColliderRegain()
    {
        if ((Player1.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled == false) || (Player1.transform.GetChild(2).GetComponent<BoxCollider2D>().enabled == false))
        {
            Player1.transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
            Player1.transform.GetChild(3).GetComponent<BoxCollider2D>().enabled = true;
        }

        if ((Player2.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled == false) || (Player2.transform.GetChild(2).GetComponent<BoxCollider2D>().enabled == false))
        {
            Player2.transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
            Player2.transform.GetChild(3).GetComponent<BoxCollider2D>().enabled = true;
        }

    }


    //-----------------------------------------Players WIn UI-------------------------------------------------------
    public void WinnerPlayer1()
    {
        StartCoroutine(RestartGame());
        winnerUI.text = "Player 1 has won.";
    }

    public void WinnerPlayer2()
    {
        StartCoroutine(RestartGame());
        winnerUI.text = "Player 2 has won.";
    }

    public void StartBtn()
    {
        startBtn.gameObject.SetActive(false);
    }
    public void RestartBtn()
    {
        SceneManager.LoadScene("JumpBirdScene");
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5);
        winnerUI.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(true);
    }



}
