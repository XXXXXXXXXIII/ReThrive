using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Controls ghost
 * attach to ghost object, 
 */ 
public class GhostController : MonoBehaviour
{
    Rigidbody ghost;

    private List<Vector3> coords;
    private bool isActive;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        ghost = GetComponent<Rigidbody>();
        isActive = false;
        index = 0;
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            if (index >= coords.Count)
                index = 0;
            ghost.transform.position = coords[index];
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            isActive = true;
    }

    public void SetRoute(List<Vector3> list)
    {
        coords = list;
    }
}
