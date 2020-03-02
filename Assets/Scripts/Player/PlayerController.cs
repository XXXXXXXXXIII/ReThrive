using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float hopForce;
    // Rigidbody rigidbody;
    // Transform player;
    private Vector3 moveDirection;
    public float gravityScale;

    private string KeyMoveVertical = "Vertical";
    private string KeyMoveHorizontal = "Horizontal";
    private string MouseMoveHorizontal = "Mouse Y";
    private string MouseMoveVertical = "Mouse X";

    private bool freezePlayer = false;

    public float rotateRate = 1f;
    public float sprintMultiplier = 1.5f;
    public float jumpMultiplier = 200f;

    Rigidbody Player;
    PlayerState PS;
    GhostManager GM;
    Animator AC;
    CharacterController CC;

    
    // Start is called before the first frame update
    void Start()
    {
        // rigidbody = GetComponent<Rigidbody>();
        CC = GetComponent<CharacterController>();
        Player = GetComponent<Rigidbody>();
        PS = GetComponent<PlayerState>();
        GM = GetComponent<GhostManager>();
        AC = GetComponent<Animator>();
        AC.SetBool("isMoving", false);
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal") * moveSpeed * transform.right + new Vector3(0f, moveDirection.y, 0f) + Input.GetAxis("Vertical") * moveSpeed * transform.forward;

        if ((Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.E))) // Circle button
        {
            PS.isInteracting = true;
            PS.onInteractStart?.Invoke();
        }
        else if ((Input.GetButtonUp("Fire3") || Input.GetKeyUp(KeyCode.E))) // Circle button
        {
            PS.isInteracting = false;
            PS.onInteractEnd?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Triangle button
        {
            // Wilt/suicide only if a seed has been planted.
            //if (GM.isRecording)
            //{
                PS.onWilt.Invoke();
            //}
            //else
            //{
            //    Debug.Log("PC::Not recording chost atm!");
            //}
        }

        /*
        if ((Input.GetKeyDown(KeyCode.F)))
        {
            PS.isInteracting = true;
            PS.onInteractStart?.Invoke();
        }

        if ((Input.GetKeyUp(KeyCode.F)))
        {
            PS.isInteracting = false;
            PS.onInteractEnd?.Invoke();
        }
        */

        // RaycastHit hit = new RaycastHit();
        // if (Physics.Raycast (player.position, -Vector3.up, out hit)) {
        //     var distanceToGround = hit.distance;
        //     Debug.Log(distanceToGround);
        // }

        // if (!CC.isGrounded) {
        //     if (Physics.Raycast(player.position, Vector3.down, 1.15f)) {
        //         Debug.Log("ground below!");
        //     };
        // }

        if (CC.isGrounded) {
            moveDirection.y = 0f;
            AC.SetBool("isMoving", false);
            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
            }
            else if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) {
                moveDirection.y = hopForce;
                AC.SetBool("isMoving", true);
            }
        }
        
        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        float turnAxisX = Input.GetAxis("Vertical2");
        float turnAxisY = Input.GetAxis("Mouse X"); ;
        if (!freezePlayer)
        {
            ApplyTurnInput(turnAxisX, turnAxisY);
            CC.Move(moveDirection * Time.deltaTime); 
        }
    }

    public void FreezePlayer()
    {
        Debug.Log("PC::Freezing Player");
        freezePlayer = true;
    }

    public void UnfreezePlayer()
    {
        Debug.Log("PC::Unfreezing Player");
        freezePlayer = false;
    }

    private void ApplyTurnInput(float turnX, float turnY)
    {
        transform.Rotate(0, turnY * rotateRate, 0);
    }
}
