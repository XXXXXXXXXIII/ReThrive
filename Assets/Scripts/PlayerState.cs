using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Keeps track of player's state and status
public class PlayerState : MonoBehaviour
{
    public int health { get; set; }
    public int currAnimation { get; set; }
    public bool isActive { get; set; }
    public bool isJumping { get; set; }

    public bool onDirt { get; set; }
    public Dirt currDirt { get; set; }

    public bool isInteracting { get; set; }

    private List<Ghost> ghosts;
    private Puzzle currPuzzle;

    // Initial spawn coordinates
    public float spawnX, spawnY, spawnZ;
    public List<Vector3> seedCoords;

    // Append functions to these actions
    // NOTE: Directly invoke these for the approperiate action
    public UnityAction onDie;
    public UnityAction onSpawn;
    public UnityAction onInteract;
    public UnityAction onPlant;
    public UnityAction onWilt;

    GhostManager GM;
    PlayerControl PC;
    Rigidbody player;

    // Prefabs for ghost and seed
    public GameObject seed;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GhostManager>();
        PC = GetComponent<PlayerControl>();
        player = GetComponent<Rigidbody>();

        //GameObject puzzleObject = GameObject.Find("Puzzle");
        //currPuzzle = puzzleObject.GetComponent<Puzzle>();

        onDie += onDie;

        onWilt += OnWilt;

        onSpawn += OnSpawn;

        onPlant += PlantSeed;

        ghosts = new List<Ghost>();
        seedCoords = new List<Vector3>();
        isJumping = false;
        onDirt = false;
        currDirt = null;
        isActive = false;
        isInteracting = false;
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

    // When player collides with object
    // Only put common collisions such as "Death" or "Dirt" here, don't add puzzle specific detectors.
    // TODO: Remove this function
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Platform")
        {
            player.velocity = Vector3.zero;
            player.angularVelocity = Vector3.zero;
            isJumping = false;
        }
        if (collision.collider.tag == "Death")
        {
            onDie.Invoke();
        }
    }

    private void OnWilt()
    {
        Debug.Log("PlayerState::Player wilted");
        Ghost newGhost = GM.StopRecording();
        ghosts.Add(newGhost);
        currDirt.ghosts.Add(newGhost);
        foreach (Ghost g in ghosts)
        {
            g.Reset();
        }
        onSpawn.Invoke();
    }

    private void OnDie()
    {
        Debug.Log("PlayerState::Player died");
        GM.CancelRecording();
        foreach (Ghost g in ghosts)
        {
            g.Reset();
        }
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

    private void PlantSeed()
    {
        if (onDirt)
        {
            if (GM.isRecording)
            {
                Debug.Log("PlayerState::Cannot plant: Currently Recording Ghost!");
            }
            else if (currDirt.CanPlant())
            {
                Debug.Log("I can plant");
                GameObject newSeed = Instantiate(seed, player.position, Quaternion.identity);
                newSeed.transform.SetParent(currDirt.transform);
                //SetSpawn(player.position[0], player.position[1], player.position[2]);
                //seedCoords.Add(player.position);
                GM.StartRecording();
            }
            else
            {
                Debug.Log("PlayerState::Cannot plant: Max seed reached!");
            }
        }
        else Debug.Log("PlayerState::Cannot plant: Not on Dirt!");
    }

    private void OnSpawn()
    {
        Debug.Log("PlayerState::Player spawned");
        player.transform.position = new Vector3(spawnX, spawnY, spawnZ);
        foreach (Ghost g in ghosts)
        {
            g.Animate();
        }

        
        //currPuzzle.StartPuzzle();
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
