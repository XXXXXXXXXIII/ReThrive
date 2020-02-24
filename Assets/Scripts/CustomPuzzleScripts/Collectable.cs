using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    GameObject collectable;
    public bool isActive;
    public CollectorPuzzle CP { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        collectable = gameObject;
        isActive = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") || collider.CompareTag("Ghost"))
        {
            collectable.SetActive(false);
            this.isActive = false;
            CP.Collect();
            Debug.Log("Collectable::Collectable collected!");
        }
    }

    public void EnableCollectable()
    {
        this.isActive = true;
        collectable.SetActive(true);
    }

    public void DisableCollectable()
    {
        this.isActive = false;
        collectable.SetActive(false);
    }
}
