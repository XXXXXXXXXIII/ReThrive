using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStart : MonoBehaviour
{
    public bool useTrigger = true;
    //public bool useSequence = false;
    public string tutorialText = "";
    HeadsUpDisplay HUD;
    public float delay = 5.0f; 
    public float prevPosition;
    public float tutorialStage;
    public bool initialPrompt = false;
    
    void Start() {
        tutorialStage = 0;
        // disable lookaround
        // disable jump or set jump force zero
        // disable clone, wilt
        // enable actions sequentially
    }
    void Update() {
        if (tutorialStage == 0) // movement
        {
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0) {
                // delay is how many seconds to wait after player completed the correct action
                if (delay <= -8) {
                    HUD.SetTutorial("you moved!");
                    tutorialStage++;
                } else {
                    delay -= Time.deltaTime;
                }
            } else {
                delay -= Time.deltaTime;
            }
        }
        if (tutorialStage == 1) // lookaround
        { 
            HUD.SetTutorial("use the right joystick to look around");
            if (delay <= -15) {
                tutorialStage++;
            } else {
                delay -= Time.deltaTime;
            }  
        }
        if (tutorialStage == 2) // jump
        {

            HUD.SetTutorial("jump using X"); // currently triange?
            if (Input.GetKey("joystick button 3")) { // "joystick button 1" for X button
                HUD.SetTutorial("you jumped!");
            }

        }
        // tutorial stage 3 and onward (button, dirt patch, etc) use Tutorial.cs on GameObject
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !initialPrompt)
        {
            HUD = other.gameObject.GetComponent<HeadsUpDisplay>(); 
            HUD.SetTutorial("use left joystick to move");
            initialPrompt = true;
        }
    }

}