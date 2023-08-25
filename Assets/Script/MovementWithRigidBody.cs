using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWithRigidBody : MonoBehaviour
{
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;

    public LayerMask GroundLayer;

    public Transform FeetTransform;
    public Transform PlayerCamera;


    private  float xRotation;


    public float Speed;
    public float Sensitivity;
    public float JumpForce;

    public float minValue;
    public float maxValue;

    Rigidbody PlayerRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //inputs ( from the user)
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //[-1,1])

        MovePlayer();
        MoveCameraPlayer();
    }


    void MovePlayer()
    {                                                           //input(from the user)
        Vector3 moveVector = transform.TransformDirection(PlayerMovementInput) * Speed ;
        
        PlayerRigidBody.velocity = new Vector3(moveVector.x,PlayerRigidBody.velocity.y,moveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(FeetTransform.position , 0.1f, GroundLayer))
            {
                PlayerRigidBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
            
        }
    }
    void MoveCameraPlayer()
    {
        xRotation -= PlayerMouseInput.y * Sensitivity;
       //1st rotation ==> Player
        transform.Rotate(0f,PlayerMouseInput.x * Sensitivity,0);

        //limit rotation

        float limtedxRotation = Mathf.Clamp(xRotation, minValue, maxValue);

       //2nd Rotation ==> Camera
        PlayerCamera.transform.localRotation = Quaternion.Euler(limtedxRotation, 0f, 0f);

    }
}
