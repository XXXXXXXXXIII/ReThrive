using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulley : MonoBehaviour
{
    public GameObject otherPulley;
    public float netMass = 0f;
    public float cargoMass { private set; get; }
    public Vector3 upOffset;
    public Vector3 downOffset;

    private Pulley other;
    private Vector3 initCoord;

    void Start()
    {
        cargoMass = 0;
        other = otherPulley.GetComponent<Pulley>();
        initCoord = transform.position;
    }

    void Update()
    {
        if (cargoMass + netMass > other.netMass + other.cargoMass)
        {
            if (Vector3.Distance(transform.position, initCoord + downOffset) > 0.1f)
            {
                transform.Translate(downOffset * Time.deltaTime);
            }
        }
        else if (cargoMass + netMass < other.netMass + other.cargoMass)
        {
            if (Vector3.Distance(transform.position, initCoord + upOffset) > 0.1f)
            {
                transform.Translate(upOffset * Time.deltaTime);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, initCoord) > 0.1f)
            {
                transform.Translate((initCoord - transform.position) * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ghost"))
        {
            cargoMass++;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ghost"))
        {
            cargoMass--;
        }
    }
}
