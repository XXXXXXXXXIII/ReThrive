using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Keeps track of player's state and status
public class PlayerState : MonoBehaviour
{
    public int health { get; set; }
    public int currAnimation { get; set; }
    public bool isActive { get; private set; }
    public bool isJumping { get; set; }
    public bool isInteracting { get; set; }
    public bool isRecording { get; set; }
    public bool onDirt { get; set; }
    public bool onSeed { get; set; }

    public Dirt currDirt { get; set; }
    public Seed currSeed { get; set; }
    private List<Seed> seeds;
    
    private Puzzle currPuzzle;

    // Initial spawn coordinates
    public Vector3 spawnCoord;
    public Vector3 spawnRot;
    List<Vector3> seedCoords;

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
    public GameObject seedPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GhostManager>();
        PC = GetComponent<PlayerControl>();
        player = GetComponent<Rigidbody>();

        //GameObject puzzleObject = GameObject.Find("Puzzle");
        //currPuzzle = puzzleObject.GetComponent<Puzzle>();

        onDie += OnDie;

        onWilt += OnWilt;

        onSpawn += OnSpawn;

        onPlant += PlantSeed;

        onInteract += OnInteract;

        seeds = new List<Seed>();
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
        if (collision.collider.CompareTag("Platform"))
        {
            player.velocity = Vector3.zero;
            player.angularVelocity = Vector3.zero;
            isJumping = false;
        }
        if (collision.collider.CompareTag("Death"))
        {
            onDie.Invoke();
        }
    }

    private void OnWilt()
    {
        Debug.Log("PS::Player wilted");
        currSeed.ghost = GM.StopRecording();
        foreach (Seed s in seeds)
        {
            s.ghost?.Reset();
        }
        onSpawn.Invoke();
    }

    private void OnDie()
    {
        Debug.Log("PS::Player died");
        if (GM.isRecording)
        {
            currSeed.ghost = GM.StopRecording();
        }
        foreach (Seed s in seeds)
        {
            s.ghost?.Reset();
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

    private void PlantSeed()
    {
        if (onSeed)
        {
            GM.StartRecording();
        }
        else if (onDirt)
        {
            if (GM.isRecording)
            {
                Debug.Log("PS::Cannot plant: Currently Recording Ghost!");
            }
            else if (currDirt.CanPlant())
            {
                Debug.Log("PS::I can plant");
                GameObject newSeed = Instantiate(seedPrefab, player.position, Quaternion.identity);
                newSeed.transform.SetParent(currDirt.transform);
                Seed seed = newSeed.GetComponent<Seed>();
                seeds.Add(seed);
                currDirt.seeds.Add(seed);
                currSeed = seed;
                //SetSpawn(player.position[0], player.position[1], player.position[2]);
                //seedCoords.Add(player.position);
                GM.StartRecording();
            }
            else
            {
                Debug.Log("PS::Cannot plant: Max seed reached!");
            }
        }
        else Debug.Log("PS::Cannot plant: Not on Dirt!");
    }

    private void OnSpawn()
    {
        Debug.Log("PS::Player spawned");
        player.transform.position = spawnCoord;
        player.transform.rotation = Quaternion.Euler(spawnRot);
        foreach (Seed s in seeds)
        {
            s.ghost?.Animate();
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

    private void OnInteract()
    {
        Debug.Log("PS::Interact Triggered!");
    }
}
