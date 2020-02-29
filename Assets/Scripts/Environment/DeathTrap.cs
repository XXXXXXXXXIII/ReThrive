using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    PlayerState PS;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("DT::Player entered deathtrap, player died");
            PS = other.gameObject.GetComponent<PlayerState>();
            PS.onDie.Invoke();
        }
    }
}
