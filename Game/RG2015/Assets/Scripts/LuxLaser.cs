using UnityEngine;
using System.Collections;

public class LuxLaser : MonoBehaviour {

    public int damage = 400;
    public GameObject redLaser;
    public GameObject rainbowLaser;

    public float WarningDelay = 0.05f;

    public float redLaserTime = 0.15f;
    public float rainbowLaserTime = 0.4f;

    public float redLaserWidthStart = 0.05f;
    public float redLaserWidthEnd = 0.1f;

    public float rainbowLaserWidthStart = 1.0f;
    public float rainbowLaserWidthEnd = 7f;

    private float startTime;
    private float journeyLength;

    enum State
    {
        UNKNOWN,
        WARNING,
        SHOOT,
        DESTROY
    };

    State currentState = State.UNKNOWN;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnLuxLaser());
	}
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case State.WARNING:
                float compl = (Time.time - startTime) / redLaserTime;
                LineRenderer line = redLaser.GetComponent<LineRenderer>();
                float lineWidth = Mathf.Lerp(redLaserWidthStart, redLaserWidthEnd, compl);
                line.SetWidth(lineWidth, lineWidth);
                break;
            case State.SHOOT:
                float timeDiff = Time.time - startTime;
                float laserHalfLife = (rainbowLaserTime / 2);

                if (timeDiff < laserHalfLife)
                {
                    compl = timeDiff / rainbowLaserTime;
                    line = rainbowLaser.GetComponent<LineRenderer>();
                    lineWidth = Mathf.Lerp(rainbowLaserWidthStart, rainbowLaserWidthEnd, compl);
                    line.SetWidth(lineWidth, lineWidth);
                }
                else
                {
                    timeDiff -= laserHalfLife;
                    compl = timeDiff / rainbowLaserTime;
                    line = rainbowLaser.GetComponent<LineRenderer>();
                    lineWidth = Mathf.Lerp(rainbowLaserWidthEnd, rainbowLaserWidthStart, compl);
                    Debug.Log(compl.ToString());
                    line.SetWidth(lineWidth, lineWidth);
                }
                break;
            case State.DESTROY:
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().ChangeHealth(-damage);
        }
    }

    IEnumerator SpawnLuxLaser()
    {
        startTime = Time.time;
        currentState = State.WARNING;
        redLaser.SetActive(true);
        yield return new WaitForSeconds(redLaserTime);
        redLaser.SetActive(false);
        yield return new WaitForSeconds(WarningDelay);
        startTime = Time.time;
        rainbowLaser.SetActive(true);
        currentState = State.SHOOT;
        yield return new WaitForSeconds(rainbowLaserTime);
        rainbowLaser.SetActive(false);
        currentState = State.DESTROY;
    }
}
