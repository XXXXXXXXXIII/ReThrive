using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Keeps track of player's state and status
public class PlayerState : MonoBehaviour
{
    public enum Player_Status
    {
        idle = 0,
        spawning,
        active, 
        wilting,
        dead
    }

    public Player_Status status = Player_Status.idle; 
    public int health { get; set; }
    public float sunMeter { get; set; }
    public float waterMeter { get; set; }
    public float wiltMeter { get; set; }
    public int currAnimation { get; set; }
    public bool isInteracting { get; set; }
    public bool onDirt { get; set; }
    public bool onSeed { get; set; }
    public bool inSun { get; set; }
    public bool inWater { get; set; }

    public bool spawnAtCurrCoord = false;

    public float plantCost = 0.2f;
    public float deathCost = 0.01f;
    public Dirt currDirt { get; set; }
    public Seed currSeed { get; set; }
    private List<Seed> seeds;
    
    // Initial spawn coordinates
    public Vector3 spawnCoord;
    public Vector3 spawnRot;
    List<Vector3> seedCoords;

    // Append functions to these actions
    // NOTE: Directly invoke these for the approperiate action
    public UnityAction onDie;
    public UnityAction onSpawn;
    public UnityAction onInteractStart;
    public UnityAction onInteractEnd;
    //public UnityAction onPlant;
    public UnityAction onWilt;

    GhostManager GM;
    PlayerController PC;
    HeadsUpDisplay HUD;
    Animator AC;

    // Prefabs for ghost and seed
    public GameObject seedPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GhostManager>();
        PC = GetComponent<PlayerController>();
        AC = GetComponent<Animator>();
        HUD = GetComponent<HeadsUpDisplay>();

        if (spawnAtCurrCoord)
        {
            spawnCoord = transform.position;
        }

        //GameObject puzzleObject = GameObject.Find("Puzzle");
        //currPuzzle = puzzleObject.GetComponent<Puzzle>();

        onDie += OnDie;

        onWilt += OnWilt;

        onSpawn += OnSpawn;

        onInteractStart += OnInteract;

        //onInteractStart += OnInteract;

        seeds = new List<Seed>();
        seedCoords = new List<Vector3>();
        onDirt = false;
        currDirt = null;
        isInteracting = false;
        //spawnCoord = transform.position;
        sunMeter = 1f;
        waterMeter = 1f;
        wiltMeter = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        switch (status)
        {
            case Player_Status.idle:
                PC.FreezePlayer();
                onSpawn.Invoke();
                break;
            case Player_Status.spawning:
                if (AC.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    status = Player_Status.active;
                    PC.UnfreezePlayer();
                }
                break;
            case Player_Status.wilting:
                if (AC.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    status = Player_Status.idle;
                }
                break;
            case Player_Status.dead:
                status = Player_Status.idle;
                break;
            case Player_Status.active:
                break;
        }


        if (!GM.isRecording)
        {
            if (inWater && waterMeter < 1f)
            {
                waterMeter += 0.01f;
            }
            if (inSun && sunMeter < 1f)
            {
                sunMeter += 0.01f;
            }
        }
    }
    private void OnInteract()
    {
        if (onSeed)
        {
            ReplantSeed();
        }
        else if (onDirt)
        {
            PlantSeed();
        }
    }

    private void ReplantSeed()
    {
        foreach (Seed s in seeds)
        {
            s.ghost?.Reset();
            s.ghost?.Animate();
        }
        GM.StartRecording();
    }

    private void PlantSeed()
    {
        if (GM.isRecording)
        {
            Debug.Log("PS::Cannot plant: Currently Recording Ghost!");
            HUD.SetWarning("You are recording a clone!");
        }
        else if (!currDirt.CanPlant())
        {
            HUD.SetWarning("You can't spawn any more clones on this dirt patch!");
            Debug.Log("PS::Cannot plant: Max seed reached!");
        }
        else if (sunMeter < plantCost)
        {
            HUD.SetWarning("Not enough sunlight!");
            Debug.Log("PS::Not enough sun!");
        }
        else if (waterMeter < plantCost)
        {
            HUD.SetWarning("Not enough water!");
            Debug.Log("PS::Not enough water!");
        }
        else
        {
            Debug.Log("PS::I can plant");
            waterMeter -= plantCost;
            sunMeter -= plantCost;
            foreach (Seed s in seeds)
            {
                s.ghost?.Reset();
                s.ghost?.Animate();
            }
            GameObject newSeed = Instantiate(seedPrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity);
            newSeed.transform.SetParent(currDirt.transform);
            Seed seed = newSeed.GetComponent<Seed>();
            seeds.Add(seed);
            currDirt.seeds.Add(seed);
            currSeed = seed;
            GM.StartRecording();
        }
    }

    private void OnWilt()
    {
        HUD.SetWarning("You wilted!");
        Debug.Log("PS::Player wilted");
        PC.FreezePlayer();
        if (GM.isRecording)
        {
            currSeed.ghost = GM.StopRecording();
        }
        else
        {
            sunMeter -= deathCost;
            waterMeter -= deathCost;
        }
        foreach (Seed s in seeds)
        {

            s.ghost?.Reset();
        }
        AC.SetTrigger("OnWilt");
        status = Player_Status.wilting;
    }

    private void OnDie()
    {
        HUD.SetWarning("You died!");
        Debug.Log("PS::Player died");
        PC.FreezePlayer();
        if (GM.isRecording)
        {
            currSeed.ghost = GM.StopRecording();
        }
        else
        {
            sunMeter -= deathCost;
            waterMeter -= deathCost;
        }
        foreach (Seed s in seeds)
        {
            s.ghost?.Reset();
        }
        AC.SetTrigger("OnWilt");
        status = Player_Status.dead;
    }

    private void OnSpawn()
    {
        Debug.Log("PS::Player spawned at " + spawnCoord);
        PC.FreezePlayer();
        transform.position = spawnCoord;
        transform.rotation = Quaternion.Euler(spawnRot);
        foreach (Seed s in seeds)
        {
            s.ghost?.Animate();
        }
        AC.SetTrigger("OnSpawn");
        status = Player_Status.spawning;
    }
}
