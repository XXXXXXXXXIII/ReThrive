using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines a collector puzzle
public class CollectorPuzzle : MonoBehaviour, Puzzle
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
            StopPuzzle();
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
                        StopPuzzle();
                        return;
                    }
                }
            }
        }
    }

    public void InitPuzzle()
    {

    }

    public void StartPuzzle()
    {
        Debug.Log("Mission Start!");
        Debug.Log("Total objects: " + totalObjects);
        startTime = Time.time;
        totalObjects = collectables.Count;
        isRunning = true;
    }

    public void ResetPuzzle()
    {
        Debug.Log("Puzzle Reset!");
        isRunning = false;
        startTime = Time.time;
        totalObjects = collectables.Count;
        foreach (Collectable o in collectables)
        {
            o.EnableCollectable();
        }
    }

    public void StopPuzzle()
    {
        isRunning = false;
    }

    public void DestroyPuzzle()
    {

    }
}
