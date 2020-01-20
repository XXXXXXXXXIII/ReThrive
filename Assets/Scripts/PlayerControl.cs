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
    public float moveRate = -10;

    public Rigidbody Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveAxisX = Input.GetAxis(KeyMoveVertical);
        float moveAxisZ = Input.GetAxis(KeyMoveHorizontal);
        float turnAxisX = Input.GetAxis(MouseMoveHorizontal);
        float turnAxisY = Input.GetAxis(MouseMoveVertical);

        ApplyMoveInput(moveAxisX, moveAxisZ);
        ApplyTurnInput(turnAxisX, turnAxisY);
    }

    private void ApplyMoveInput(float moveX, float moveZ)
    {
        //transform.Translate(Vector3.forward * moveX * moveRate, Space.Self);
        //transform.Translate(Vector3.right * moveZ * moveRate / 3, Space.Self);

        // Press shift to run
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveRate *= 1.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveRate /= 1.5f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.AddForce(transform.up * 300, ForceMode.Force);
        }

        Player.AddForce(transform.forward * moveX * moveRate, ForceMode.Force);
        Player.AddForce(transform.right * moveZ * moveRate / 3, ForceMode.Force);
    }

    private void ApplyTurnInput(float turnX, float turnY)
    {
        transform.Rotate(0, turnY * rotateRate, 0);
    }
}
