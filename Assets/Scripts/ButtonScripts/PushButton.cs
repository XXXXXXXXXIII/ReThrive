﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonRelease;

    private int triggerCount;

    // Start is called before the first frame update
    void Start()
    {
        triggerCount = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ghost"))
        {
            PlayerState PS = collider.gameObject.GetComponent<PlayerState>();
            PS.onInteract += OnButtonPress.Invoke;
            PS.onInteractRelease += OnButtonRelease.Invoke;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ghost"))
        {
            PlayerState PS = collider.gameObject.GetComponent<PlayerState>();
            PS.onInteract -= OnButtonPress.Invoke;
            PS.onInteractRelease -= OnButtonRelease.Invoke;
        }
    }
}
