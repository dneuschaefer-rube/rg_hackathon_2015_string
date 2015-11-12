using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public float m_MovementSpeed = 5.0f;
    private Vector3 m_WalkingDestination;
    private float m_AnimationBlocking = 0.0f;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateKeyboard();
        UpdateMovement();

        HandleAnimation();
    }

    void UpdateKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAnimation("RenektonQ", true);
        }
    }

    void UpdateMovement()
    { 
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, LayerMask.GetMask("Floor")) && hit.transform)
            {
                m_WalkingDestination = hit.point;
            }
        }

        Vector3 t_Direction = m_WalkingDestination - transform.position;
        if (t_Direction.sqrMagnitude > 0.01f)
        {
            t_Direction.y = 0;
            t_Direction = t_Direction.normalized;

            transform.Translate(t_Direction * m_MovementSpeed * Time.deltaTime, Space.World);
            transform.forward = t_Direction;
            PlayAnimation("RenektonRun");
        }
        else PlayAnimation("RenektonIdle");
    }

    void PlayAnimation(String a_Animation, bool a_Block = false)
    {
        if (m_AnimationBlocking <= 0)
        {
            if(!a_Block)
                GetComponent<Animation>().CrossFade(a_Animation);
            else GetComponent<Animation>().Play(a_Animation);
        }


        if (a_Block && m_AnimationBlocking <= 0)
        {
            m_AnimationBlocking = GetComponent<Animation>().GetClip(a_Animation).length;
        }
    }

    void HandleAnimation()
    {
        m_AnimationBlocking -= Time.deltaTime;
    }
}
