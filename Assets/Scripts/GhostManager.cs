using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * Ghost Manager class
 * Attach this to player, it will record player's actions until StopRecording() is called
 */
public class GhostManager : MonoBehaviour
{
    public List<List<Vector3>> allGhost;

    private List<Vector3> currGhost;
    private bool isRecording;
    private Rigidbody player;

    // Start is called before the first frame update
    void Start()
    {
        currGhost = new List<Vector3>();
        allGhost = new List<List<Vector3>>();
        player = GetComponent<Rigidbody>();
        isRecording = false;
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            currGhost.Add(player.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRecording()
    {
        Debug.Log("Started Recording Ghost\n");
        isRecording = true;
        currGhost = new List<Vector3>();
    }

    public void CancelRecording()
    {
        isRecording = false;
        currGhost = new List<Vector3>();
    }

    // Stops recording and saves ghost
    public List<Vector3> StopRecording()
    {
        Debug.Log("Stopped Recording Ghost\n");
        isRecording = false;
        allGhost.Add(currGhost);
        return currGhost;
    }
}
