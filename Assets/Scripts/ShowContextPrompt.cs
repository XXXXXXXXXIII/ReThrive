using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowContextPrompt : MonoBehaviour
{
    public GameObject contextPrompt;
    void Start()
    {
        Debug.Log("starting context");
        contextPrompt.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("enter");
        if (collision.collider.tag == "Player") 
        {
            contextPrompt.SetActive(true);
        }
    }

    void OnTriggerEnter (Collider player) {
        Debug.Log("enter trigger");
        if (player.gameObject.tag == "Player") {
            contextPrompt.SetActive(true);
        }
    }
    void OnTriggerExit (Collider player) {
        Debug.Log("enter trigger");
        if (player.gameObject.tag == "Player") {
            contextPrompt.SetActive(false);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player") 
        {
            contextPrompt.SetActive(false);
        }
    }

}
