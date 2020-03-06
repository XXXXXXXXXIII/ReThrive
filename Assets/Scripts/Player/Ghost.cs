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

    public UnityAction onInteractStart, onInteractEnd, onInteractHold, onWilt, onActive, onVoid;

    public Animator AC { get; private set; }
    public CharacterController CC { get; private set; }

    public Vector3 SeedCoord { get; set; }
    public Quaternion SeedRot { get; set; }
    public List<Vector3> GhostPath { get; set; }
    public List<Vector3> GhostRotation { get; set; }
    public List<int> AnimationState { get; set; }
    public List<bool> InteractionState { get; set; }
    public int pressCounter { get; set; }

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
        status = Player_Status.spawning;
        pressCounter = 0;
        AC.SetTrigger("OnSpawn");
    }

    void FixedUpdate()
    {
        if (isControlling && !AC.GetCurrentAnimatorStateInfo(0).IsName("Respawn"))
        {
            if (status == Player_Status.spawning)
            {
                onActive?.Invoke();
                status = Player_Status.active;
            }
        }
        else if (isActive && !AC.GetCurrentAnimatorStateInfo(0).IsName("Respawn") && !AC.GetCurrentAnimatorStateInfo(0).IsName("Wilt"))
        {
            if (index >= GhostPath.Count)
            {
                if (index < maxIndex)
                {
                    Vector3 moveDirection = finalAction;
                    moveDirection.y += Physics.gravity.y * 5f;
                    CC.Move(moveDirection);
                    if (finalAction.magnitude > 0)
                    {
                        AC.SetBool("isMoving", true);
                    }
                    else
                    {
                        AC.SetBool("isMoving", false);
                    }
                    index++;
                }
                else
                {
                    ResetGhost();
                    if (isLoop)
                    {
                        Animate();
                    }
                }
            }
            else
            {
                Vector3 moveDirection = GhostPath[index];
                CC.Move(moveDirection);
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

    public void ResetGhost()
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
        //status = Player_Status.idle;
        //ghost.SetActive(false); //TODO: Removing this cuz it disables collider as well
    }

    public void Animate()
    {
        isActive = true;
        isInteracting = false;
        //gameObject.SetActive(true);
        maxIndex = (int)(duration * (1 / Time.fixedDeltaTime));
        finalAction = GhostPath[GhostPath.Count - 1];
        finalAction.y = 0; // Prevent ghost from jumping up forever
        finalRotation = GhostRotation[GhostRotation.Count - 1];
        status = Player_Status.spawning;
        AC.SetTrigger("OnSpawn");
    }

    public void Kill()
    {
        if (isInteracting)
        {
            onInteractEnd?.Invoke();
        }
        isActive = false;
        isInteracting = false; 
        status = Player_Status.dead;
        AC.SetTrigger("OnWilt");
    }

    public void Halt()
    {
        isActive = false;
    }

    private void OnWilt()
    {
        if (GhostPath.Count < 1)
        {
            GhostPath.Add(Vector3.zero);
            GhostRotation.Add(Vector3.zero);
            InteractionState.Add(false);
        }
        status = Player_Status.wilting;
        AC.SetTrigger("OnWilt");
    }

    public void OnInteractStart()
    {
        onInteractStart?.Invoke();
    }

    public void OnInteractEnd()
    {
        onInteractEnd?.Invoke();
    }
}
