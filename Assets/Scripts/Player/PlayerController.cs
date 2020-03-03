using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float hopForce;
    // Rigidbody rigidbody;
    // Transform player;
    private Vector3 moveDirection;
    private Vector3 defaultCameraCoord;
    private Quaternion defaultCameraRot;
    public float gravityScale;

    private string KeyMoveVertical = "Vertical";
    private string KeyMoveHorizontal = "Horizontal";
    private string MouseMoveHorizontal = "Mouse Y";
    private string MouseMoveVertical = "Mouse X";

    private bool freezePlayer = false;
    private bool inCameraTransition = false;

    public float rotateRate = 1f;
    public float sprintMultiplier = 1.5f;
    public float jumpMultiplier = 200f;

    Animator AC;
    CharacterController CC;
    GameObject Main;
    GameObject CameraCenter;

    public UnityAction onWilt { get; set; }
    public UnityAction onInteractionStart { get; set; }
    public UnityAction onInteractionEnd { get; set; }

    public bool isInteracting { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        CC = GetComponent<CharacterController>();
        AC = GetComponent<Animator>();
        CameraCenter = GameObject.Find("CameraCenter");
        defaultCameraCoord = CameraCenter.transform.localPosition;
        defaultCameraRot = CameraCenter.transform.localRotation;
        Main = gameObject;
    }

    // Update is called once per frame
    void Update()
    {


        float turnAxisX = Input.GetAxis("Mouse Y");
        float turnAxisY = Input.GetAxis("Mouse X"); ;
        moveDirection = Input.GetAxis("Horizontal") * moveSpeed * Main.transform.right + new Vector3(0f, moveDirection.y, 0f) + Input.GetAxis("Vertical") * moveSpeed * Main.transform.forward;
        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        if (!freezePlayer)
        {
            if ((Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.E))) // Circle button
            {
                onInteractionStart?.Invoke();
                isInteracting = true;
            }
            else if ((Input.GetButtonUp("Fire3") || Input.GetKeyUp(KeyCode.E))) // Circle button
            {
                onInteractionEnd?.Invoke();
                isInteracting = false;
            }

            if (Input.GetKeyDown(KeyCode.Q)) // Triangle button
            {
                onWilt?.Invoke();
            }

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
        
            TurnPlayer(turnAxisX, turnAxisY);
            CC.Move(moveDirection * Time.deltaTime); 
        }
        TurnCamera(turnAxisX, turnAxisY);

        if (inCameraTransition && (Vector3.Distance(CameraCenter.transform.localPosition, defaultCameraCoord) > 0.1f))
        {
            TransitionCamera();
        }
        else
        {
            inCameraTransition = false;
        }
    }

    private void TransitionCamera()
    {
        //Debug.Log("Translating");
        CameraCenter.transform.Translate((defaultCameraCoord - CameraCenter.transform.localPosition) * 3 * Time.deltaTime, Space.Self);
    }

    private void TurnCamera(float turnX, float turnY)
    {
        CameraCenter.transform.Rotate(turnX * rotateRate * -1, 0, 0);
    }

    private void TurnPlayer(float turnX, float turnY)
    {
        Main.transform.Rotate(0, turnY * rotateRate, 0);
    }

    public void SwitchToGhost(Ghost ghost, Animator animator, CharacterController charCon)
    {
        Debug.Log("PC::Switch control to ghost");
        onInteractionStart = ghost.onInteractStart;
        onInteractionEnd = ghost.onInteractEnd;
        onWilt += ghost.onWilt;
        CC = charCon;
        AC = animator;
        inCameraTransition = true;
        CameraCenter.transform.parent = ghost.transform;
        //CameraCenter.transform.localPosition = defaultCameraCoord;
        CameraCenter.transform.localRotation = defaultCameraRot;
        Main = ghost.gameObject;
    }

    public void SwitchToPlayer(PlayerState PS, Animator animator, CharacterController charCon)
    {
        Debug.Log("PC::Switch control to player");
        onInteractionStart = PS.onInteractStart;
        onInteractionEnd = PS.onInteractEnd;
        onWilt = PS.onWilt;
        CC = charCon;
        AC = animator;
        inCameraTransition = true;
        CameraCenter.transform.parent = PS.transform;
        //CameraCenter.transform.localPosition = defaultCameraCoord;
        CameraCenter.transform.localRotation = defaultCameraRot;
        Main = PS.gameObject;
    }

    public void FreezePlayer()
    {
        //Debug.Log("PC::Freezing Player");
        freezePlayer = true;
    }

    public void UnfreezePlayer()
    {
        //Debug.Log("PC::Unfreezing Player");
        freezePlayer = false;
    }
}
