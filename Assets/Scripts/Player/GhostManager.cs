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
    public float minDuration = 10f;
    public float duration { get; set; }
    public bool isRecording { get; set; }
    
    PlayerState PS;
    PlayerController PC;
    HeadsUpDisplay HUD;
    Ghost ghost;

    private Vector3 prevCoord;
    private Quaternion prevRot;
    public float startTime { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<PlayerState>();
        PC = GetComponent<PlayerController>();
        HUD = GetComponent<HeadsUpDisplay>();
        isRecording = false;
        duration = minDuration;
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            if (ghost.status != Player_Status.active)
            {
                PC.FreezePlayer();
            }
            else
            {
                PC.UnfreezePlayer();
                if (Time.time - startTime >= duration)
                {
                    ghost.onWilt?.Invoke();
                }
                else
                {
                    //Debug.Log("GM::Time Remaining: " + (10f - (Time.time - startTime)));
                    //Debug.Log(ghost.transform.position);
                    ghost.GhostPath.Add(ghost.transform.position - prevCoord);
                    ghost.GhostRotation.Add(ghost.transform.rotation.eulerAngles - prevRot.eulerAngles);
                    prevCoord = ghost.transform.position;
                    prevRot = ghost.transform.rotation;
                    ghost.InteractionState.Add(PC.isInteracting);
                    //ghost.AnimationState.Add(ghost.currAnimation);
                }
            }
        }
    }

    public void StartRecording(Ghost g)
    {
        Debug.Log("GM::Started Recording Ghost\n");
        HUD.ClearPrompt();
        HUD.PushPrompt("Press Q to wilt");

        this.ghost = g;
        isRecording = true;
        ghost.isControlling = true;
        ghost.GhostPath = new List<Vector3>();
        ghost.GhostRotation = new List<Vector3>();
        ghost.InteractionState = new List<bool>();
        ghost.AnimationState = new List<int>();

        prevCoord = ghost.transform.position;
        prevRot = ghost.transform.rotation;
        startTime = Time.time;
    }

    // Stops recording and returns ghost
    public void StopRecording()
    {
        Debug.Log("GM::Stopped Recording Ghost\n");
        HUD.ClearPrompt();
        isRecording = false;
        ghost.isControlling = false;
        ghost.duration = this.duration;
        ghost.Reset();
    }
}
