using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPlayer : MonoBehaviour
{
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Ghost"))
        {
            Debug.Log("AP::Attached player");
            collision.collider.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Ghost"))
        {
            Debug.Log("AP::Disattached player");
            collision.collider.gameObject.transform.parent = null;
        }
    }
    */

    void OnTriggerEnter (Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ghost"))
        {
            Debug.Log("AP::Attached player");
            collider.gameObject.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ghost"))
        {
            Debug.Log("AP::Disattached player");
            collider.gameObject.transform.parent = null;
        }
    }
}
