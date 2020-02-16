using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private string KeyMoveVertical = "Vertical";
    private string KeyMoveHorizontal = "Horizontal";
    private string MouseMoveHorizontal = "Mouse Y";
    private string MouseMoveVertical = "Mouse X";

    public float rotateRate = 1;
    public float moveRate = 1;

    Rigidbody Player;
    PlayerState PS;
    GhostManager GM;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody>();
        PS = GetComponent<PlayerState>();
        GM = GetComponent<GhostManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveAxisX = -Input.GetAxis(KeyMoveVertical);
        float moveAxisZ = -Input.GetAxis(KeyMoveHorizontal);
        float turnAxisX = Input.GetAxis(MouseMoveHorizontal);
        float turnAxisY = Input.GetAxis(MouseMoveVertical);

        ApplyMoveInput(moveAxisX, moveAxisZ);
        ApplyTurnInput(turnAxisX, turnAxisY);
    }

    private void ApplyMoveInput(float moveX, float moveZ)
    {
        transform.Translate(Vector3.forward * moveX * moveRate, Space.Self);
        transform.Translate(Vector3.right * moveZ * moveRate / 3, Space.Self);

        // Press shift to run
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveRate *= 1.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveRate /= 1.5f;
        }

        if ((Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Space)) && !PS.GetState()) // X button
        {
            PS.SetState(true);
            Player.AddForce(transform.up * 200, ForceMode.Force);
        }

        if ((Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Q))) // Circle button
        {
            PS.onPlant.Invoke();
        }

        if ((Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.E))) // Triangle button
        {
            // Wilt/suicide only if a seed has been planted.
            if (GM.isRecording)
            {
                PS.onWilt.Invoke();
            }
        }

        //Player.AddForce(transform.forward * moveX * moveRate / 2, ForceMode.Force);
        //Player.AddForce(transform.right * moveZ * moveRate / 2, ForceMode.Force);
    }

    private void ApplyTurnInput(float turnX, float turnY)
    {
        transform.Rotate(0, turnY * rotateRate, 0);
    }
}
