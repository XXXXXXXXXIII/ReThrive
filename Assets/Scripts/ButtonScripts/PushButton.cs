using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    public string PromptText = "Press E to Interact";

    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonRelease;

    PlayerState PS;
    HeadsUpDisplay HUD;

    Ghost ghost;
    
    private int triggerCount;    

    // Start is called before the first frame update
    void Start()
    {
        triggerCount = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Button::Player stepping on button");
            PS = collider.gameObject.GetComponent<PlayerState>();
            HUD = collider.gameObject.GetComponent<HeadsUpDisplay>();
            HUD.PushPrompt(PromptText);
            PS.onInteractStart += ButtonPress;
            PS.onInteractEnd += ButtonRelease;
        }
        else if (collider.CompareTag("Ghost"))
        {
            ghost = collider.gameObject.GetComponent<Ghost>();
            ghost.onInteractStart += ButtonPress;
            ghost.onInteractEnd += ButtonRelease;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Button::Player left button");
            HUD.PopPromptOnMatch(PromptText);
            PS.onInteractStart -= ButtonPress;
            PS.onInteractEnd -= ButtonRelease;
            ButtonRelease();
        }
        else if (collider.CompareTag("Ghost"))
        {
            ghost = collider.gameObject.GetComponent<Ghost>();
            ghost.onInteractStart -= ButtonPress;
            ghost.onInteractEnd -= ButtonRelease;
            ButtonRelease();
        }
    }

    public void ButtonPress()
    {
        Debug.Log("Button::Player pressed button");
        if (++triggerCount == 1)
        {
            OnButtonPress.Invoke();
        }
    }

    public void ButtonRelease()
    {
        Debug.Log("Button::Player unpressed button");
        if (--triggerCount <= 0)
        {
            OnButtonRelease.Invoke();
            triggerCount = 0;
        }
    }
}
