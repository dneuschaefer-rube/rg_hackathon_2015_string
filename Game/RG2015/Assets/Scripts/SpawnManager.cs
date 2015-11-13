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
    private bool spawnAdditional;
    private float lastSpawnTime;
    private float lastBossSpawnTime;

	void Start ()
    {
        StartCoroutine(SpawnWaves());
	}

    void Update ()
    {
        if (spawnAdditional)
        {
            if (Time.time > (lastBossSpawnTime + 90))
            {
                GameObject.Instantiate(Lux, LuxSpawnPoint, Quaternion.identity);
                lastSpawnTime = Time.time;
                lastBossSpawnTime = lastSpawnTime;
            }
            else if (Time.time > (lastSpawnTime + 30))
            {
                System.Random rnd = new System.Random();
                int i = rnd.Next(1, 2);
                int j = rnd.Next(1, 2);
                if ((i == 1) && (j == 1))
                    GameObject.Instantiate(Ezreal, SpawnPoint1, Quaternion.identity);
                else if ((i == 1) && (j == 2))
                    GameObject.Instantiate(Ezreal, SpawnPoint2, Quaternion.identity);
                else if ((i == 2) && (j == 1))
                    GameObject.Instantiate(Xerath, SpawnPoint1, Quaternion.identity);
                else if ((i == 2) && (j == 2))
                    GameObject.Instantiate(Xerath, SpawnPoint2, Quaternion.identity);
                lastSpawnTime = Time.time;
            }
        }     
    }
	

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < spawnTimes.Length; i++)
        {
            yield return new WaitForSeconds(spawnTimes[i]);
            if (i == 0)
            {
                GameObject.Instantiate(Ezreal, SpawnPoint1, Quaternion.identity);
                GameObject.Instantiate(Ezreal, SpawnPoint2, Quaternion.identity);
                GameObject.Instantiate(Caitlyn, SpawnPoint1, Quaternion.identity);
            }
            else if (i == 1)
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
                spawnAdditional = true;
                lastSpawnTime = Time.time;
                lastBossSpawnTime = lastSpawnTime;
            }
        }
    }
}
