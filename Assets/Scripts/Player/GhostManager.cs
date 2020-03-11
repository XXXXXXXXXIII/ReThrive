using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


/* 
 * Ghost Manager class
 * Attach this to player, it will record player's actions until StopRecording() is called
 */
public class GhostManager : MonoBehaviour
{
    public float minDuration = 10f;
    public float duration { get; set; }
    public bool isRecording { get; private set; }
    public float startTime { get; private set; }

    PlayerState PS;
    PlayerController PC;
    HeadsUpDisplay HUD;
    Ghost ghost;

    private Vector3 prevCoord, prevLocalCoord;
    private Quaternion prevRot;
    private bool isAttached = false;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponent<PlayerState>();
        PC = GetComponent<PlayerController>();
        HUD = GetComponent<HeadsUpDisplay>();
        isRecording = false;
        duration = minDuration;
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            if (Time.time - startTime >= duration)
            {
                ghost.onWilt?.Invoke();
                PS.onWilt?.Invoke();
            }
            else
            {
                //Debug.Log("GM::Time Remaining: " + (10f - (Time.time - startTime)));
                //Debug.Log(ghost.transform.localPosition.x - prevLocalCoord.x);
                Vector3 transformOffset = ghost.transform.localPosition - prevLocalCoord;
                if (ghost.transform.parent != null && !isAttached)
                {
                    isAttached = true;
                    transformOffset = ghost.transform.position - prevCoord;
                }
                else if (ghost.transform.parent == null && isAttached)
                {
                    isAttached = false;
                    transformOffset = ghost.transform.position - prevCoord;
                } else if (isAttached)
                {
                    transformOffset = ghost.transform.parent.TransformVector(transformOffset);
                }
                if (transformOffset.y < 0f)
                {
                    transformOffset.y = 0f;
                }
                Debug.Log(transformOffset);
                ghost.GhostPath.Add(transformOffset);
                ghost.GhostRotation.Add(ghost.transform.rotation.eulerAngles - prevRot.eulerAngles);
                ghost.GhostLinePath.Add(ghost.transform.position);
                prevCoord = ghost.transform.position;
                prevLocalCoord = ghost.transform.localPosition;
                prevRot = ghost.transform.rotation;
                ghost.InteractionState.Add(PC.isInteracting);
                //ghost.AnimationState.Add(ghost.currAnimation);
            }
        }
    }

    public void StartRecording(Ghost g)
    {
        Debug.Log("GM::Started Recording Ghost\n");
        HUD.ClearPrompt();
        HUD.PushPrompt("Press Q to wilt");

        this.ghost = g;
        ghost.isControlling = true;
        ghost.GhostPath = new List<Vector3>();
        ghost.GhostRotation = new List<Vector3>();
        ghost.InteractionState = new List<bool>();
        ghost.AnimationState = new List<int>();
        ghost.onActive += ActivateRecorder;
        ghost.onActive += PC.UnfreezePlayer;

        PC.FreezePlayer();

        prevCoord = ghost.transform.position;
        prevLocalCoord = ghost.transform.localPosition;
        prevRot = ghost.transform.rotation;
        HUD.GhostView();
    }

    // Stops recording and returns ghost
    public void StopRecording()
    {
        Debug.Log("GM::Stopped Recording Ghost\n");
        HUD.PlayerView();
        HUD.ClearPrompt();
        isRecording = false;
        ghost.isControlling = false;
        ghost.duration = this.duration;
        ghost.ResetGhost();
        ghost.onActive = null;

        LineRenderer lineRenderer = ghost.GetComponent<LineRenderer>();
        lineRenderer.positionCount = ghost.GhostLinePath.Count;
        lineRenderer.SetPositions(ghost.GhostLinePath.ToArray());
    }

    private void ActivateRecorder()
    {
        isRecording = true;
        startTime = Time.time;
    }
}
