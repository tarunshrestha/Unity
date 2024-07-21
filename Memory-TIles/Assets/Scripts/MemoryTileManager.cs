using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemoryTileManager : MonoBehaviour
{
    public GameObject[] Tiles;
    List<int> usedNumbers = new List<int>() {};
    List<int> leftNumbers = new List<int>() {};

    public GameObject player1;
    public GameObject player2;

    int p1Score = 0;
    int p2Score = 0;

    public TMP_Text player1ScoreDisplay;
    public TMP_Text player2ScoreDisplay;
    public TMP_Text timerDisplay;


    public TMP_Text winDisplay;

    public GameObject safeTileDisplay;
    int safeTile;
    public int noOfSafetiles = 4;

    public Sprite apple;
    public Sprite banana;
    public Sprite cherry;

    public Button startBtn;
    public Button restartBtn;

    // Start is called before the first frame update
    void Start()
    {
        ResetTiles();
        timerDisplay.text = "";
        startBtn.gameObject.SetActive(true);
        restartBtn.gameObject.SetActive(false);
        //SelectWinTile();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))  //Start Game
        {
            StartCoroutine(StartGame());
        }



        if (Input.GetKeyDown(KeyCode.R))  //Start Game
        {
            SceneManager.LoadScene("MemoryTile");
        }


        if (p1Score == 3)
        {
            StopAllCoroutines();
            PlayerStop();
            winDisplay.text = "Player 1 has won..";
            winDisplay.gameObject.SetActive(true);
            restartBtn.gameObject.SetActive(true);
        }
        else if(p2Score == 3)
        {
            StopAllCoroutines();
            PlayerStop();
            winDisplay.text = "Player 2 has won..";
            winDisplay.gameObject.SetActive(true);
            restartBtn.gameObject.SetActive(true);
        }

    }

    void SelectWinTile()
    {
        safeTileDisplay.gameObject.SetActive(false);
        safeTile = Random.Range(0, 3);
        //Debug.Log(safeTile); 

        if (safeTile == 0)
        {
            safeTileDisplay.GetComponent<SpriteRenderer>().sprite = banana;
        }
        else if(safeTile == 1)
        {
            safeTileDisplay.GetComponent<SpriteRenderer>().sprite = cherry;
        }
        else
        {
            safeTileDisplay.GetComponent<SpriteRenderer>().sprite = apple;
        }
    }

    void SelectWinTileDisplay()
    {
        safeTileDisplay.gameObject.SetActive(true);
    }

    public void ResetTiles()
    {
        timerDisplay.text = "";
        usedNumbers.Clear();
        leftNumbers.Clear();
        for (int i = 0; i < Tiles.Length; i++)  //Resetting all lists and functions
        {
            Tiles[i].GetComponent<PolygonCollider2D>().enabled = false;
            Tiles[i].transform.GetChild(0).gameObject.SetActive(false);
            Tiles[i].GetComponent<SpriteRenderer>().color = Color.white;

        }
    }

   
    
    void AddArray()
    {
        for (int i = 0; i < noOfSafetiles ; i++) // To add random numbers in used list
        {
            int num = Random.Range(0, Tiles.Length );
            if (usedNumbers.Contains(num))
            {
                num = Random.Range(0, Tiles.Length);
            }
            else
            {
                usedNumbers.Add(num);
            }
        }

        for (int i = 0; i < Tiles.Length; i++) // To add un-used numbers in leftlist
        {
            if (!usedNumbers.Contains(i))
            {
                leftNumbers.Add(i);
            }
        }
    }

    void ShowTrap()
    {
        foreach (int num in leftNumbers)  // Numbers in left list are activating collider
        {
            // Debug.Log("leftNumbers " + num);
            if (safeTile == 0)
            {
                if (num % 2 == 0)
                {
                    Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = apple;
                }
                else
                {
                    Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cherry;
                }
            }
            else if (safeTile == 1)
            {
                if (num % 2 == 0)
                {
                    Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = banana;
                }
                else
                {
                    Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = apple;
                }
            }
            else
            {
                if (num % 2 == 0)
                {
                    Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = banana;
                }
                else
                {
                    Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cherry;
                }
            }

            
            Tiles[num].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void HideTrap()
    {
        foreach (int num in leftNumbers)  // Numbers in left list are activating collider
        {
            Tiles[num].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void StartTrap()
    {
        foreach (int num in leftNumbers) 
        {
            Tiles[num].GetComponent<PolygonCollider2D>().enabled = true;
            Tiles[num].GetComponent<SpriteRenderer>().color=Color.red;

        }
    }

    void ShowSafeTile()
    {
        foreach (int num in usedNumbers)  // Numbers in used list are activating picture
        {
            //Debug.Log("usedNumbers " + num);
            if (safeTile == 0)
            {
                Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = banana;
            }
            else if (safeTile == 1)
            {
                Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cherry;
            }
            else
            {
                Tiles[num].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = apple;
            }
            Tiles[num].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void HideSafeTile()
    {
        foreach (int num in usedNumbers)  // Numbers in used list are activating picture
        {
            Tiles[num].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void PlayerCanplay()
    {
        player1.GetComponent<MemoryTilePlayerController>().canplay = true;
        player2.GetComponent<MemoryTilePlayerController>().canplay = true;

        player1.GetComponent<MemoryTilePlayerController>().isAlive = true;
        player2.GetComponent<MemoryTilePlayerController>().isAlive = true;

    }
    void PlayerStop()
    {
        player1.GetComponent<MemoryTilePlayerController>().canplay = false;
        player2.GetComponent<MemoryTilePlayerController>().canplay = false;
    }

    void PlayerDies()
    {
        if (player1.GetComponent<MemoryTilePlayerController>().isAlive == false && player2.GetComponent<MemoryTilePlayerController>().isAlive == true)
        {
            //PlayerStop();
            //Debug.Log("Player 2 Won...");
            p2Score ++;
            player2ScoreDisplay.text = p2Score + "";
            StartCoroutine(DrawGame());

        }
        else if(player1.GetComponent<MemoryTilePlayerController>().isAlive == true && player2.GetComponent<MemoryTilePlayerController>().isAlive == false)
        {
            //PlayerStop();
            //Debug.Log("Player 1 Won...");
            p1Score++;
            player1ScoreDisplay.text = p1Score + "";
            StartCoroutine(DrawGame());

        }
        else
        {
            Debug.Log("Draw");

            StartCoroutine(DrawGame());
        }
    }
    public void StartGameBtn()
    {
        startBtn.gameObject.SetActive(false);

        StartCoroutine(StartGame());
    }

    public void RestartBtn()
    {
        SceneManager.LoadScene("MemoryTile");
    }
    IEnumerator StartGame()
    {
        Debug.Log("GameStart");
        //PlayerStop();
        PlayerCanplay();
        ResetTiles();
        SelectWinTile();
        yield return new WaitForSeconds(1);
        AddArray();
        ShowSafeTile();
        ShowTrap();
        yield return new WaitForSeconds(2);
        //PlayerCanplay();
        HideTrap();
        HideSafeTile();
        yield return new WaitForSeconds(2);
        SelectWinTileDisplay();
        timerDisplay.text = "3";
        yield return new WaitForSeconds(1);
        timerDisplay.text = "2";
        yield return new WaitForSeconds(1);
        timerDisplay.text = "1";
        yield return new WaitForSeconds(1);
        timerDisplay.text = "0";
        ShowSafeTile();
        ShowTrap();
        StartTrap();
        yield return new WaitForSeconds(1);
        PlayerDies();
    }

    IEnumerator DrawGame()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("New Game");
        yield return new WaitForSeconds(3);
        StartCoroutine(StartGame());
    }


}
