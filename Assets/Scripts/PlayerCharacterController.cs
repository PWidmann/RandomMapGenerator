using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController: MonoBehaviour
{
    //Player Values
    [Header("Health Settings")]
    public float maxHealthPoints = 50;
    public float currentHealthPoints;

    [Header("Player States")]
    public bool isAlive = true;
    public bool canInteract = false;
    public bool isGrounded = false;
    public float interactRange = 2f;
    public bool hasWon = false;
    public bool escapeMenu = false;
    public bool craftingMenu = false;
    public bool introTutorial = true;
    

    //Movement
    [Header("Movement")]
    public Vector2 inputDir;
    public Vector3 velocity;
    public bool canMove = true;
    public float walkSpeed = 2;
    public float runSpeedDefault = 6;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    public bool strafeLeft = false;
    public bool strafeRight = false;



    //References
    Animator animator;
    Transform cameraT;
    CharacterController controller;



    void Start()
    {
       
        animator = GetComponentInChildren<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        //Movement
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;
        Move(inputDir);

        // animator
        float animationSpeedPercent = currentSpeed / runSpeed;

        CheckForGrounded();
    }

    void Move(Vector2 inputDir)
    {
        //Gravity
        velocityY += Time.deltaTime * gravity;

        //Player Rotation
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        // Smooth runspeed 
        float targetSpeed = runSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        velocity = transform.forward * currentSpeed + Vector3.up * velocityY;


        controller.Move(velocity * Time.deltaTime);

        //currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
    }

   


   

    void CheckForGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        if (hit.distance > 0.1f)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

}
