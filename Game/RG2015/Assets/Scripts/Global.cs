using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Global : MonoBehaviour
{
	public Single GameTime;
	
	// Use this for initialization
	void Start ()
	{
		GameTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		GameTime += Time.deltaTime;

		Single GameTimeSecond = (Single)Math.Round (GameTime, 0);

		Single Minutes = Mathf.Floor(GameTimeSecond / 60);
		Single Seconds = GameTimeSecond % 60;
		String GameTimeString = Minutes + ":" + Seconds.ToString("00");

		Debug.Log ("Setting timer to " + GameTimeString);
		GameObject.Find ("TimerHudText").GetComponent<Text> ().text = GameTimeString;
	}
}
