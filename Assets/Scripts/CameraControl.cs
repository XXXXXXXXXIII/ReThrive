using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private string MouseMoveHorizontal = "Mouse Y";
    private string MouseMoveVertical = "Mouse X";

    public float rotateRate = 1;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float turnAxisX = Input.GetAxis(MouseMoveHorizontal);
        float turnAxisY = Input.GetAxis(MouseMoveVertical);

        ApplyTurnInput(turnAxisX, turnAxisY);
    }

    private void ApplyTurnInput(float turnX, float turnY)
    {
        transform.Rotate(turnX * rotateRate * -1, 0, 0);
    }
}
