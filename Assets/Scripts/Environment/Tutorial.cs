using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public bool useTrigger = true;
    //public bool useSequence = false;
    public string tutorialText = "";
    HeadsUpDisplay HUD;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUD = other.gameObject.GetComponent<HeadsUpDisplay>();
            HUD.SetTutorial(tutorialText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUD.SetTutorial("");
        }
    }
}
