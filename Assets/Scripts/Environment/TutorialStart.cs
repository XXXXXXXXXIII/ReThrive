using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStart : MonoBehaviour
{
    public bool useTrigger = true;
    //public bool useSequence = false;
    public string tutorialText = "";
    HeadsUpDisplay HUD;
    public float delay = 2.0f; 
    public float prevPosition;
    public float tutorialStage = -1;
    public bool initialPromptComplete = false;
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    
    void Start() {

    }
    void Update() {
        // Debug.Log(tutorialStage);
        if (tutorialStage == 0) // movement
        {
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0) {
                if (delay <= 0) {
                    tutorialStage++;
                } else {
                    delay -= Time.deltaTime;
                }
            }
        }
        if (tutorialStage == 1) // lookaround
        { 
            HUD.SetTutorial("use the right joystick to look around");
            // detect right stick movement
            if (Input.GetKey("r")) { // joystick axis
                tutorialStage++;
            }
        }
        if (tutorialStage == 2) // jump
        {
            HUD.SetTutorial("jump using X");
            if (Input.GetKey("space")) { // "joystick button 1" for X button
                Destroy(wall1);
                Destroy(wall2);
                Destroy(wall3);
                Destroy(wall4);
                delay = 5.0f;
                tutorialStage++;
            }
        }
        if (tutorialStage == 3) // mushroom
        {
            HUD.SetTutorial("some mushrooms move platforms");
            if (delay <= 0) {
                tutorialStage++;
            } else {
                delay -= Time.deltaTime;
            }
        }
        if (tutorialStage == 4) // dirt patch seed
        {
            HUD.SetTutorial("record actions on dirt patches");
            // detect plant seed
            if (Input.GetKey("e")) { // joystick button 1 (circle) 
                tutorialStage++;
            } 
        }
        if (tutorialStage == 5)  // dirt patch wilt
        {
            HUD.SetTutorial("press Q to stop recording");
            if (Input.GetKey("q")) { // joystick button 1 (circle)
                delay = 5.0f;
                tutorialStage++;
            }
        }
        if (tutorialStage == 5)  // dirt patch replant
        {
            HUD.SetTutorial("approach seeds to replant them");
            if (delay <= 0) {
                tutorialStage++;
            } else {
                delay -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !initialPromptComplete)
        {
            HUD = other.gameObject.GetComponent<HeadsUpDisplay>(); 
            HUD.SetTutorial("use left joystick to move");
            initialPromptComplete = true;
            tutorialStage = 0;
        }
    }

}