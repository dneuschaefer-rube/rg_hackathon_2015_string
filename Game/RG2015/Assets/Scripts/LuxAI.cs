using UnityEngine;
using System.Collections;

public class LuxAI : MonoBehaviour {
    public GameObject m_LuxLaserPrefab = null;

    private bool m_AnimationBlocking = false;
    private string m_AnimationPlaying;
    public float m_MovementSpeed = 5f;

    public Vector3 m_Origin;
    public Vector3 m_Destination;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
