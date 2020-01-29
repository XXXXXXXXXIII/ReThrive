using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    private int health;
    private bool isActive;

    public float initX, initY, initZ;

    public UnityAction onDie;
    public UnityAction onSpawn;
    GhostManager GM;
    PlayerControl PC;
    Rigidbody player;

    public GameObject ghost;

    // Start is called before the first frame update
    void Start()
    {
        GM = GetComponent<GhostManager>();
        PC = GetComponent<PlayerControl>();
        player = GetComponent<Rigidbody>();
        onDie += OnDie;
        onSpawn += OnSpawn;
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
        if (collision.collider.tag == "Death")
        {
            onDie.Invoke();
        }
    }

    void OnDie()
    {
        Debug.Log("Player died");
        GM.StopRecording();
        onSpawn.Invoke();
    }

    void OnSpawn()
    {
        Debug.Log("Player spawned");
        player.transform.position = new Vector3(initX, initY, initZ);
        GM.StartRecording();
        if (GM.allGhost.Count > 0)
        {
            foreach (List<Vector3> list in GM.allGhost)
            {
                Debug.Log("Spawning Ghost");
                GameObject newGhost = Instantiate(ghost, new Vector3(-0.73f, 39.84f, 93.49f), Quaternion.identity);
                newGhost.GetComponent<GhostController>().SetRoute(list);
            }
        }
    }
}
