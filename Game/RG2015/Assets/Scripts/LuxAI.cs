using UnityEngine;
using System.Collections;

public class LuxAI : MonoBehaviour {
    public GameObject m_LuxLaserPrefab = null;

    private bool m_AnimationBlocking = false;
    private string m_AnimationPlaying;
    public float m_MovementSpeed = 5f;

    public Vector3 m_Origin;
    public Vector3 m_ShootPosition;
    public Vector3 m_Destination;

    bool fired = false;

    enum State
    {
        RUNNING,
        SHOOTING
    };

    State m_CurrentState = State.RUNNING;

    void Start()
    {
        //Pick a random Z
        Bounds t_Floor = GameObject.FindGameObjectWithTag("Floor").GetComponent<BoxCollider>().bounds;
        m_Origin.z = Random.Range(t_Floor.min.z, t_Floor.max.z);
        m_ShootPosition.z = m_Origin.z;
        m_Destination.z = m_Origin.z;
        this.transform.position = m_Origin;
        GameObject.Find("LuxLaughSound").GetComponent<AudioSource>().Play();

    }

    void Update()
    {
        switch (m_CurrentState)
        {
            case State.RUNNING:
                Vector3 t_Direction;

                if(!fired)
                    t_Direction = m_ShootPosition - transform.position;
                else
                    t_Direction = m_Destination - transform.position;

                if (t_Direction.sqrMagnitude < 0.01f)
                {
                    if (!fired)
                    {
                        m_CurrentState = State.SHOOTING;
                        FIRINGMYLAZOR();
                        fired = true;
                    }
                    else
                        Destroy(this.gameObject);
                }
                else
                {
                    t_Direction.Normalize();

                    transform.Translate(t_Direction * m_MovementSpeed * Time.deltaTime, Space.World);
                    transform.forward = t_Direction;
                }
                break;

            case State.SHOOTING:
                GameObject.Find("LuxLaughSound").GetComponent<AudioSource>().Stop();
                if (!IsShooting())
                {
                    m_CurrentState = State.RUNNING;
                    PlayAnimation("LuxRun");
                    goto case State.RUNNING;
                }
                break;
        }
    }

    void FIRINGMYLAZOR()
    {
        //Debug.Log("Ezreal is shooting.");
        m_CurrentState = State.SHOOTING;
        PlayAnimation("LuxR", true);
        GameObject.Find("LuxRSound").GetComponent<AudioSource>().Play();
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        Vector3 normalizedDirection = (Player.transform.position - this.transform.position).normalized;

        transform.forward = normalizedDirection;
        GameObject projectile = (GameObject)GameObject.Instantiate(m_LuxLaserPrefab, new Vector3(transform.position.x + 86, 3.6f, transform.position.z), new Quaternion(270, 270, 0, 1));
        projectile.GetComponent<LuxLaser>().setEndPosition(Player.transform.position);
    }

    bool IsShooting()
    {
        return GetComponent<Animation>().IsPlaying("LuxR");
    }

    void PlayAnimation(string a_Animation, bool a_Block = false)
    {
        if (!GetComponent<Animation>().isPlaying || !m_AnimationBlocking)
        {
            m_AnimationBlocking = a_Block;
            m_AnimationPlaying = a_Animation;
            GetComponent<Animation>().CrossFade(a_Animation);
        }
    }
}
