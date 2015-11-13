using UnityEngine;
using System.Collections;

public class EzrealQ : MonoBehaviour {

    public float speed = 15.0f;
    public float maxDist = 40.0f;
    public Vector3 endPosition;
    public Vector3 startPosition;

	// Use this for initialization
	void Start ()
	{
	    //Load Start and End Position
        startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //Movement
        float t_Distance = Vector3.Distance(transform.position, startPosition);
        
		if (t_Distance > maxDist)
            Destroy(this.gameObject);
        else
			this.transform.Translate(-transform.right * speed * Time.deltaTime, Space.World);
	}

    public void setEndPosition(Vector3 position)
    {
        endPosition = position;
        transform.forward = Quaternion.Euler(0, 90, 0) * (endPosition - startPosition).normalized;
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            Debug.Log("HIT PLAYER");
            Destroy(this.gameObject);
            other.GetComponent<Player>().Damaged();
        }
    }
}
