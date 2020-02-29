using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunlight : MonoBehaviour
{
    PlayerState PS;

    private void Start()
    {
        PS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(transform.position, -(transform.position - (PS.transform.position + Vector3.up * 0.2f)));
        if (Physics.Raycast(transform.position, -(transform.position - (PS.transform.position + Vector3.up * 0.2f)), out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                //Debug.Log("SUN::Player under sunlight");
                PS.inSun = true;
            }
            else
            {
                PS.inSun = false;
            }
        }
        else
        {
            //Debug.Log("SUN::Player left sunlight");
            PS.inSun = false;
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PS = other.GetComponent<PlayerState>();
            PS.inSun = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PS.inSun = false;
        }
    }
    */
}
