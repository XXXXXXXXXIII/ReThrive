using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    public GameObject bridge;

    public bool isTriggered = false;

    void OnTriggerEnter(Collider collider)
    {
        if (!isTriggered)
        {
            // Show bridge and enable collider.
            bridge.GetComponent<Renderer>().enabled = true;
            bridge.GetComponent<Collider>().enabled = true;
            isTriggered = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (isTriggered)
        {
            // Hide bridge and disable collider.
            bridge.GetComponent<Renderer>().enabled = false;
            bridge.GetComponent<Collider>().enabled = false;
            isTriggered = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
