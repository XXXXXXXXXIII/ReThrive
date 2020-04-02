using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Pointer : MonoBehaviour
{
  
  private Vector3 targetPosition;
  private RectTransform pointerRectTransform;
  private void Awake() {
      targetPosition = new Vector3(200, 45);
      pointerRectTransform = transform.Find("pointer").GetComponent<RectTransform>();
  }

    // Update is called once per frame
    private void Update()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f; 
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = UtilsClass.GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

    }
}
