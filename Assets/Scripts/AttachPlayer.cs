using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer : MonoBehaviour
{
    void OnTriggerEnter (Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.transform.parent = null;
        }
    }
}
