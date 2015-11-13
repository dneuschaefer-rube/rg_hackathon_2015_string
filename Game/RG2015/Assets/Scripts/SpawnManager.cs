using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    GameObject globalScript;

    public GameObject Ezreal;
    public GameObject Caitlyn;
    public GameObject Lux;
    public GameObject Xerath;

    public Vector3 SpawnPoint1;
    public Vector3 SpawnPoint2;
    public Vector3 LuxSpawnPoint;

    public float[] spawnTimes;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < spawnTimes.Length; i++)
        {
            yield return new WaitForSeconds(spawnTimes[i]);
            if(i == 0)
            {

            }
        }
    }
}
