using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Player_Status
{ 
    idle = 0, 
    spawning, 
    active, 
    wilting, 
    dead
}

// Keeps track of player's state and status
public class PlayerState : MonoBehaviour
{
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
    public int pressCounter { get; set; }

    public bool spawnAtCurrCoord = false;

    public float plantCost = 0.2f;
    public float deathCost = 0.01f;
    public Dirt currDirt { get; set; }
    public Seed currSeed { get; set; }
    private List<Seed> seeds;
    
    // Initial spawn coordinates
    public Vector3 spawnCoord;
    public Vector3 spawnRot;

    // Append functions to these actions
    // NOTE: Directly invoke these for the approperiate action
    public UnityAction onDie;
    public UnityAction onSpawn;
    public UnityAction onInteractStart;
    public UnityAction onInteractHold;
    public UnityAction onInteractEnd;
    //public UnityAction onPlant;
    public UnityAction onWilt;

    GhostManager GM;
    public PlayerController PC { get; private set; }
    public HeadsUpDisplay HUD { get; private set; }
    public Animator AC { get; private set; }
    public CharacterController CC { get; private set; }

    // Prefabs for ghost and seed
    public GameObject seedPrefab;
    public GameObject ghostPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GhostManager>();
        PC = GetComponent<PlayerController>();
        AC = GetComponent<Animator>();
        HUD = GetComponent<HeadsUpDisplay>();
        CC = GetComponent<CharacterController>();

        if (spawnAtCurrCoord)
        {
            spawnCoord = transform.position;
        }

        onDie += OnDie;
        onWilt += OnWilt;
        onSpawn += OnSpawn;
        PC.onInteractStart += OnInteractStart;
        PC.onInteractEnd += OnInteractEnd;
        PC.onInteractHold += OnInteractHold;

        PC.onWilt += onWilt.Invoke;

        seeds = new List<Seed>();
        onDirt = false;
        currDirt = null;
        isInteracting = false;
        //spawnCoord = transform.position;
        sunMeter = 1f;
        waterMeter = 1f;
        wiltMeter = 1f;
        pressCounter = 0;
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
                if (AC.GetCurrentAnimatorStateInfo(0).IsName("Void"))
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

    public void OnInteractStart()
    {
        //Debug.Log("PS::Interaction Start"); 
        onInteractStart?.Invoke();
    }

    public void OnInteractHold()
    {
        onInteractHold?.Invoke();
    }

    public void OnInteractEnd()
    {
        //Debug.Log("PS::Interaction End");
        if (onSeed)
        {
            ReplantSeed();
        }
        else if (onDirt)
        {
            PlantSeed();
        }
        onInteractEnd?.Invoke();
    }

    private void ReplantSeed()
    {
        AC.SetBool("isMoving", false);
        ResetGhosts();
        AnimateGhosts();
        PC.SwitchToGhost(currSeed.ghost, currSeed.ghost.AC, currSeed.ghost.CC);
        GM.StartRecording(currSeed.ghost);
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
            AC.SetBool("isMoving", false);
            ResetGhosts();
            AnimateGhosts();
            //TODO: Let dirt decide where to plant the seed
            GameObject newSeed = Instantiate(seedPrefab, currDirt.transform.position + currDirt.GetNextSeedCoord(), transform.rotation);
            newSeed.transform.SetParent(currDirt.transform);
            Seed seed = newSeed.GetComponent<Seed>();
            currDirt.seeds.Add(seed);
            currSeed = seed;
            seeds.Add(seed);

            GameObject newGhost = Instantiate(ghostPrefab, newSeed.transform.position, newSeed.transform.rotation);
            newGhost.transform.parent = null;
            Ghost ghost = newGhost.GetComponent<Ghost>();
            seed.ghost = ghost;
            ghost.SeedCoord = seed.transform.position;
            ghost.SeedRot = seed.transform.rotation;

            PC.SwitchToGhost(ghost, ghost.AC, ghost.CC);
            GM.StartRecording(ghost);
        }
    }

    private void OnWilt()
    {
        HUD.SetWarning("You wilted!");
        AC.SetBool("isMoving", false);

        if (GM.isRecording)
        {
            GM.StopRecording();
            PC.SwitchToPlayer(this, AC, CC);
            ResetGhosts();
            AnimateGhosts();
        }
        else
        {
            Debug.Log("PS::Player wilted");
            PC.FreezePlayer();
            sunMeter -= deathCost;
            waterMeter -= deathCost;
            AC.SetTrigger("OnWilt");
            status = Player_Status.wilting;
        }

    }

    private void OnDie()
    {
        HUD.SetWarning("You died!");
        Debug.Log("PS::Player died");
        AC.SetBool("isMoving", false);
        PC.FreezePlayer();
        if (GM.isRecording)
        {
            GM.StopRecording();
            PC.SwitchToPlayer(this, AC, CC);
            ResetGhosts();
            AnimateGhosts();
        }
        else
        {
            sunMeter -= deathCost;
            waterMeter -= deathCost;
            AC.SetTrigger("OnWilt");
            status = Player_Status.dead;
        }

    }

    private void OnSpawn()
    {
        Debug.Log("PS::Player spawned at " + spawnCoord);
        AC.SetBool("isMoving", false);
        PC.FreezePlayer();
        transform.position = spawnCoord;
        transform.rotation = Quaternion.Euler(spawnRot);
        ResetGhosts();
        AnimateGhosts();
        AC.SetTrigger("OnSpawn");
        status = Player_Status.spawning;
    }

    public void ResetGhosts()
    {
        seeds.RemoveAll(s => s == null);
        foreach (Seed s in seeds)
        {
            s.ghost?.ResetGhost();
        }
    }

    public void AnimateGhosts()
    {
        seeds.RemoveAll(s => s == null);
        foreach (Seed s in seeds)
        {
            s.ghost?.Animate();
        }
    }
}
