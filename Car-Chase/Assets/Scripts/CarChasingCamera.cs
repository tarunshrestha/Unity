using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChasingCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CameraShake()
    {
        StartCoroutine(ShakeCamera(.3f, .1f));

        /*if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(ShakeCamera(.15f, .4f));

        }*/
    }
    public IEnumerator ShakeCamera(float duration , float magnitude)
    {
        Vector3 originalpos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1 ,1f) * magnitude;
            float y = Random.Range(-1, 1f) * magnitude;

            
            transform.localPosition = new Vector3(x, y, transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;


        }

        transform.localPosition = originalpos;
    }
}
