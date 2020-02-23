﻿using System.Collections;
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
    public bool isRecording;
    
    Rigidbody player;
    PlayerState PS;

    private Vector3 prevCoord, startCoord;
    private 

    // Start is called before the first frame update
    void Start()
    {
        currPath = new List<Vector3>();
        currInteractions = new List<bool>();
        currAnimations = new List<int>();
        player = GetComponent<Rigidbody>();
        PS = GetComponent<PlayerState>();
        isRecording = false;
        duration = minDuration;
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            currPath.Add(player.position - prevCoord);
            prevCoord = player.position;
            currInteractions.Add(PS.isInteracting);
            currAnimations.Add(PS.currAnimation);
        }
    }

    public void StartRecording()
    {
        Debug.Log("Started Recording Ghost\n");
        isRecording = true;
        currPath = new List<Vector3>();
        currInteractions = new List<bool>();
        currAnimations = new List<int>();
        prevCoord = player.position;
        startCoord = player.position;
    }

    public void CancelRecording()
    {
        isRecording = false;
    }

    // Stops recording and returns ghost
    public Ghost StopRecording()
    {
        Debug.Log("Stopped Recording Ghost\n");
        isRecording = false;

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
