using UnityEngine;
using System.Collections;

public class Healthrelic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    //Rotate Health Relic
        transform.Rotate(0, 50 * Time.deltaTime, 0); //rotates 50 degrees per second around z axis
	}

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Heal();
        }
    }
}
