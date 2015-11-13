using UnityEngine;
using System.Collections;

public class XerathE : MonoBehaviour {
    public float speed = 15.0f;
    public float maxDist = 100.0f;
    public float stunTime = 0.25f;
    public Vector3 endPosition;
    public Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        //Load Start and End Position
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        float t_Distance = Vector3.Distance(transform.position, startPosition);

        if (t_Distance > maxDist)
            Destroy(this.gameObject);
        else
            this.transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    public void setEndPosition(Vector3 position)
    {
        endPosition = position;

        Vector3 CoolBoyPosition = transform.position;
        CoolBoyPosition.y = 0;

        transform.forward = (endPosition - CoolBoyPosition).normalized;
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            Debug.Log("HIT PLAYER");

            if(!other.GetComponent<Player>().IsUndying())
            {
                other.GetComponent<Player>().ChangeHealth(-50);
                other.GetComponent<Player>().Stun(stunTime);
                Destroy(this.gameObject);
            }
            else
            {
                Vector3 CoolBoyPosition = transform.position;
                CoolBoyPosition.y = 0;

                Vector3 t_Direction = (endPosition - CoolBoyPosition).normalized;


                RaycastHit hit;
                if (!Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    return;
                }

                Vector3 t_Hit = hit.point; t_Hit.y = 0;
                Vector3 t_Origin = other.transform.position; t_Origin.y = 0;


                Vector3 t_Normal = (t_Hit - t_Origin).normalized;

                setEndPosition(transform.position + Vector3.Reflect(t_Direction, t_Normal) * 50.0f);
            }
        }
    }
}
