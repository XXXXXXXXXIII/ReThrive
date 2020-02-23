using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    GameObject collectable;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        collectable = this.transform.gameObject;
        Debug.Log(collectable);
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            collectable.SetActive(false);
            this.isActive = false;
            Debug.Log("Collectable collected!");
        }
        if (collision.collider.tag == "Ghost")
        {
            collectable.SetActive(false);
            this.isActive = false;
            Debug.Log("Collectable collected!");
        }
    }

    public void EnableCollectable()
    {
        this.isActive = true;
        collectable.SetActive(true);
    }
}
