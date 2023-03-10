using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Vector3 movement = Vector3.zero;
    
    public Camera playerCamera;
    public float walkSpeed = 0.00004f;
    public float runSpeed = 0.00008f;
    public float crouchSpeed = 0.7f;

    public float rotationSpeed = 9f;
    public float rotationLimit = 120f;
    public float rotationX = 0f;

    public Vector3 crouchScale = new Vector3 (1f, 0.5f, 1f);
    public Vector3 normalScale = new Vector3 (1f, 1f, 1f);

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        movement.y = -9.81f;
        Move();
    }

    private void ApplyRotation()
    {
        rotationX += -Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, -rotationLimit, rotationLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * rotationSpeed, 0f);
    }

    private void ApplyMovement()
    {
        Vector3 forwardBackward = transform.TransformDirection(Vector3.forward);
        Vector3 leftRight = transform.TransformDirection(Vector3.right);
        float currSpeed = walkSpeed;

        bool isCrouching = Input.GetKey(KeyCode.LeftControl);
        if (isCrouching)
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            currSpeed = crouchSpeed;
        }
        else
        {
            transform.localScale = normalScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning && !isCrouching)
            currSpeed = runSpeed;

        movement = (forwardBackward * currSpeed * Input.GetAxis("Vertical")) + (leftRight * currSpeed * Input.GetAxis("Horizontal"));
        Move();
    }

    private void Move()
    {
        characterController.Move(movement * Time.deltaTime);
    }
}
