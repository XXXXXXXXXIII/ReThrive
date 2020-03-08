using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    private int currwaterCount = 0;
    private int currSeedCount = 0;
    public int maxWaterCount = 3;
    public int maxSeedCount = 5;
    public float defaultGhostDuration = 10f;

    public string PromptText = "Press E to plant Seed";
    public string DirtText = "Space Remaining: ";

    public List<Seed> seeds { get; set; }

    private List<Vector3> seedCoords;
    private bool isTracking = false;
    private float _ghostDuration;

    PlayerState PS;
    GhostManager GM;
    HeadsUpDisplay HUD;
    TextMesh Text;

    private void Start()
    {
        Text = GetComponentInChildren<TextMesh>();
        Text?.gameObject.SetActive(false);

        seeds = new List<Seed>();
        seedCoords = new List<Vector3>();
        Vector3 direction = new Vector3(0,0,0);
        float angle = 360f / maxSeedCount;
        for (int i = 0; i < maxSeedCount; i++)
        {
            seedCoords.Add(Quaternion.Euler(direction) * Vector3.right * 1f);
            direction.y += angle;
        }

        _ghostDuration = defaultGhostDuration;
    }

    private void FixedUpdate()
    {
        if (isTracking)
        {
            Text.text = DirtText + (maxSeedCount - seeds.Count);
            Text.gameObject.transform.rotation = Quaternion.LookRotation(transform.position - PS.transform.position, Vector3.up);
            if (seeds.Count == maxSeedCount)
            {
                Text.color = Color.red;
            }
            else
            {
                Text.color = Color.green;
            }
        }
    }

    public Vector3 GetNextSeedCoord()
    {
        return seedCoords[seeds.Count];
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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Dirt::Player stepped on dirt");
            PS = collider.gameObject.GetComponent<PlayerState>();
            GM = collider.gameObject.GetComponent<GhostManager>();
            HUD = collider.gameObject.GetComponent<HeadsUpDisplay>();
            PS.onDirt = true;
            PS.onInteractEnd += PlantSeed;
            PS.onInteractHold += ResetDirt;
            if(!GM.isRecording)
            {
                HUD.PushPrompt(PromptText);
                PS.currDirt = this;
                GM.duration = _ghostDuration;
            }
            isTracking = true;
            Text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Dirt::Player left dirt");
            HUD.PopPromptOnMatch(PromptText);
            PS.onDirt = false;
            GM.duration = GM.minDuration;
            PS.onInteractEnd -= PlantSeed;
            PS.onInteractHold -= ResetDirt;
            //PS.currDirt = null;
            isTracking = false;
            Text.gameObject.SetActive(false);
        }
    }

    private void ResetDirt()
    {
        foreach (Seed s in seeds)
        {
            s.ghost.Kill();
            Destroy(s.ghost.gameObject, .6f);
            Destroy(s.gameObject, .6f);
        }
        seeds.Clear();
        HUD.SetWarning("Dirt Cleared!");
    }
}
