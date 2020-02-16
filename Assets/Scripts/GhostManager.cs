using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * Ghost Manager class
 * Attach this to player, it will record player's actions until StopRecording() is called
 */
public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab;

    private List<Vector3> currPath;
    private List<bool> currInteractions;
    private List<int> currAnimations;
    public bool isRecording;
    
    Rigidbody player;
    PlayerState PS;

    // Start is called before the first frame update
    void Start()
    {
        currPath = new List<Vector3>();
        currInteractions = new List<bool>();
        currAnimations = new List<int>();
        player = GetComponent<Rigidbody>();
        PS = GetComponent<PlayerState>();
        isRecording = false;
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            currPath.Add(player.position);
            currInteractions.Add(PS.isInteracting);
            currAnimations.Add(PS.currAnimation);
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
        currPath = new List<Vector3>();
        currInteractions = new List<bool>();
        currAnimations = new List<int>();
    }

    public void CancelRecording()
    {
        isRecording = false;
        currPath = new List<Vector3>();
        currInteractions = new List<bool>();
        currAnimations = new List<int>();
    }

    // Stops recording and returns ghost
    public Ghost StopRecording()
    {
        Debug.Log("Stopped Recording Ghost\n");
        isRecording = false;

        GameObject ghostObject = Instantiate(ghostPrefab, currPath[0], Quaternion.identity);
        Ghost ghost = ghostObject.GetComponent<Ghost>();
        ghost.SeedCoord = currPath[0];
        ghost.GhostPath = currPath;
        ghost.AnimationState = currAnimations;
        ghost.InteractionState = currInteractions;

        return ghost;
    }
}
