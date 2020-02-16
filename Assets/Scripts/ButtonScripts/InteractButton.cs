using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractButton : MonoBehaviour
{
    public UnityEvent OnButtonPress;
    public UnityEvent OnButtonRelease;

    private int triggerCount;

    // Start is called before the first frame update
    void Start()
    {
        triggerCount = 0;
    }

    void OnTriggerEnter(Collider collider)
    {
        triggerCount++;
        if (collider.tag.Equals("Player") || collider.tag.Equals("Ghost"))
        {
            OnButtonPress?.Invoke();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        triggerCount--;
        if (triggerCount == 0)
        {
            if (collider.tag.Equals("Player") || collider.tag.Equals("Ghost"))
            {
                OnButtonRelease?.Invoke();
            }
        }
    }
}
