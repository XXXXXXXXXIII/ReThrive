using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private string KeyMoveVertical = "Vertical";
    private string KeyMoveHorizontal = "Horizontal";
    private string MouseMoveHorizontal = "Mouse Y";
    private string MouseMoveVertical = "Mouse X";

    public float rotateRate = 1f;
    public float moveRate = 1f;
    public float sprintMultiplier = 1.5f;
    public float jumpMultiplier = 200f;

    Rigidbody Player;
    PlayerState PS;
    GhostManager GM;
    Animator AC;

    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody>();
        PS = GetComponent<PlayerState>();
        GM = GetComponent<GhostManager>();
        AC = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveAxisX = -Input.GetAxis(KeyMoveVertical);
        float moveAxisZ = -Input.GetAxis(KeyMoveHorizontal);
        float turnAxisX = Input.GetAxis(MouseMoveHorizontal);
        float turnAxisY = Input.GetAxis(MouseMoveVertical);
       
        // Press shift to run
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveRate *= sprintMultiplier;
        } 
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveRate /= sprintMultiplier;
        }

        if ((Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Q))) // Circle button
        {
            PS.onInteractStart.Invoke();
        }

        if ((Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.E))) // Triangle button
        {
            // Wilt/suicide only if a seed has been planted.
            if (GM.isRecording)
            {
                PS.onWilt.Invoke();
            }
            else
            {
                Debug.Log("PC::Not recording chost atm!");
            }
        }

        if ((Input.GetKeyDown(KeyCode.F)))
        {
            PS.isInteracting = true;
        }

        if ((Input.GetKeyUp(KeyCode.F)))
        {
            PS.isInteracting = false;
            PS.onInteractEnd.Invoke();
        }

        moveDirection = new Vector3(moveAxisZ, 0, moveAxisX).normalized;
        ApplyTurnInput(turnAxisX, turnAxisY);
        ApplyMoveInput(moveAxisX, moveAxisZ);
    }

    private void FixedUpdate()
    {
        //Player.AddForce(moveDirection * moveRate, ForceMode.Acceleration);
    }

    private void ApplyMoveInput(float moveX, float moveZ)
    {
        transform.Translate(Vector3.forward * moveX * moveRate, Space.Self);
        transform.Translate(Vector3.right * moveZ * moveRate / 3, Space.Self);
        //Player.AddForce(transform.forward * moveX * moveRate / 2, ForceMode.Force);
        //Player.AddForce(transform.right * moveZ * moveRate / 2, ForceMode.Force);
    }

    private void ApplyTurnInput(float turnX, float turnY)
    {
        transform.Rotate(0, turnY * rotateRate, 0);
    }
}
