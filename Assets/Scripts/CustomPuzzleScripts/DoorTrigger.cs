using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject door;

    public bool isTriggered = false;

    void OnTriggerEnter(Collider collider)
    {
        if (!isTriggered)
        {
            // Open door.
            isTriggered = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (isTriggered)
        {
            // Close door.
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
