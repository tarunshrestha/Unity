using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < -40)
        {
            GameOver();
        }

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            //Zoom();
        }
    }

    void GameOver()
    {
        player.GetComponent<PlayerController>().canBoost = false;
        StartCoroutine(GameOverCount());
    }

    IEnumerator GameOverCount()
    {
        yield return new WaitForSeconds(2);
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Stage1");
    }


}
