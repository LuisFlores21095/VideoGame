using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextArrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("colliding");
        if (col.gameObject.CompareTag("Player"))
        {
            if(SceneManager.GetActiveScene().name == "SampleScene")
            {
                SceneManager.LoadScene("Spooky");
            }
            else if(SceneManager.GetActiveScene().name == "Spooky")
            {
                SceneManager.LoadScene("castle");
            }
            else
            {
                SceneManager.LoadScene("mainMenu");
            }
        }
    }
}
