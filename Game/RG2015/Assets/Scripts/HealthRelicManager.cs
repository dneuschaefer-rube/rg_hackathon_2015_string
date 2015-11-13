using UnityEngine;
using System.Collections;

public class HealthRelicManager : MonoBehaviour {

    public int healAmount = 200;
    public float initialSpawn = 10.0f;
    public float respawnTime = 50.0f;
    bool healthRelicSpawned = false;
    bool playerInArea = false;
    public GameObject model;
    Player player;
	// Use this for initialization
	void Start () {
        StartCoroutine(Respawn(initialSpawn));
	}
	
	// Update is called once per frame
	void Update () {
        if (playerInArea && healthRelicSpawned)
        {
            StartCoroutine(Respawn(respawnTime));
            player.ChangeHealth(healAmount);
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
        healthRelicSpawned = false;
        model.SetActive(false);
        yield return new WaitForSeconds(time);
        model.SetActive(true);
        healthRelicSpawned = true;
    }
}
