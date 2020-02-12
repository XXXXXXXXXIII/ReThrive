using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines a collector puzzle
public class CollectorPuzzle : MonoBehaviour
{
    // Any child object is a collectable
    List<Collectable> collectables;

    private float startTime;
    private bool isRunning;
    private int totalObjects;

    // Max allowed time
    public float allottedTime = 30f;
    // Start is called before the first frame update
    void Start()
    {
        collectables = new List<Collectable>(GetComponentsInChildren<Collectable>());
        startTime = -allottedTime;
        isRunning = false;
        PuzzleReset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        if (startTime + allottedTime < Time.time)
        {
            isRunning = false;
            Debug.Log("Mission Failed");
        }
        else
        {
            totalObjects = collectables.Count;
            foreach (Collectable o in collectables)
            {
                if (o.isActive == false)
                {
                    totalObjects--;
                    if (totalObjects == 0)
                    {
                        Debug.Log("Mission Complete!");
                        isRunning = false;
                        return;
                    }
                }
            }
        }
    }

    public void PuzzleReset()
    {
        startTime = Time.time;
        isRunning = true;
        totalObjects = collectables.Count;
        Debug.Log("Mission Start!");
        Debug.Log("Total objects: " + totalObjects);
        foreach (Collectable o in collectables)
        {
            o.EnableCollectable();
        }
    }
}
