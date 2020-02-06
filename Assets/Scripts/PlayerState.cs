using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    private int health;
    private bool isActive;
    private bool isJumping;
    private bool canPlant;

    public float spawnX, spawnY, spawnZ;
    public List<Vector3> seedCoords;

    public UnityAction onDie;
    public UnityAction onSpawn;
    GhostManager GM;
    PlayerControl PC;
    Rigidbody player;

    public GameObject ghost;
    public GameObject seed;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GhostManager>();
        PC = GetComponent<PlayerControl>();
        player = GetComponent<Rigidbody>();
        onDie += OnDie;
        onSpawn += OnSpawn;
        seedCoords = new List<Vector3>();
        isJumping = false;
        canPlant = false;
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            isActive = true;
            onSpawn.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player Collision Detected");
        if (collision.collider.tag == "Platform")
        {
            player.velocity = Vector3.zero;
            player.angularVelocity = Vector3.zero;
            isJumping = false;
        }
        if (collision.collider.tag == "Dirt")
        {
            canPlant = true;
        }
        if (collision.collider.tag == "Death")
        {
            onDie.Invoke();
        }
    }

    public void OnWilt()
    {
        Debug.Log("Player died");
        List<Vector3> newGhostCoords = GM.StopRecording();
        GameObject newGhost = Instantiate(ghost, new Vector3(-0.73f, 39.84f, 93.49f), Quaternion.identity);
        newGhost.GetComponent<GhostController>().SetRoute(newGhostCoords);
        onSpawn.Invoke();
    }

    public void OnDie()
    {
        Debug.Log("Player died");
        GM.CancelRecording();
        onSpawn.Invoke();
    }

    public bool GetState()
    {
        // Returns only the jumping state for now.
        return isJumping;
    }

    public void SetState(bool jumping)
    {
        isJumping = jumping;
    }

    public void SetSpawn(float newX, float newY, float newZ)
    {
        spawnX = newX;
        spawnY = newY;
        spawnZ = newZ;
    }

    public void PlantSeed()
    {
        if (canPlant)
        {
            Debug.Log("I can plant");
            GameObject newSeed = Instantiate(seed, player.position, Quaternion.identity);
            SetSpawn(player.position[0], player.position[1], player.position[2]);
            seedCoords.Add(player.position);
            GM.StartRecording();
        }
        else Debug.Log("CANNOT plant!");
    }

    public void OnSpawn()
    {
        Debug.Log("Player spawned");
        player.transform.position = new Vector3(spawnX, spawnY, spawnZ);
        // if (GM.allGhost.Count > 0)
        // {
        //     foreach (List<Vector3> list in GM.allGhost)
        //     {
        //         Debug.Log("Spawning Ghost");
        //         GameObject newGhost = Instantiate(ghost, new Vector3(-0.73f, 39.84f, 93.49f), Quaternion.identity);
        //         newGhost.GetComponent<GhostController>().SetRoute(list);
        //     }
        // }
    }
}
