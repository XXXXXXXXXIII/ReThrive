using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Ghost object
// This replaces GhostController
public class Ghost : MonoBehaviour
{
    public float duration = 15f; // Duration of ghost, after which it will reset and loop
    public bool isLoop = true;

    GameObject ghost;

    public Vector3 seedCoord { get; set; }
    public List<Vector3> ghostPath { get; set; }
    public List<int> animations { get; set; }
    public List<bool> interactions { get; set; }

    private bool isActive;
    private bool isInteracting;
    private int animationState;
    private int index;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Spawned new ghost");
        ghost = this.transform.gameObject;
        isActive = false;
        isInteracting = false;
        ghost.SetActive(false);
        animationState = 0;
        index = 0;

        seedCoord = new Vector3();
        ghostPath = new List<Vector3>();
        animations = new List<int>();
        interactions = new List<bool>();
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            if (index >= ghostPath.Count)
            {
                index = 0;
                if (!isLoop)
                {
                    ghost.SetActive(false); 
                    isActive = false;
                }
            }
            ghost.transform.position = ghostPath[index];
            this.isInteracting = interactions[index];
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
        ghost.SetActive(false);
    }
}
