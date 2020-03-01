using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Keeps track of player's state and status
public class PlayerState : MonoBehaviour
{
    public int health { get; set; }
    public float sunMeter { get; set; }
    public float waterMeter { get; set; }
    public float wiltMeter { get; set; }
    public int currAnimation { get; set; }
    public bool isActive { get; private set; }
    public bool isJumping { get; set; }
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
    public UnityAction onInteract;
    public UnityAction onInteractRelease;
    public UnityAction onPlant;
    public UnityAction onWilt;

    GhostManager GM;
    PlayerControl PC;
    HeadsUpDisplay HUD;
    CharacterController CC;
    Animator AC;

    // Prefabs for ghost and seed
    public GameObject seedPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GhostManager>();
        PC = GetComponent<PlayerControl>();
        CC = GetComponent<CharacterController>();
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

        onPlant += PlantSeed;

        onInteract += OnInteract;

        seeds = new List<Seed>();
        seedCoords = new List<Vector3>();
        isJumping = false;
        onDirt = false;
        currDirt = null;
        isActive = false;
        isInteracting = false;
        //spawnCoord = transform.position;
        sunMeter = 1f;
        waterMeter = 1f;
        wiltMeter = 1f;
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

    void FixedUpdate()
    {
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

    private void OnWilt()
    {
        HUD.SetWarning("You wilted!");
        Debug.Log("PS::Player wilted");
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
        onSpawn.Invoke();
    }

    private void OnDie()
    {
        HUD.SetWarning("You died!");
        Debug.Log("PS::Player died");
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
            foreach (Seed s in seeds)
            {
                s.ghost?.Reset();
                s.ghost?.Animate();
            }
            GM.StartRecording();
        }
        else if (onDirt)
        {
            if (GM.isRecording)
            {
                Debug.Log("PS::Cannot plant: Currently Recording Ghost!");
                HUD.SetWarning("You are recording a clone!");
            }
            else if (currDirt.CanPlant())
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
            else
            {
                HUD.SetWarning("You can't spawn any more clones on this dirt patch!");
                Debug.Log("PS::Cannot plant: Max seed reached!");
            }
        }
        else
        {
            HUD.SetWarning("You are not on a dirt patch!");
            Debug.Log("PS::Cannot plant: Not on Dirt!");
        }
    }

    private void OnSpawn()
    {
        Debug.Log("PS::Player spawned at " + spawnCoord);
        transform.position = spawnCoord;
        transform.rotation = Quaternion.Euler(spawnRot);
        foreach (Seed s in seeds)
        {
            s.ghost?.Animate();
        }
        AC.SetTrigger("OnSpawn");


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
