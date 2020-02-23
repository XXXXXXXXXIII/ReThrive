using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    public int currwaterCount = 0;
    public int currSeedCount = 0;
    public int maxWaterCount = 3;
    public int maxSeedCount = 5;

    public List<Seed> seeds { get; set; }

    PlayerState PS;
    GhostManager GM;

    private float _ghostDuration = 10f;

    private void Start()
    {
        seeds = new List<Seed>();
    }


    public void PlantSeed()
    {
        if (seeds.Count == maxSeedCount)
        {
        }
        else
        {
            currSeedCount = seeds.Count;
        }
    }

    public void WaterSoil() //TODO: Decide if we want this
    {
        if (currwaterCount == maxWaterCount)
        {
        }
        else
        {
            currwaterCount++;
            _ghostDuration += 5f;
        }
    }

    public bool CanPlant()
    {
        return seeds.Count < maxSeedCount;
    }

    public bool CanWater()
    {
        return currwaterCount < maxWaterCount;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Dirt::Player stepped on dirt");
            PS = collider.gameObject.GetComponent<PlayerState>();
            GM = collider.gameObject.GetComponent<GhostManager>();
            PS.onDirt = true;
            if (seeds.Count < maxSeedCount)
            {
                GM.duration = _ghostDuration;
                PS.onPlant += PlantSeed;
                PS.currDirt = this;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Dirt::Player left dirt");
            PS.onDirt = false;
            GM.duration = GM.minDuration;
            PS.onPlant -= PlantSeed;
            //PS.currDirt = null;
        }
    }
}
