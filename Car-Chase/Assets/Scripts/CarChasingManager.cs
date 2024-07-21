using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CarChasingManager : MonoBehaviour
{
    public Camera cameraShake;
    public TMP_Text winnerUI;
    public Button playButton;
    AudioSource blastAudio;
    AudioSource bgSound;
    //public bool isSlow = false; // Slow motion


    //=============players============
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    //=============enemy============
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;

    // Start is called before the first frame update
    void Start()
    {
        winnerUI.gameObject.SetActive(false);
        //StartGame();
        bgSound = transform.Find("BGSound").GetComponent<AudioSource>();
        blastAudio = transform.Find("blastSound").GetComponent<AudioSource>();
        bgSound.Play();

        //blastSound = transform.Find("blastSound").GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("CarChasingScene");
        }

        //if(isSlow)
        //{
        //    SlowMotion();
        //}


    }



    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("CarChasingScene");


    }

    public void StartGame()
    {

        bgSound.Stop();
        //=============================player=============================
        player1.GetComponent<CarChasingPlayerController>().StartPlayer();
        player2.GetComponent<CarChasingPlayerController>().StartPlayer();

        //=============================Enemy=============================
        enemy1.GetComponent<CarChasingEnemy>().canplay = true;
        enemy1.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;


        enemy2.GetComponent<CarChasingEnemy>().canplay = true;
        enemy2.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;


        playButton.gameObject.SetActive(false);
        //policeAudio.Play();

    }


    public void GameOver()
    {
        //===================shake Camera===========
        cameraShake.GetComponent<CarChasingCamera>().CameraShake();

        bgSound.Play();
        //=============================player=============================
        player1.GetComponent<CarChasingPlayerController>().canplay = false;
        player2.GetComponent<CarChasingPlayerController>().canplay = false;

        //=============================Enemy=============================
        enemy1.GetComponent<CarChasingEnemy>().canplay = false;
        enemy1.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        enemy2.GetComponent<CarChasingEnemy>().canplay = false;
        enemy2.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;


        //isSlow = true;
        blastAudio.Play();


        StartCoroutine(ReloadScene());
    } 

    public void WonPlayer1()
    {
        winnerUI.gameObject.SetActive(true);
        winnerUI.text = "Player 1 has won.";
    }
    public void WonPlayer2()
    {
        winnerUI.gameObject.SetActive(true);
        winnerUI.text = "Player 2 has won.";
    }

    //void SlowMotion()
    //{
    //    Time.timeScale = 0.10f;
    //    //Time.fixedDeltaTime = Time.timeScale * 0.02f;
    //}
   
}
