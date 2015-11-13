﻿using UnityEngine;
using System.Collections;

public class CaitCupcake : MonoBehaviour {

    bool m_TrapTriggered = false;
    public float cupcakeArmTime = 0.25f;
    public float stunTime = 0.5f;
    bool armed = false;
	// Use this for initialization
	void Start () {
        StartCoroutine(armCupcake());
	}

    IEnumerator armCupcake()
    {
        yield return new WaitForSeconds(cupcakeArmTime);
        armed = true;
    }
	
	// Update is called once per frame
	void Update () {
        //Wait until Trap finishes setting:
        //Debug.Log(GetComponent<Animation>().IsPlaying("CaitlyTrapSet").ToString());

        if (armed)
        {
            if (m_TrapTriggered)
            {
                if (!GetComponent<Animation>().IsPlaying("CaitlynTrapTrigger"))
                    Destroy(this.gameObject);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            GetComponent<Animation>().CrossFade("CaitlynTrapTrigger");
            GameObject.Find("CaitlynWActivateSound").GetComponent<AudioSource>().Play();
            m_TrapTriggered = true;
            Debug.Log("HIT PLAYER");
            other.GetComponent<Player>().ChangeHealth(-50);
            other.GetComponent<Player>().Stun(stunTime);
        }
    }
}
