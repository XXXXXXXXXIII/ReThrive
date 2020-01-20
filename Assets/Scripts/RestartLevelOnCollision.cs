using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// import manager script

public class RestartLevelOnCollision : MonoBehaviour
{
    [SerializeField]
    string strTag;
    // public GameObject platform = GameObject.Find("Platform");
    // cameraMovementScript.Move(cameraPosition);
    GameObject managerObj;
    Manager Manager;


    void Start()
    {
        managerObj = GameObject.Find("Manager");
        Manager = managerObj.GetComponent<Manager>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        // restart on collision
        if (collision.collider.tag == strTag)
            Debug.Log(collision.gameObject.transform.position);

           // Manager.AddDeathCoord(collision.gameObject.transform.position);
           renderPlatform(collision.gameObject.transform.position);

            // GameObject platform = GameObject.Find("Platform");
            // Instantiate(platform, collision.gameObject.transform.position, Quaternion.identity);
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // move sphere to initial state
            GameObject platform = GameObject.Find("Sphere");
            platform.transform.position = new Vector3(-1.9f, 20f, 3.1f);


    }

    void renderPlatform(Vector3 platformPosition) {
        GameObject platform = GameObject.Find("Platform");
        Instantiate(platform, platformPosition, Quaternion.identity);
    }
}
