using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharController : MonoBehaviour
{
    public float m_speed =1;
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
            WaterMechanics();

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
    private void WaterMechanics()
    {
        
        Ray R = m_Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit RHit;
        if (Physics.Raycast(R, out RHit, m_eyeSight))
        {
            if (RHit.transform.gameObject.tag == "StoneBlock" && m_BucketEmpty == false)
            {
                World.MakeCoffe(RHit, Za_Warudo.transform);
            }
            else if (RHit.transform.gameObject.tag == "Door")
            {
                // Open/Close Door
            }
        }
        
    }
    
}
