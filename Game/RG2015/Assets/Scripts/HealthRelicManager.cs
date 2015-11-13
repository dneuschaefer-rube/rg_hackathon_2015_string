using UnityEngine;
using System.Collections;

public class HealthRelicManager : MonoBehaviour {

    public int healAmount = 200;
    public float initialSpawn = 10.0f;
    public float respawnTime = 50.0f;
    public float speedBoost = 1.5f;
    public float speedBoostTime = 1.0f;
    bool healthRelicSpawned = false;
    bool playerInArea = false;
    public GameObject model;
    Player player;
    GameObject[] m_Floors;
	// Use this for initialization
	void Start () {
        m_Floors = GameObject.FindGameObjectsWithTag("Floor");
        StartCoroutine(Respawn(initialSpawn));
	}
	
	// Update is called once per frame
	void Update () {
        if (playerInArea && healthRelicSpawned)
        {
            StartCoroutine(Respawn(respawnTime));
            player.ChangeHealth(healAmount);
            player.BoostSpeed(speedBoost, speedBoostTime);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            player = other.GetComponent<Player>();
            playerInArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Destroy(other.gameObject);
        if (other.tag == "Player")
        {
            playerInArea = false;
        }
    }

    IEnumerator Respawn(float time)
    {
        Bounds t_Floor = m_Floors[Random.Range(0, m_Floors.Length)].GetComponent<BoxCollider>().bounds;

        float t_X = Random.Range(t_Floor.min.x, t_Floor.max.x);
        float t_Y = Random.Range(t_Floor.min.z, t_Floor.max.z);

        this.transform.position = new Vector3(t_X, 0, t_Y);


        healthRelicSpawned = false;
        model.SetActive(false);
        yield return new WaitForSeconds(time);
        model.SetActive(true);
        healthRelicSpawned = true;
    }
}
