using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLighting : MonoBehaviour
{
    Color LightingColor;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Load Lighting
        }
    }
}
