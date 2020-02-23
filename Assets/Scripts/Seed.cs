using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public Ghost ghost { get; set; }
    private PlayerState PS;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO: Prompt player that they can modify the ghost in seed
            Debug.Log("Seed::Player touching seed");
            PS = other.GetComponent<PlayerState>();
            PS.onInteract += OnInteract;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Seed::Player not touching seed");
            PS.onInteract -= OnInteract;
        }
    }

    public void OnInteract()
    {
        Destroy(this.ghost.gameObject);
    }
}
