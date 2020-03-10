using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpForce = 10;
    public float hopForce = 0;
    public float gravityScale = 5;
    public float interactHoldDuration = 2f;
    // Rigidbody rigidbody;
    // Transform player;
    private Vector3 moveDirection;
    private Vector3 defaultCameraCoord;
    private Quaternion defaultCameraRot;

    private string MoveVertical = "Vertical";
    private string MoveHorizontal = "Horizontal";
    private string LookHorizontal = "LookVertical";
    private string LookVertical = "LookHorizontal";

    private bool freezePlayer = false;
    private bool inCameraTransition = false;
    private float queueJump;

    public float rotateRate = 1f;
    public float sprintMultiplier = 1.5f;
    public float jumpMultiplier = 200f;

    Animator AC;
    CharacterController CC;
    GameObject Main;
    GameObject CameraCenter;

    public UnityAction onWilt { get; set; }
    public UnityAction onInteractStart { get; set; }
    public UnityAction onInteractHold { get; set; }
    public UnityAction onInteractEnd { get; set; }

    public bool isInteracting { get; private set; }
    private float interactHoldTime;
    public bool canPlant { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        CC = GetComponent<CharacterController>();
        AC = GetComponent<Animator>();
        CameraCenter = GameObject.Find("CameraCenter");
        defaultCameraCoord = CameraCenter.transform.localPosition;
        defaultCameraRot = CameraCenter.transform.localRotation;
        Main = gameObject;
        canPlant = false;
    }

    // Update is called once per frame
    void Update()
    {
        float turnAxisX = Input.GetAxis(LookHorizontal);
        float turnAxisY = Input.GetAxis(LookVertical);
        moveDirection = Input.GetAxis(MoveHorizontal) * moveSpeed * Main.transform.right + new Vector3(0f, moveDirection.y, 0f) + Input.GetAxis(MoveVertical) * moveSpeed * Main.transform.forward;
        moveDirection.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        if (!freezePlayer)
        {
            if ((Input.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E))) // Circle button
            {
                onInteractStart?.Invoke();                
                isInteracting = true;
                interactHoldTime = Time.time;
                canPlant = true;
            }
            else if (isInteracting && (Input.GetButtonUp("Interact") || Input.GetKeyUp(KeyCode.E))) // Circle button
            {
                onInteractEnd?.Invoke();
                isInteracting = false;
                canPlant = false;
                interactHoldTime = Mathf.Infinity;
            }
            else if (isInteracting && Time.time - interactHoldTime > interactHoldDuration)
            {
                onInteractHold?.Invoke();
                canPlant = false;
                interactHoldTime = Mathf.Infinity;
            }

            if (Input.GetButtonDown("Wilt") || Input.GetKeyDown(KeyCode.Q)) // Triangle button
            {
                onWilt?.Invoke();
            }

            if (CC.isGrounded) {
                moveDirection.y = 0f;
                AC.SetBool("isMoving", false);
                if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space) || queueJump > 0f)
                {
                    moveDirection.y = jumpForce;
                    queueJump = 0f;
                }
                else if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) {
                    //moveDirection.y = hopForce;
                    AC.SetBool("isMoving", true);
                }
            }
            else if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            {
                queueJump = 0.2f;
            }
            else if (queueJump > 0f)
            {
                queueJump -= Time.deltaTime;
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
        isInteracting = false;
        onInteractStart = ghost.OnInteractStart;
        onInteractHold = null;
        onInteractEnd = ghost.OnInteractEnd;
        onWilt += ghost.onWilt;
        CC = charCon;
        AC = animator;
        inCameraTransition = true;
        CameraCenter.transform.parent = ghost.transform;
        //CameraCenter.transform.localPosition = defaultCameraCoord;
        CameraCenter.transform.localRotation = defaultCameraRot;
        Main = ghost.gameObject;
        canPlant = false;
    }

    public void SwitchToPlayer(PlayerState PS, Animator animator, CharacterController charCon)
    {
        Debug.Log("PC::Switch control to player");
        onInteractStart = PS.OnInteractStart;
        onInteractHold = PS.OnInteractHold;
        onInteractEnd = PS.OnInteractEnd;
        onWilt = PS.onWilt;
        CC = charCon;
        AC = animator;
        inCameraTransition = true;
        CameraCenter.transform.parent = PS.transform;
        //CameraCenter.transform.localPosition = defaultCameraCoord;
        CameraCenter.transform.localRotation = defaultCameraRot;
        Main = PS.gameObject;
        canPlant = false;
    }

    public void FreezePlayer()
    {
        //Debug.Log("PC::Freezing Player");
        isInteracting = false;
        freezePlayer = true;
    }

    public void UnfreezePlayer()
    {
        //Debug.Log("PC::Unfreezing Player");
        isInteracting = false;
        freezePlayer = false;
        canPlant = false;
    }
}
