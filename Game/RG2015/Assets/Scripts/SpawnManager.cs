using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    GameObject globalScript;
	// Use this for initialization
	void Start () {
        globalScript = GameObject.FindGameObjectWithTag("Global");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
