using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    public Vector3 translation;
    public Vector3 rotation;
    public Vector3 scale;
    public bool resetOnRelease = false;
    public bool toggleRender = false;
    //public GameObject spawnObject;

    private GameObject currObject;
    private Vector3 defaultCoord;
    private Quaternion defaultRot;
    private Vector3 defaultScale;
    
    private bool isTranslating;
    private bool isRotating;
    private bool isScaling;

    //TODO: Can't use scale/rotate/translate together with reset on release
    private void Start()
    {
        currObject = gameObject;
        defaultCoord = gameObject.transform.position;
        defaultRot = gameObject.transform.rotation;
        defaultScale = gameObject.transform.localScale;

        isTranslating = false;
        isRotating = false;
        isScaling = false;
    }

    private void FixedUpdate()
    {
        if (isTranslating)
        {
            transform.Translate(translation, Space.Self);
        }
        
        if (isRotating)
        {
            transform.Rotate(rotation, Space.Self);
        }

        if (!isTranslating && !isRotating && resetOnRelease)
        {
            if (transform.position != defaultCoord)
            {
                transform.Translate(-translation, Space.Self);
            }

            if (transform.rotation != defaultRot)
            {
                transform.Rotate(-rotation, Space.Self);
            }
        }
    }

    public void EventTester()
    {
        Debug.Log("Event Triggered");
    }

    public void ActivateObject()
    {
        if (toggleRender)
        {
            currObject.SetActive(true);
        }
    }

    public void DeActivateObject()
    {
        if (toggleRender)
        {
            currObject.SetActive(false);
        }
    }

    public void StartTranslate()
    {
        isTranslating = true;
    }

    public void StopTranslate()
    {
        isTranslating = false;
    }

    public void StartRotate()
    {
        isRotating = true;
    }

    public void StopRotate()
    {
        isRotating = false;
    }
}
