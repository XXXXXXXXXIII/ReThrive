using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public DoorTrigger button1;
    public DoorTrigger button2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (button1.isTriggered && button2.isTriggered) this.gameObject.transform.position = new Vector3(0.07f, 2f, -0.03f);
        else this.gameObject.transform.position = new Vector3(0.07f, 1.267f, -0.03f);
    }
}
