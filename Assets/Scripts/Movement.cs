using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float hopForce;
    // public Rigidbody rigidbody;
    public Transform player;
    public CharacterController controller;
    private Vector3 moveDirection;
    public float gravityScale;

    private float rotateRate = 1f;

    private string KeyMoveVertical = "Vertical";
    private string KeyMoveHorizontal = "Horizontal";
    private string MouseMoveHorizontal = "Mouse Y";
    private string MouseMoveVertical = "Mouse X";

    public float moveRate = 1f;
    public float sprintMultiplier = 1.5f;
    public float jumpMultiplier = 200f;

    Rigidbody Player;
    PlayerState PS;
    GhostManager GM;

    
    // Start is called before the first frame update
    void Start()
    {
        // rigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        Player = GetComponent<Rigidbody>();
        PS = GetComponent<PlayerState>();
        GM = GetComponent<GhostManager>();
    }

    private bool wasGrounded;

    // Update is called once per frame
    void Update()
    {
        // rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, rigidbody.velocity.y, Input.GetAxis("Vertical") * moveSpeed);

        // if (Input.GetButtonDown("Jump")) {
        //     rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);

        // }

        //Debug.Log("Controller enabled: " + controller.enabled);

        moveDirection = -Input.GetAxis("Horizontal") * moveSpeed * transform.right + new Vector3(0f, moveDirection.y, 0f) + -Input.GetAxis("Vertical") * moveSpeed * transform.forward;

        if ((Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Space))) // X button
        {
            // TODO: Fix this
            if (Player.velocity.y == 0)
            {
                Player.AddForce(transform.up * jumpMultiplier, ForceMode.Acceleration);
            }
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
            PS.onInteract.Invoke();
        }

        // RaycastHit hit = new RaycastHit();
        // if (Physics.Raycast (player.position, -Vector3.up, out hit)) {
        //     var distanceToGround = hit.distance;
        //     Debug.Log(distanceToGround);
        // }

        // if (!controller.isGrounded) {
        //     if (Physics.Raycast(player.position, Vector3.down, 1.15f)) {
        //         Debug.Log("ground below!");
        //     };
        // }

        if (controller.isGrounded) {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump")) {
                moveDirection.y = jumpForce;
            } else if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) {
                moveDirection.y = hopForce;
            }
        }
        
        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        float turnAxisX = Input.GetAxis("Mouse Y");
        float turnAxisY = Input.GetAxis("Mouse X");
        ApplyTurnInput(turnAxisX, turnAxisY);
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void ApplyTurnInput(float turnX, float turnY)
    {
        transform.Rotate(0, turnY * rotateRate, 0);
    }
}


// bool wasGrounded;
//  void Update()
//  {
  
//      isGrounded = Physics2D.Linecast (.........);
  
//      if (isGrounded && !wasGrounded)
//          // is grounded but wasn't last time -> landed
//      else if (!isGrounded && wasGrounded)
//          // not grounded but was last Update() -> jump occurred
 
//      wasGrounded = isGrounded;
//  }