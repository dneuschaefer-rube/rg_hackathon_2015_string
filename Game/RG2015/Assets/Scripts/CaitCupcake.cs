using UnityEngine;
using System.Collections;

public class CaitCupcake : MonoBehaviour {

    bool m_TrapTriggered = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Wait until Trap finishes setting:
        //Debug.Log(GetComponent<Animation>().IsPlaying("CaitlyTrapSet").ToString());

        if(m_TrapTriggered)
        {
            if (!GetComponent<Animation>().IsPlaying("CaitlynTrapTrigger"))
                Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            GetComponent<Animation>().CrossFade("CaitlynTrapTrigger");
            m_TrapTriggered = true;
            Debug.Log("HIT PLAYER");
        }
    }
}
