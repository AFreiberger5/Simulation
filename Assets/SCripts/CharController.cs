﻿using UnityEngine;


public class CharController : MonoBehaviour
{
    public float m_speed = 1;
    public float m_rotationSpeed = 50;
    public float m_eyeSight = 1;
    public Camera m_Cam;
    public GameObject Za_Warudo;


    private CharacterController m_Player;
    private float m_YSpeed;
    private bool m_BucketEmpty = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_Player = GetComponent<CharacterController>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC" && m_BucketEmpty == true)
            m_BucketEmpty = false;

    }

    // Update is called once per frame
    void Update()
    {

        Movement();
        if (Input.GetButton("Fire3"))
            RayTargetActions();

    }
    private void Movement()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * m_rotationSpeed);
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move);
        move = move.normalized * Time.deltaTime * m_speed;
        if (m_Player.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                m_YSpeed = .5f;
            }
            else
            {
                m_YSpeed = -0.01f;
            }
        }
        else
        {
            m_YSpeed += -4 * Time.deltaTime;
        }
        move.y = m_YSpeed;
        m_Player.Move(move);
    }
    private void RayTargetActions()
    {
        //todo: make sure mouse will always be locked to middle of screen
        Ray R = m_Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit RHit;

        if (Physics.Raycast(R, out RHit, m_eyeSight))
        {
            Debug.DrawLine(transform.position, RHit.transform.position);

            if (RHit.transform.gameObject.tag == "StoneBlock" && m_BucketEmpty == false)
            {
                World.BrewCoffee(RHit, Za_Warudo.transform);
            }
            else if (RHit.transform.gameObject.tag == "RayDoorTop" || RHit.transform.gameObject.tag == "RayDoorBottom")
            {
                DoorSwitch(RHit);
            }
        }

    }
    private void DoorSwitch(RaycastHit _RHit)
    {
        if (_RHit.transform.gameObject.tag == "RayDoorTop")
        {
            _RHit.transform.gameObject.GetComponent<DoorTop>().OpenClose();
        }
        else if (_RHit.transform.gameObject.tag == "RayDoorBottom")
        {
            _RHit.transform.gameObject.GetComponentInParent<DoorTop>().OpenClose();
        }
    }
}
