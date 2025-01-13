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


    [Header("Misc")]
    public bool cursorLocked = true;




    void UpdateMovementDirection()
    {
        movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        movementDirection = transform.TransformDirection(movementDirection);
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
        Camera camera = Camera.main;
        float mouseDeltaX = Input.GetAxis("Mouse X");
        float mouseDeltaY = Input.GetAxis("Mouse Y");

        Vector3 previousEulerAngles = transform.eulerAngles;

        float angleX = previousEulerAngles.x + -mouseDeltaY;
        angleX *= cameraSensitivity;
        angleX = Mathf.Clamp(angleX, -80, 80);

        float angleY = previousEulerAngles.y + mouseDeltaX;
        //angleY %= 360;
        angleY *= cameraSensitivity;

        transform.eulerAngles = new Vector3(previousEulerAngles.x, angleY, previousEulerAngles.z); // Apply player eulerAngles
        camera.transform.position = transform.position + Vector3.up * cameraHeight; // Apply camera position
        camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, transform.eulerAngles.y, 0); // Apply camera eulerAngles
        camera.transform.eulerAngles += new Vector3(angleX, 0, 0);
    }


    void Update()
    {
        Cursor.lockState = (cursorLocked) ? CursorLockMode.Locked : CursorLockMode.Confined;

        UpdateMovementDirection();
        UpdateCurrentSpeed();
        UpdateCamera();
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
