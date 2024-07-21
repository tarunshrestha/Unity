using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChasingRandomLocate : MonoBehaviour
{
    public GameObject obstacle;
    //float x_pos = 1;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject[] car;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 prevPosition = Vector3.zero;
        Vector3 spawmPosition = Vector3.zero;
        //for (int i = 0; i < 2; i++)
        //{
            
        //    spawmPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        //    while (spawmPosition == prevPosition) {
        //        spawmPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        //    }
        //    Instantiate(obstacle,spawmPosition, Quaternion.identity);
        //    prevPosition = spawmPosition;
        //    //x_pos = x_pos + 6;
        //}

        for (int i = 0; i < 2; i++)
        {

            spawmPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            while (spawmPosition == prevPosition)
            {
                spawmPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            car[i].transform.position = spawmPosition;
            prevPosition = spawmPosition;
            //x_pos = x_pos + 6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
