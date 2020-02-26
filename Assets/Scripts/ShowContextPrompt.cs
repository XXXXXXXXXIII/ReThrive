using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowContextPrompt : MonoBehaviour
{
    public GameObject contextPrompt;
    void Start()
    {
        contextPrompt.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player") 
        {
            contextPrompt.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player") 
        {
            contextPrompt.SetActive(false);
        }
    }

}
