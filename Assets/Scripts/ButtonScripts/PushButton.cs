using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    public string PromptText = "Hold E to Interact";
    public Light ButtonLight;

    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonRelease;

    PlayerState PS;
    HeadsUpDisplay HUD;

    Ghost ghost;
    
    private int triggerCount;    

    // Start is called before the first frame update
    void Start()
    {
        HUD = GameObject.Find("Leafy_Player").GetComponent<HeadsUpDisplay>();
        triggerCount = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Button::Player stepping on button");
            PS = collider.gameObject.GetComponent<PlayerState>();
            HUD.PushPrompt(PromptText);
            PS.onInteractStart += ButtonPress;
            PS.onInteractEnd += ButtonRelease;
        }
        else if (collider.CompareTag("Ghost"))
        {
            ghost = collider.gameObject.GetComponent<Ghost>();
            if (ghost.isControlling)
            {
                HUD.PushPrompt(PromptText);
            }
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
            if (ghost.isControlling)
            {
                HUD.PopPromptOnMatch(PromptText);
            }
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
            if (ButtonLight)
            {
                ButtonLight.intensity *= 1.1f;
                ButtonLight.range *= 1.2f;
            }
        }
    }

    public void ButtonRelease()
    {
        Debug.Log("Button::Player unpressed button");
        if (--triggerCount <= 0)
        {
            OnButtonRelease.Invoke();
            triggerCount = 0;
            if (ButtonLight)
            {
                ButtonLight.intensity /= 1.1f;
                ButtonLight.range /= 1.2f;
            }
        }
    }
}
