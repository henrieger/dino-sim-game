using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomController : MonoBehaviour
{
    float rawZoomAmount;
    [SerializeField] float zoomThrust = 4f;
    float zoomStartTime = -1;
    float zoomAmount = 0;
    [SerializeField] float zoomAnimationTime = 0.2f;

    [SerializeField] float zoomUpperBound = 100;
    [SerializeField] float zoomLowerBound = 15;

    // Update is called once per frame
    void FixedUpdate()
    {
        bool inRightPlace = transform.position.y > zoomLowerBound && transform.position.y < zoomUpperBound;
        bool tooLow = transform.position.y < zoomLowerBound && (zoomAmount < 0 || rawZoomAmount < 0);
        bool tooHigh = transform.position.y > zoomUpperBound && (zoomAmount > 0 || rawZoomAmount > 0);

        if (inRightPlace || tooHigh || tooLow)
            Zoom(rawZoomAmount);
    }

    void Zoom(float amount)
    {
        if (zoomStartTime < 0 && amount != 0)
        {
            zoomStartTime = Time.time;
            zoomAmount = amount;
        }
        MoveCamera();
    }

    void MoveCamera()
    {
        if (rawZoomAmount != 0 && rawZoomAmount != zoomAmount)
        {
            zoomAmount = rawZoomAmount;
            zoomStartTime = Time.time;
        }

        float timePassed = Time.time - zoomStartTime;

        if (timePassed < zoomAnimationTime)
        {
            float speed = zoomThrust  * zoomAmount * (zoomAnimationTime - Mathf.Abs(2*timePassed - zoomAnimationTime));
            transform.Translate(Vector3.forward * speed, Space.Self);
        }
        else
        {
            zoomStartTime = -1;
            zoomAmount = 0;
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        rawZoomAmount = context.ReadValue<float>();
    }
}
