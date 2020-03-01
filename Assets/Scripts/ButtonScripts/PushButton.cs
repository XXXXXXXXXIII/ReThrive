using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    public string PromptText = "Press F to Interact";

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
            PS = collider.gameObject.GetComponent<PlayerState>();
            HUD = collider.gameObject.GetComponent<HeadsUpDisplay>();
            HUD.PushPrompt(PromptText);
            PS.onInteractStart += OnButtonPress.Invoke;
            PS.onInteractEnd += OnButtonRelease.Invoke;
        }
        else if (collider.CompareTag("Ghost"))
        {
            ghost = collider.gameObject.GetComponent<Ghost>();
            ghost.onInteractStart += OnButtonPress.Invoke;
            ghost.onInteractEnd += OnButtonRelease.Invoke;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            HUD.PopPromptOnMatch(PromptText);
            PS.onInteractStart -= OnButtonPress.Invoke;
            PS.onInteractEnd -= OnButtonRelease.Invoke;
            OnButtonRelease?.Invoke();
        }
        else if (collider.CompareTag("Ghost"))
        {
            ghost = collider.gameObject.GetComponent<Ghost>();
            ghost.onInteractStart -= OnButtonPress.Invoke;
            ghost.onInteractEnd -= OnButtonRelease.Invoke;
            OnButtonRelease?.Invoke();
        }
    }
}
