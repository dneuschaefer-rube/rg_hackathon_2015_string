using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public float m_MovementSpeed = 5.0f;

    public float m_DashSpeed = 15.0f;
    public float m_DashDistance = 5f;

    private Vector3 m_WalkingDestination;

    public String m_Character = "Renekton";
    private bool m_AnimationBlocking = false;
    private String m_AnimationPlaying;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateKeyboard();
        UpdateMovement();

        if(m_AnimationBlocking)
            m_AnimationBlocking = GetComponent<Animation>().IsPlaying(m_AnimationPlaying);
    }

    void UpdateKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!IsDashing() && IsWalking())
                CancelMovement();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAnimation(m_Character + "Q", true);

            switch (m_Character)
            {
                case "Renekton":
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayAnimation(m_Character + "W", true);

            switch (m_Character)
            {
                case "Renekton":
                    CancelMovement();
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            switch (m_Character)
            {
                case "Renekton":
                    if (IsCasting())
                        break;

                    PlayAnimation(m_Character + "E", true);
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, LayerMask.GetMask("Floor")) && hit.transform)
                    {
                        Vector3 t_Direction = (hit.point - transform.position).normalized * m_DashDistance;

                        m_WalkingDestination = transform.position + t_Direction;
                    }


                    break;
            }
        }
    }

    void UpdateMovement()
    { 
        if (Input.GetMouseButton(1) && CanWalk())
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
            
            transform.Translate(t_Direction * GetSpeed() * Time.deltaTime, Space.World);
            transform.forward = t_Direction;
            PlayAnimation(m_Character + "Run");
        }
        else PlayAnimation(m_Character + "Idle");
    }

    void CancelMovement()
    {
        m_WalkingDestination = transform.position;
    }

    void PlayAnimation(String a_Animation, bool a_Block = false)
    {
        if (!m_AnimationBlocking)
        {
            m_AnimationPlaying = a_Animation;
            GetComponent<Animation>().CrossFade(a_Animation);
        }
        
        if (a_Block && (!m_AnimationBlocking || !GetComponent<Animation>().IsPlaying(m_AnimationPlaying)))
        {
            m_AnimationBlocking = true;
        }
    }

    bool CanWalk()
    {
        if (m_Character == "Renekton" && IsCasting('W'))
            return false;

        if (IsDashing())
            return false;


        return true;
    }

    bool IsDashing()
    {
        if(m_Character == "Renekton")
        {
            return IsCasting('E');
        }

        return false;
    }

    bool IsWalking()
    {
        Vector3 t_Direction = m_WalkingDestination - transform.position;
        if (t_Direction.sqrMagnitude > 0.01f)
            return true;
        return false;
    }

    float GetSpeed()
    {
        if (IsDashing())
            return m_DashSpeed;

        return m_MovementSpeed;
    }
    
    bool IsCasting(char Key = '0')
    {
        if(Key == '0')
        {
            if (GetComponent<Animation>().IsPlaying(m_Character + 'Q') ||
                GetComponent<Animation>().IsPlaying(m_Character + 'R') ||
                GetComponent<Animation>().IsPlaying(m_Character + 'E') ||
                GetComponent<Animation>().IsPlaying(m_Character + 'W'))
                return true;
            else return false;
        }

        return GetComponent<Animation>().IsPlaying(m_Character + Key);
    }
}
