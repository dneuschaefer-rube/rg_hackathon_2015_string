using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public float MovementSpeed = 3.0f;

    // Use this for initialization
    void Start ()
    {
        GetComponent<Animation>().Play("NidaleeRun");
    }
	
	// Update is called once per frame
	void Update ()
    {
        float t_MovementHorizontal = Input.GetAxis("Horizontal") * MovementSpeed;
        float t_MovementVertical = Input.GetAxis("Vertical") * MovementSpeed;

        Vector3 t_Direction = new Vector3(t_MovementHorizontal * Time.deltaTime, 0.0f, t_MovementVertical * Time.deltaTime);
        
        transform.Translate(t_Direction, Space.World);

        // TODO FIX LOL 
        //t_Direction.Normalize();
        //transform.Rotate(new Vector3(0, 1, 0), Mathf.Atan2(t_Direction.y, t_Direction.x));
    }
}
