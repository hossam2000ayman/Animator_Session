using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementWithCharacterController : MonoBehaviour
{
    private float xRotation;
    private Vector2 PlayerMouseInput;
    private Vector3 PlayerMovementInput;
    private Vector3 Velocity;
    public Transform PlayerCamera;
    public float moveSpeed;
    public float JumpForce;
    public float Sensitivity;

    public float Gravity = -9.81f;

    public float minRotationValue = -15f;
    public float maxRotationValue = 15f;

    CharacterController characterController;
    Animator PLayerAnimator;

    public int comboPunch = 0;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        characterController = GetComponent<CharacterController>();
        PLayerAnimator = GetComponent<Animator>();
        PlayerMovementInput = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();
    }

    void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput);

        if (characterController.isGrounded)
        {
            //Land
            Velocity.y = -1f;
            PLayerAnimator.SetBool("isFalling", false);
            PLayerAnimator.SetBool("isLanding", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //jump
                Velocity.y = JumpForce;
                characterController.Move(Velocity * Time.deltaTime);
                PLayerAnimator.SetBool("isJumping", true);
                PLayerAnimator.SetBool("isLanding", false);
            }
        }
        else
        {
            //fall
            Velocity.y -= -2f * Gravity * Time.deltaTime;
            PLayerAnimator.SetBool("isJumping", false);
            PLayerAnimator.SetBool("isFalling", true);
            PLayerAnimator.SetBool("isLanding", false);

        }
        //move
        characterController.Move(MoveVector * moveSpeed * Time.deltaTime);
        PLayerAnimator.SetFloat("Run", PlayerMovementInput.z);
        PLayerAnimator.SetFloat("RunStrafe", PlayerMovementInput.x);

        //attack

        PlayerAttack();
        


        characterController.Move(Velocity * Time.deltaTime);
    }


    void MovePlayerCamera()
    {
        xRotation -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        float limitedxRotation = Mathf.Clamp(xRotation, minRotationValue, maxRotationValue);
        PlayerCamera.transform.localRotation = Quaternion.Euler(limitedxRotation, 0f, 0f);
    }

    void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PLayerAnimator.SetTrigger("hit1");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            PLayerAnimator.SetTrigger("hit2");
        }
    }
}
