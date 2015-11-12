using UnityEngine;
using System.Collections;
using System;

public class Rupture : MonoBehaviour {

	// Use this for initialization
    public GameObject particle;
    public GameObject mesh;

    Vector3 meshPosition;
    Vector3 popUpPosition;
    public float speed = 5.0f;
    Boolean retract = false;

    private float startTime;
    private float journeyLength;

	void Start () {
        meshPosition = mesh.transform.position;
        popUpPosition = new Vector3(meshPosition.x, meshPosition.y + 0.75f, meshPosition.z);

        startTime = Time.time;
        journeyLength = Vector3.Distance(meshPosition, popUpPosition);
	}
	
	// Update is called once per frame
	void Update () {
        if (!particle.GetComponent<ParticleSystem>().IsAlive()) 
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;

            if (retract)
                mesh.transform.position = Vector3.Lerp(popUpPosition, meshPosition, fracJourney);
            else
                mesh.transform.position = Vector3.Lerp(meshPosition, popUpPosition, fracJourney);

            if (mesh.transform.position == popUpPosition)
            {
                startTime = Time.time;
                retract = true;
                this.GetComponent<Collider>().enabled = true;
            }
            else if (mesh.transform.position == meshPosition)
                Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if(other.tag == "Player")
        {
            Debug.Log("HIT PLAYER");
        }
    }
}
