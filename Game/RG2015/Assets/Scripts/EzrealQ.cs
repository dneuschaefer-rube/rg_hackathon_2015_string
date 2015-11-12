using UnityEngine;
using System.Collections;

public class EzrealQ : MonoBehaviour {

    public float speed;
    public Vector3 endPosition;
    private Vector3 startPosition;
	// Use this for initialization
	void Start () {
	    //Load Start and End Position
        startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    //Movement
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
	}

    public void setEndPosition(Vector3 position)
    {
        Debug.Log("POSITION SET");
        endPosition = position;
        transform.forward = (endPosition - startPosition).normalized;
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            Debug.Log("HIT PLAYER");
            Destroy(this.gameObject);
        }
    }
}
