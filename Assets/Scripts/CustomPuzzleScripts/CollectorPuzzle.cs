using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines a collector puzzle
public class CollectorPuzzle : MonoBehaviour
{
    // Any child object is a collectable
    List<Collectable> collectables;

    private float startTime;
    private bool _isRunning;
    public bool isSolved { get; private set; }
    private int _totalObjects;
    private int _collected;

    // Max allowed time
    public float allottedTime = 30f;
    // Start is called before the first frame update
    void Start()
    {
        collectables = new List<Collectable>(GetComponentsInChildren<Collectable>());
        foreach (Collectable c in collectables)
        {
            c.CP = this;
            c.DisableCollectable();
        }
        startTime = -allottedTime;
        _isRunning = false;
        isSolved = false;
        _totalObjects = collectables.Count;
        _collected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRunning)
        {
            return;
        }

        if (startTime + allottedTime < Time.time)
        {
            StopPuzzle();
            Debug.Log("CollectorPuzzle::Puzzle Failed");
        }
        else if(_collected == _totalObjects)
        {
            Debug.Log("CollectorPuzzle::Puzzle Complete!");
            StopPuzzle();
            isSolved = true;
        }
    }

    public void Collect()
    {
        _collected++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartPuzzle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetPuzzle();
        }
    }

    public void StartPuzzle()
    {
        Debug.Log("CollectorPuzzle::Puzzle Start!");
        Debug.Log("CollectorPuzzle::Total objects: " + _totalObjects);
        foreach (Collectable c in collectables)
        {
            c.EnableCollectable();
        }
        startTime = Time.time;
        _isRunning = true;
        _collected = 0;
    }

    public void ResetPuzzle()
    {
        Debug.Log("CollectorPuzzle::Puzzle Reset!");
        _isRunning = false;
        startTime = Time.time;
        _collected = 0;
        foreach (Collectable o in collectables)
        {
            o.DisableCollectable();
        }
    }

    public void StopPuzzle()
    {
        _isRunning = false;
    }

    public void DestroyPuzzle()
    {

    }
}
