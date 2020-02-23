using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 spawnRot;
    public float offsetY = 0.1f;

    private Vector3 spawnCoord;
    private PlayerState PS;

    private void Start()
    {
        spawnCoord = transform.position;
        spawnCoord.y += offsetY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PS = other.gameObject.GetComponent<PlayerState>();
            if (!PS.isRecording)
            {
                Debug.Log("CheckPoint::New Spawn Coord: " + this.spawnCoord.ToString());
                PS.spawnCoord = spawnCoord;
                PS.spawnRot = spawnRot;
            }
        }
    }
}
