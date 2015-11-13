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
        StartCoroutine(SpawnWaves());
	}
	

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < spawnTimes.Length; i++)
        {
            yield return new WaitForSeconds(spawnTimes[i]);
            if(i == 0)
            {
                GameObject.Instantiate(Ezreal, SpawnPoint1, Quaternion.identity);
                GameObject.Instantiate(Ezreal, SpawnPoint2, Quaternion.identity);
                GameObject.Instantiate(Caitlyn, SpawnPoint1, Quaternion.identity);
            }else if(i == 1)
            {
                GameObject.Instantiate(Xerath, SpawnPoint1, Quaternion.identity);
            }
            else if (i == 2)
            {
                GameObject.Instantiate(Xerath, SpawnPoint2, Quaternion.identity);
            }
            else if (i == 3)
            {
                GameObject.Instantiate(Lux, LuxSpawnPoint, Quaternion.identity);
            }
        }
    }
}
