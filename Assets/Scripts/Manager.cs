using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Vector3 respawnCoord = new Vector3();
    public List<Vector3> deathCoords = new List<Vector3>(); // to be populated as player dies;
    
    // Start is called before the first frame update
    void Start()
    {
        // render player function?
        // call outside respawn function
        // respawn(respawnCoords)

        // rerender platforms
        // renderPlatforms(deathCoords);

        //for (var i = 0; i < deathCoords.count(); i++) {
         //   renderPlatform(deathCoords[i]);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void ChangeRespawnPoint(Vector3 respawnCoord) {

    // }

    void AddDeathCoord(Vector3 deathCoord) {
        deathCoords.Add(deathCoord);
    }

    // void ResetDeathCoodes() {
    //     deathCoords.Clear();
    // }

    public void renderPlatform(Vector3 platformPosition) {
        GameObject platform = GameObject.Find("Platform");
        Instantiate(platform, platformPosition, Quaternion.identity); 
    }
}
