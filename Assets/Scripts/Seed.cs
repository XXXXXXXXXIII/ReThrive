﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public Ghost ghost { get; set; }

    private PlayerState PS;

    private void Start()
    {
        ghost = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO: Prompt player that they can modify the ghost in seed
            PS = other.GetComponent<PlayerState>();
            if (!PS.isRecording)
            {
                Debug.Log("Seed::Player touching seed");
                PS.onPlant += OnInteract;
                PS.onSeed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Seed::Player not touching seed");
            PS.onSeed = false;
            PS.onPlant -= OnInteract;
        }
    }

    public void OnInteract()
    {
        ghost.gameObject.transform.position = new Vector3(10000, 10000, 10000);
        //ghost.gameObject.GetComponent<Renderer>().enabled = false;
        Destroy(ghost.gameObject, 0.5f);
        ghost = null;
        PS.currSeed = this;
    }
}
