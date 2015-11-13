using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaitlynAI : MonoBehaviour
{
    public GameObject m_TrapPrefab = null;

    private bool m_AnimationBlocking = false;
    private string m_AnimationPlaying;

    public float m_TrapPlacingChance = 20.0f;
    public float m_RunningChance = 40.0f;
    public float m_IdleChance = 40.0f;
    public float leftBoundary = -15.0f;
    public float rightBoundary = 15.0f;

    public float m_MovementSpeed = 5f;
    public uint m_MaxTraps = 15;

    public Vector3 m_Origin;

    private float m_IdleTimer = 0.0f;

    private Vector3 m_Destination;

    private List<GameObject> m_OwnedTraps = new List<GameObject>();
    
    enum State
    {
        UNKNOWN,
        IDLING,
        RUNNING,
        PLACING_TRAP
    };

    State m_CurrentState = State.UNKNOWN;

    GameObject[] m_Floors;

    void Start ()
    {
        m_Floors = GameObject.FindGameObjectsWithTag("Floor");
    }
	
	void Update ()
    {
        switch(m_CurrentState)
        {
            case State.UNKNOWN:
                DecideState();
                break;

            case State.IDLING:
                if (m_IdleTimer <= 0)
                    goto case State.UNKNOWN;
                m_IdleTimer -= Time.deltaTime;
                break;

            case State.RUNNING:
                Vector3 t_Direction = m_Destination - transform.position;
                if (t_Direction.sqrMagnitude < 0.01f)
                {
                    goto case State.UNKNOWN;
                }
                else
                {
                    t_Direction.Normalize();

                    transform.Translate(t_Direction * m_MovementSpeed * Time.deltaTime, Space.World);
                    transform.forward = t_Direction;
                }
                break;

            case State.PLACING_TRAP:
                if (!IsPlacingTrap())
                {
                    PlayAnimation("CaitlynIdle", false);
                    goto case State.UNKNOWN;
                }
                break;
        }
    }

    void DecideState()
    {
        float t_Roll = Random.Range(0F, 1.0F);

        if((t_Roll -= m_TrapPlacingChance * 0.01f) < 0.0f)
        {
            PlaceTrap();
        }
        else if ((t_Roll -= m_RunningChance * 0.01f) < 0.0f)
        {
            Run();
        }
        else if ((t_Roll -= m_IdleChance * 0.01f) < 0.0f)
        {
            Idle();
        }
    }

    void PlaceTrap()
    {
        //Debug.Log("Caitlyn is placing a trap.");
        m_CurrentState = State.PLACING_TRAP;
        PlayAnimation("CaitlynW", true);


        Bounds t_Floor = m_Floors[Random.Range(0, m_Floors.Length)].GetComponent<BoxCollider>().bounds;

        float t_X = Random.Range(t_Floor.min.x, t_Floor.max.x);
        float t_Y = Random.Range(t_Floor.min.z, t_Floor.max.z);

        Vector3 t_TrapDestination = new Vector3(t_X, 0, t_Y);

        transform.forward = (t_TrapDestination - transform.position).normalized;
        m_OwnedTraps.Add((GameObject)GameObject.Instantiate(m_TrapPrefab, t_TrapDestination, Quaternion.identity));

        if(m_OwnedTraps.Count > m_MaxTraps)
        {
            Destroy(m_OwnedTraps[0]);
            m_OwnedTraps.Remove(m_OwnedTraps[0]);
        }

    }

    bool IsPlacingTrap()
    {
        return GetComponent<Animation>().IsPlaying("CaitlynW");
    }

    void Run()
    {
        //Debug.Log("Caitlyn is running.");
        m_CurrentState = State.RUNNING;
        PlayAnimation("CaitlynRun", false);

        m_Destination = transform.position;
        m_Destination.x = m_Origin.x + Random.Range(leftBoundary, rightBoundary);
    }

    void Idle()
    {
        m_IdleTimer = Random.Range(0.2f, 1.0f);
        //Debug.Log("Caitlyn is idling for " + m_IdleTimer + " sec.");
        m_CurrentState = State.IDLING;
		PlayAnimation("CaitlynIdle", false);
    }

    void PlayAnimation(string a_Animation, bool a_Block)
    {
        if (!GetComponent<Animation>().isPlaying || !m_AnimationBlocking)
        {
            m_AnimationBlocking = a_Block;
            m_AnimationPlaying = a_Animation;
            GetComponent<Animation>().CrossFade(a_Animation);
        }
    }
}
