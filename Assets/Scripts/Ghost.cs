using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This replaces GhostController
public class Ghost : MonoBehaviour
{
    public float duration = 15f; // Duration of ghost, after which it will reset and loop
    public float health;
    public bool isLoop = true;
    public bool isInteracting = true;

    GameObject ghost;

    public Vector3 SeedCoord { get; set; }
    public List<Vector3> GhostPath { get; set; }
    public List<int> AnimationState { get; set; }
    public List<bool> InteractionState { get; set; }

    private bool isActive;
    private int index;

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
        GhostPath = new List<Vector3>();
        AnimationState = new List<int>();
        InteractionState = new List<bool>();
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            if (index >= GhostPath.Count)
            {
                index = 0;
                if (!isLoop)
                {
                    ghost.SetActive(false); 
                    isActive = false;
                }
            }
            ghost.transform.position = GhostPath[index];
            this.isInteracting = InteractionState[index];
            index++;
        }
    }

    public void Reset()
    {
        isActive = false;
        index = 0;
        isInteracting = false;
        ghost.SetActive(false);
    }

    public void Animate()
    {
        isActive = true;
        ghost.SetActive(true);
    }

    public void Halt()
    {
        isActive = false;
    }
}
