using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This replaces GhostController
public class Ghost : MonoBehaviour
{
    public float duration { get; set; } // Duration of ghost in seconds, after which it will reset and loop
    public float health;
    public bool isLoop = true;
    public bool isInteracting = true;

    GameObject ghost;

    public Vector3 SeedCoord { get; set; }
    public Quaternion SeedRot { get; set; }
    public List<Vector3> GhostPath { get; set; }
    public List<Vector3> GhostRotation { get; set; }
    public List<int> AnimationState { get; set; }
    public List<bool> InteractionState { get; set; }

    private bool isActive;
    private int index;
    private int maxIndex;
    private Vector3 finalAction;
    private Vector3 finalRotation;

    // Awake is required here
    void Awake()
    {
        Debug.Log("Spawned new ghost");
        ghost = this.transform.gameObject;
        isActive = false;
        isInteracting = false;
        ghost.SetActive(false);
        index = 0;

        SeedCoord = new Vector3();
        SeedRot = new Quaternion();
        GhostPath = new List<Vector3>();
        GhostRotation = new List<Vector3>();
        AnimationState = new List<int>();
        InteractionState = new List<bool>();
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            if (index >= GhostPath.Count)
            {
                if (index < maxIndex)
                {
                    transform.Translate(finalAction, Space.World); // Repeat last step 
                    //transform.Rotate(finalRotation, Space.Self);
                    // this.isInteracting = false;
                    index++;
                }
                else
                {
                    Reset();
                    if (isLoop)
                    {
                        Animate();
                    }
                }
            }
            else
            {
                transform.Translate(GhostPath[index], Space.World);
                transform.Rotate(GhostRotation[index], Space.Self);
                this.isInteracting = InteractionState[index];
                index++;
            }
        }
    }

    public void Reset()
    {
        ghost.transform.position = this.SeedCoord;
        ghost.transform.rotation = this.SeedRot;
        isActive = false;
        index = 0;
        isInteracting = false;
        //ghost.SetActive(false); //TODO: Removing this cuz it disables collider as well
    }

    public void Animate()
    {
        isActive = true;
        ghost.SetActive(true);
        maxIndex = (int)(duration * (1 / Time.fixedDeltaTime));
        finalAction = GhostPath[GhostPath.Count - 1];
        finalAction.y = 0; // Prevent ghost from jumping up forever
        finalRotation = GhostRotation[GhostRotation.Count - 1];
    }

    public void Halt()
    {
        isActive = false;
    }
}
