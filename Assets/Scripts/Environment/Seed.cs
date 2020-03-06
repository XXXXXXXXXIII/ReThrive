using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public Ghost ghost { get; set; }

    public string PromptText = "Press X or E to replant seed";

    private PlayerState PS;
    private HeadsUpDisplay HUD;
    private GhostManager GM;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO: Prompt player that they can modify the ghost in seed
            PS = other.GetComponent<PlayerState>();
            HUD = other.GetComponent<HeadsUpDisplay>();
            GM = other.GetComponent<GhostManager>();
            if (!GM.isRecording)
            {
                Debug.Log("Seed::Player touching seed");
                HUD.PushPrompt(PromptText);
                PS.onInteractEnd += OnInteract;
                PS.onSeed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUD.PopPromptOnMatch(PromptText);
            Debug.Log("Seed::Player not touching seed");
            PS.onSeed = false;
            PS.onInteractEnd -= OnInteract;
        }
    }

    public void OnInteract()
    {
        //ghost.gameObject.transform.position = new Vector3(10000, 10000, 10000);
        //ghost.gameObject.GetComponent<Renderer>().enabled = false;
        //Destroy(ghost.gameObject, 0.5f);
        //ghost = null;
        ghost.ResetGhost();
        PS.currSeed = this;
    }
}
