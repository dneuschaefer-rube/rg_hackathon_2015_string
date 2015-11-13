using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EzrealAI : MonoBehaviour
{
    public GameObject m_QShotPrefab = null;

    private bool m_AnimationBlocking = false;
    private string m_AnimationPlaying;

    public float m_QShotChance = 20.0f;
    public float m_RunningChance = 40.0f;
    public float m_IdleChance = 40.0f;
    public float leftBoundary = -15.0f;
    public float rightBoundary = 15.0f;

    public float m_MovementSpeed = 5f;

    public Vector3 m_Origin;

    private float m_IdleTimer = 0.0f;

    private Vector3 m_Destination;
        
    enum State
    {
        UNKNOWN,
        IDLING,
        RUNNING,
        SHOOTING
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

            case State.SHOOTING:
                if (!IsShooting())
                {
				PlayAnimation("EzrealIdle", false);
                    goto case State.UNKNOWN;
                }
                break;
        }
    }

    void DecideState()
    {
        float t_Roll = Random.Range(0F, 1.0F);

        if((t_Roll -= m_QShotChance * 0.01f) < 0.0f)
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
        //Debug.Log("Ezreal is shooting.");
        m_CurrentState = State.SHOOTING;
        PlayAnimation("EzrealQ", true);
        GameObject.Find("EzrealQSound").GetComponent<AudioSource>().Play();
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        Vector3 normalizedDirection = (Player.transform.position - this.transform.position).normalized;

        transform.forward = normalizedDirection;
        GameObject projectile = (GameObject)GameObject.Instantiate(m_QShotPrefab, new Vector3(transform.position.x, 1.6f, transform.position.z), Quaternion.identity);

		projectile.GetComponent<EzrealQ>().setEndPosition(Player.transform.position + Player.GetComponent<Player>().GetWalkingDirection() * Random.Range(0f,3f));
		
		/*if(Random.Range(0, 1) > 0.5f)
			projectile.GetComponent<EzrealQ>().setEndPosition(Player.transform.position);
		else
			projectile.GetComponent<EzrealQ>().setEndPosition(Player.transform.position + Player.GetComponent<Player>().GetWalkingDirection() * Random.Range(0f, 1.8f));
    */}

    bool IsShooting()
    {
        return GetComponent<Animation>().IsPlaying("EzrealQ");
    }

    void Run()
    {
        //Debug.Log("Ezreal is running.");
        m_CurrentState = State.RUNNING;
		PlayAnimation("EzrealRun", false);

        m_Destination = transform.position;
        m_Destination.x = m_Origin.x + Random.Range(leftBoundary, rightBoundary);
    }

    void Idle()
    {
        m_IdleTimer = Random.Range(0.2f, 1.0f);
        //Debug.Log("Ezreal is idling for " + m_IdleTimer + " sec.");
        m_CurrentState = State.IDLING;
		PlayAnimation("EzrealIdle", false);
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
