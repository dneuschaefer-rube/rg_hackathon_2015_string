using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;

public class Global : MonoBehaviour
{
	public Single GameTime;
	
	// Use this for initialization
	void Start ()
	{
		GameTime = 0;
        LoadHighscore();
        GameObject.Find("PlayerNameInput").GetComponent<InputField>().interactable = true;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (GameObject.Find("Player").GetComponent<Player>().GameOver)
            return;

        GameTime += Time.deltaTime;
		Single Score = ((Single)Math.Round (GameTime, 0)) * 500;
		GameObject.Find ("TimerHudText").GetComponent<Text> ().text = Score.ToString();
	}

    public void LoadHighscore()
    {
        String[] players = { };
        if (File.Exists("highscore.txt"))
        {
            players = File.ReadAllLines("highscore.txt");

            for (int i = 1; i <= Math.Min(3, players.Length); i++)
            {
                String[] playerInfo = players[i-1].Split(',');
                if (playerInfo.Length == 2)
                    GameObject.Find("Player" + i + "Text").GetComponent<Text>().text = playerInfo[1] + " (" + playerInfo[0] + ")";
            }
        }
    }

    public void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
