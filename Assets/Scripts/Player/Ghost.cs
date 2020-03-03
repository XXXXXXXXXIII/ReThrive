using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This replaces GhostController
public class Ghost : MonoBehaviour
{
    public Player_Status status = Player_Status.idle; 
    public float duration { get; set; } // Duration of ghost in seconds, after which it will reset and loop
    public float health;
    public bool isLoop = true;
    public bool isInteracting = false;
    public bool isControlling = false;

    public UnityAction onInteractStart, onInteractEnd, onInteractHold, onWilt;

    public Animator AC { get; private set; }
    public CharacterController CC { get; private set; }

    public Vector3 SeedCoord { get; set; }
    public Quaternion SeedRot { get; set; }
    public List<Vector3> GhostPath { get; set; }
    public List<Vector3> GhostRotation { get; set; }
    public List<int> AnimationState { get; set; }
    public List<bool> InteractionState { get; set; }

    private bool isActive;
    private int index;
    private int maxIndex;
    private Vector3 finalAction;
    private Vector3 finalRotation;

    // Awake is required here
    void Awake()
    {
        Debug.Log("Spawned new ghost");
        AC = GetComponent<Animator>();
        CC = GetComponent<CharacterController>();
        isActive = false;
        isInteracting = false;
        //gameObject.SetActive(false);
        index = 0;
        onWilt += OnWilt;

        SeedCoord = new Vector3();
        SeedRot = new Quaternion();
        GhostPath = new List<Vector3>();
        GhostRotation = new List<Vector3>();
        AnimationState = new List<int>();
        InteractionState = new List<bool>();
        AC.SetTrigger("OnSpawn");
    }

    void FixedUpdate()
    {
        if (isControlling)
        {
            //Debug.Log("Ghost::Controlling");
        }
        else if (isActive && !AC.GetCurrentAnimatorStateInfo(0).IsName("Spawning"))
        {
            if (index >= GhostPath.Count)
            {
                if (index < maxIndex)
                {
                    transform.Translate(finalAction, Space.World); // Repeat last step 
                    //transform.Rotate(finalRotation, Space.Self);
                    // this.isInteracting = false;
                    index++;
                }
                else
                {
                    Reset();
                    if (isLoop)
                    {
                        Animate();
                    }
                }
            }
            else
            {
                transform.Translate(GhostPath[index], Space.World);
                transform.Rotate(GhostRotation[index], Space.Self);
                if (GhostPath[index].magnitude != 0)
                {
                    AC.SetBool("isMoving", true);
                }
                else
                {
                    AC.SetBool("isMoving", false);
                }

                if (isInteracting)
                {
                    if (InteractionState[index])
                    {
                        onInteractHold?.Invoke();
                        isInteracting = true;
                    }
                    else
                    {
                        onInteractEnd?.Invoke();
                        isInteracting = false;
                    }
                }
                else
                {
                    if (InteractionState[index])
                    {
                        onInteractStart?.Invoke();
                        isInteracting = true;
                    }
                }
                index++;
            }
        }
    }

    public void Reset()
    {
        if (isInteracting)
        {
            onInteractEnd?.Invoke();
        }
        transform.position = this.SeedCoord;
        transform.rotation = this.SeedRot;
        isActive = false;
        index = 0;
        isInteracting = false;
        //ghost.SetActive(false); //TODO: Removing this cuz it disables collider as well
    }

    public void Animate()
    {
        isActive = true;
        //gameObject.SetActive(true);
        maxIndex = (int)(duration * (1 / Time.fixedDeltaTime));
        finalAction = GhostPath[GhostPath.Count - 1];
        finalAction.y = 0; // Prevent ghost from jumping up forever
        finalRotation = GhostRotation[GhostRotation.Count - 1];
        AC.SetTrigger("OnSpawn");
    }

    public void Halt()
    {
        isActive = false;
    }

    private void OnWilt()
    {
        AC.SetTrigger("OnWilt");
    }
}
