using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


/* 
 * Ghost Manager class
 * Attach this to player, it will record player's actions until StopRecording() is called
 */
public class GhostManager : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float minDuration = 10f;

    public float duration { get; set; }
    private List<Vector3> currPath; // NOTE: Stores offset from prevCoord
    private List<bool> currInteractions;
    private List<int> currAnimations;
    public bool isRecording { get; set; }
    
    PlayerState PS;
    HeadsUpDisplay HUD;

    private Vector3 prevCoord, startCoord;
    private float _startTime;

    // Start is called before the first frame update
    void Start()
    {
        currPath = new List<Vector3>();
        currInteractions = new List<bool>();
        currAnimations = new List<int>();
        PS = GetComponent<PlayerState>();
        HUD = GetComponent<HeadsUpDisplay>();
        isRecording = false;
        duration = minDuration;
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            if (Time.time - _startTime >= duration)
            {
                PS.onWilt.Invoke();
            }
            else
            {
                //Debug.Log("GM::Time Remaining: " + (10f - (Time.time - _startTime)));
                currPath.Add(transform.position - prevCoord);
                prevCoord = transform.position;
                currInteractions.Add(PS.isInteracting);
                currAnimations.Add(PS.currAnimation);
            }
        }
    }

    public void StartRecording()
    {
        Debug.Log("GM::Started Recording Ghost\n");
        HUD.SetText("InteractionPrompt", "Press X/E to wilt");
        isRecording = true;
        PS.isRecording = true;
        currPath = new List<Vector3>();
        currInteractions = new List<bool>();
        currAnimations = new List<int>();
        prevCoord = transform.position;
        startCoord = transform.position;
        _startTime = Time.time;
    }

    public void PauseRecording()
    {
        isRecording = false;
    }

    // Stops recording and returns ghost
    public Ghost StopRecording()
    {
        Debug.Log("GM::Stopped Recording Ghost\n");
        HUD.SetText("InteractionPrompt", "");
        isRecording = false;
        PS.isRecording = false;

        GameObject ghostObject = Instantiate(ghostPrefab, startCoord, Quaternion.identity);
        Ghost ghost = ghostObject.GetComponent<Ghost>();
        ghost.SeedCoord = startCoord;
        ghost.GhostPath = currPath;
        ghost.AnimationState = currAnimations;
        ghost.InteractionState = currInteractions;
        ghost.duration = this.duration;

        return ghost;
    }
}
