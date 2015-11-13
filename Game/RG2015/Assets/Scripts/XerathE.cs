using UnityEngine;
using System.Collections;

public class XerathE : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Load Start and End Position
	}
	
	// Update is called once per frame
	void Update () {
        //Movement
	}

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            Debug.Log("HIT PLAYER");
            other.GetComponent<Player>().ChangeHealth(-50);
        }
    }
}
