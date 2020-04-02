using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSoundTrigger : MonoBehaviour
{
    public AK.Wwise.Event soundEvent;
    public int duration;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) soundEvent.Post(gameObject);
        Debug.Log("inside cave");
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player")) soundEvent.ExecuteAction(gameObject, AkActionOnEventType.AkActionOnEventType_Stop, duration, AkCurveInterpolation.AkCurveInterpolation_Linear);
        Debug.Log("outside cave");
    }
}
