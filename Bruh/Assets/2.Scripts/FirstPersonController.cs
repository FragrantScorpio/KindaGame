using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float speed = 10f;
        
    public float gravity = -9.8f;
    public float groundDistance = 0.4f;
    public float jumpHeigt = 3f;


    Vector3 velocity;
    bool IsGrounded;
    void OnFoot()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;


        velocity.y += gravity * Time.deltaTime;
        controller.Move(move * Time.deltaTime * speed);
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            velocity.y = jumpHeigt * -2f * gravity ;
            controller.Move(transform.up * Time.deltaTime * velocity.y);
        }   

        if (Input.GetKey("c")) { controller.height = 1.5f; }
        else { controller.height = 2.5f; }
        if (Input.GetKey("left shift")) { speed = 8f; }
        else { speed = 4f; }
    }

    // Update is called once per frame
    void Update()
    {

        OnFoot();


    }
}
