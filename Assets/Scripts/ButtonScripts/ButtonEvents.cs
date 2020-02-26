using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    public bool useTranslation = false;
    public Vector3 translationOffset;
    public float translationSpeed = 1f;
    public AnimationCurve translationCurve;
    private float _translationTime = 0;

    public bool useRotation = false;
    public Vector3 rotationOffset; //TODO: Doesn't support more than 179 degrees
    public float rotationSpeed = 1f;
    public AnimationCurve rotationCurve;
    private float _rotationTime = 0;

    public Vector3 scale;

    public bool resetOnRelease = false;
    public uint requiredTriggerCount = 1;
    //public GameObject spawnObject;

    public bool useAnimator = false;
    public string animatorTrigger;
    private Animator _animator;

    private Vector3 defaultCoord;
    private Quaternion defaultRot;
    private Vector3 defaultScale;
    
    private bool isTranslating;
    private bool isRotating;
    private bool isScaling;
    private int triggerCount;

    private void Start()
    {
        defaultCoord = gameObject.transform.position;
        defaultRot = gameObject.transform.rotation;
        defaultScale = gameObject.transform.localScale;

        isTranslating = false;
        isRotating = false;
        isScaling = false;
        triggerCount = 0;

        _translationTime = 0;
        _rotationTime = 0;

        if (useAnimator)
        {
            _animator = gameObject.GetComponent<Animator>();
        }
    }

    private void FixedUpdate()
    {
        if (useTranslation)
        {
            UpdateTranslation();
        }

        if (useRotation)
        {
            UpdateRotation();
        }
    }

    private void UpdateTranslation()
    {
        if (isTranslating && (requiredTriggerCount <= 1 || triggerCount >= requiredTriggerCount))
        {
            if (Vector3.Distance(defaultCoord, gameObject.transform.position) < Vector3.Distance(defaultCoord, defaultCoord + translationOffset))
            {
                transform.Translate(translationOffset.normalized * Mathf.Lerp(0.00f, translationSpeed, translationCurve.Evaluate(_translationTime)), Space.World);
                _translationTime += Time.fixedDeltaTime;
            }
            else
            {
                transform.position = defaultCoord + translationOffset; //TODO: I hope the translate won't overshoot the dest coord too much
            }
        }
        else if (resetOnRelease) //TODO: Prevent running once set in place
        {
            if (Vector3.Distance(gameObject.transform.position, defaultCoord + translationOffset) < Vector3.Distance(defaultCoord, defaultCoord + translationOffset))
            {
                transform.Translate(-translationOffset.normalized * Mathf.Lerp(0.00f, translationSpeed, translationCurve.Evaluate(_translationTime)), Space.World);
                _translationTime -= Time.fixedDeltaTime;
            }
            else
            {
                transform.position = defaultCoord;
            }
        }
    }

    private void UpdateRotation()
    {
        if (isRotating && (requiredTriggerCount <= 1 || triggerCount >= requiredTriggerCount))
        {
            if (Quaternion.Angle(defaultRot, gameObject.transform.rotation) < Quaternion.Angle(defaultRot, defaultRot * Quaternion.Euler(rotationOffset)))
            {
                transform.Rotate(rotationOffset.normalized * Mathf.Lerp(0f, rotationSpeed, rotationCurve.Evaluate(_rotationTime)));
                _rotationTime += Time.fixedDeltaTime;
            }
            else
            {
                transform.rotation = defaultRot * Quaternion.Euler(rotationOffset);
            }
        }
        else if (resetOnRelease)
        {
            if (Quaternion.Angle(gameObject.transform.rotation, defaultRot * Quaternion.Euler(rotationOffset)) < Quaternion.Angle(defaultRot, defaultRot * Quaternion.Euler(rotationOffset)))
            {
                transform.Rotate(-rotationOffset.normalized * Mathf.Lerp(0f, rotationSpeed, rotationCurve.Evaluate(_rotationTime)));
                _rotationTime -= Time.fixedDeltaTime;
            }
            else
            {
                transform.rotation = defaultRot;
            }
        }
    }

    public void EventTester()
    {
        Debug.Log("Event Triggered");
    }

    public void TriggerObject()
    {
        triggerCount++;
    }

    public void UntriggerObject()
    {
        triggerCount--;
    }

    public void ActivateObject()
    {
        gameObject.SetActive(true);
    }

    public void DeActivateObject()
    {
        gameObject.SetActive(false);
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

    public void SetAnimatorTrigger()
    {
        _animator.SetTrigger(animatorTrigger);
    }

    public void UnsetAnimatorTrigger()
    {
        _animator.SetTrigger(animatorTrigger);
    }
}
