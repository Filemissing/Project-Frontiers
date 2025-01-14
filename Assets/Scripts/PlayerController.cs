using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody rigidbody;


    [Header("Movement")]
    public Vector3 movementDirection;
    public float walkSpeed = 360;
    public float runSpeed = 440;
    float currentSpeed;


    [Header("Camera")]
    [SerializeField] float cameraSensitivity = 1;
    [SerializeField] float cameraHeight = 1.5f;


    [Header("Camera Effects")]
    [SerializeField] bool isBobbingEnabled;
    [SerializeField] float bobbingAmplitude = .025f;


    [Header("Misc")]
    public bool isCursorLocked = true;
    public float movementStartTime;




    void UpdateMovementDirection()
    {
        Vector3 previousMovementDirection = movementDirection;

        movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        movementDirection = transform.TransformDirection(movementDirection);

        if (previousMovementDirection == Vector3.zero && movementDirection != Vector3.zero)
            movementStartTime = Time.time;
    }


    void UpdateCurrentSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;
    }


    void AddMovementForce()
    {
        Vector3 targetVelocity = movementDirection * currentSpeed;
        Vector3 vectorDifference = targetVelocity - rigidbody.velocity;
        Vector3 force = vectorDifference;
        rigidbody.AddForce(force);
    }


    void UpdateCamera()
    {
        float ClosestIfBetween(float value, float low, float high)
        {
            if (value > low && value < high)
            {
                float mid = (high - low) / 2 + low;
                return value < mid ? low : high;
            }
            return value;
        }

        float mouseDeltaX = Input.GetAxis("Mouse X");
        float mouseDeltaY = Input.GetAxis("Mouse Y");
        Camera camera = Camera.main;

        Vector3 previousEulerAngles = transform.eulerAngles;

        float angleX = previousEulerAngles.x + -mouseDeltaY;
        angleX *= cameraSensitivity;
        angleX = Mathf.Clamp(angleX, -80, 80);

        float angleY = previousEulerAngles.y + mouseDeltaX;
        //angleY %= 360;
        angleY *= cameraSensitivity;

        float eulerX = ClosestIfBetween(camera.transform.eulerAngles.x + angleX, 70, 270); //camera.transform.eulerAngles.x + angleX; //Mathf.Clamp(camera.transform.eulerAngles.x + angleX, -80, 270);
        float eulerY = angleY;

        if (eulerX <= 0)
            eulerX += 360;
        //eulerX %= 360;

        transform.eulerAngles = new Vector3(previousEulerAngles.x, angleY, previousEulerAngles.z); // Apply player eulerAngles
        camera.transform.position = transform.position + Vector3.up * cameraHeight; // Apply camera position
        camera.transform.eulerAngles = new Vector3(eulerX, eulerY, 0); // Apply camera eulerAngles
        //camera.transform.eulerAngles += new Vector3(angleX, 0, 0);
    }


    // Camera Effects

    void BobbingEffect()
    {
        Camera camera = Camera.main;
        Vector3 localMovementDirection = transform.InverseTransformDirection(movementDirection);
        Vector3 offset = Vector3.zero;

        if (localMovementDirection != Vector3.zero)
            offset = new Vector3(0, Mathf.Sin((Time.time - movementStartTime) * 15) * .01f * bobbingAmplitude * currentSpeed, 0);

        camera.transform.position += offset;
    }

    void Update()
    {
        Cursor.lockState = (isCursorLocked) ? CursorLockMode.Locked : CursorLockMode.Confined;

        UpdateMovementDirection();
        UpdateCurrentSpeed();
        UpdateCamera();

        // Camera Effects
        Debug.Log(movementStartTime);
        if (isBobbingEnabled) BobbingEffect();
    }


    void FixedUpdate()
    {
        AddMovementForce();
    }


    void Reset()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
}
