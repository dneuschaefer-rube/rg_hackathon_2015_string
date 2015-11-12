using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public float m_MovementSpeed = 5.0f;

    public float m_DashSpeed = 15.0f;
    public float m_DashDistance = 5f;

    private Vector3 m_WalkingDestination;

    public String m_Character = "Renekton";
    private bool m_AnimationBlocking = false;
    private String m_AnimationPlaying;

	public Single[] CooldownTimes = new Single[4];
	public Single[] CurrentCooldowns = new Single[4] { 0.0f, 0.0f, 0.0f, 0.0f };

    // Use this for initialization
    void Start ()
    {
		String[] Abilities = new String[] { "Q", "W", "E", "R" };

		foreach (String s in Abilities)
		{
			Texture2D AbilityTexture = Resources.Load ("HUD/SpellIcons/" + m_Character + "/" + s, typeof(Texture2D)) as Texture2D;
			GameObject.Find (s + "_Image").GetComponent<RawImage> ().texture = AbilityTexture;
		}

		Texture2D ChampionTexture = Resources.Load ("HUD/ChampionAvatars/" + m_Character + "_Square_0", typeof(Texture2D)) as Texture2D;
		GameObject.Find ("ChampionPortrait").GetComponent<RawImage> ().texture = ChampionTexture;

		switch (m_Character)
		{
			case "Renekton":
				CooldownTimes = new Single[] { 3.0f, 3.0f, 3.0f, 3.0f };
				break;
		}
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKeyboard();
        UpdateMovement();

        if(m_AnimationBlocking)
            m_AnimationBlocking = GetComponent<Animation>().IsPlaying(m_AnimationPlaying);

		for (int i = 0; i < CurrentCooldowns.Length; i++) 
			if (CurrentCooldowns [i] > 0)
			{
				CurrentCooldowns [i] = Math.Max (CurrentCooldowns [i] - Time.deltaTime, 0);
				GameObject.Find (GetSpellName(i) + "_CooldownTimer").GetComponent<Text> ().text = CurrentCooldowns [i].ToString(("0.0"));

				if(CurrentCooldowns[i] == 0)
				{
					GameObject.Find (GetSpellName(i) + "_CooldownGray").GetComponent<Image> ().enabled = false;
					GameObject.Find (GetSpellName(i) + "_CooldownTimer").GetComponent<Text> ().text = "";
				}
			}
    }

    void UpdateKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!IsDashing() && IsWalking())
                CancelMovement();
        }

        if ((Input.GetKeyDown(KeyCode.Q)) && (!SpellOnCooldown("Q")))
        { 
            CastSpell("Q");

            switch (m_Character)
            {
                case "Renekton":
                    break;
            }


        }

		if ((Input.GetKeyDown(KeyCode.W)) && (!SpellOnCooldown("W")))
        {
            CastSpell("W");

            switch (m_Character)
            {
                case "Renekton":
                    CancelMovement();
                    break;
            }
        }

		if ((Input.GetKeyDown(KeyCode.E)) && (!SpellOnCooldown("E")))
        {
            switch (m_Character)
            {
                case "Renekton":
                    if (IsCasting('0'))
                        break;
					
                    RaycastHit hit;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, LayerMask.GetMask("Floor")) && hit.transform)
                    {
                        Vector3 t_Direction = (hit.point - transform.position).normalized * m_DashDistance;
                        m_WalkingDestination = transform.position + t_Direction;
						CastSpell("E");
                    }
                    break;
            }
        }

		if ((Input.GetKeyDown(KeyCode.R)) && (!SpellOnCooldown("E")))
		{
			switch (m_Character)
			{
				case "Renekton":
					// TODO: Renekton ult stuff
					break;
			}
		}
    }

    void UpdateMovement()
    { 
        if (Input.GetMouseButton(1) && CanWalk())
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, LayerMask.GetMask("Floor")) && hit.transform)
            {
                m_WalkingDestination = hit.point;
            }
        }

        Vector3 t_Direction = m_WalkingDestination - transform.position;

        if (t_Direction.sqrMagnitude > 0.01f)
        {
            t_Direction.y = 0;
            t_Direction = t_Direction.normalized;
            
            transform.Translate(t_Direction * GetSpeed() * Time.deltaTime, Space.World);
            transform.forward = t_Direction;
            PlayAnimation(m_Character + "Run", false);
        }
        else
			PlayAnimation(m_Character + "Idle", false);
    }

    void CancelMovement()
    {
        m_WalkingDestination = transform.position;
    }

	Int32 GetSpellIndex(String spell)
	{
		switch (spell)
		{
			case "Q":
				return 0;
			case "W":
				return 1;
			case "E":
				return 2;
			case "R":
				return 3;
		}

		return -1;
	}

	String GetSpellName (Int32 index)
	{
		switch (index)
		{
		case 0:
			return "Q";
		case 1:
			return "W";
		case 2:
			return "E";
		case 3:
			return "R";
		}
		
		return "";
	}

	void CastSpell(String spell) 
	{
		if (IsCasting('0'))
			return;

		CurrentCooldowns [GetSpellIndex(spell)] = CooldownTimes [GetSpellIndex(spell)];
		GameObject.Find (spell + "_CooldownGray").GetComponent<Image> ().enabled = true;
		GameObject.Find (spell + "_CooldownTimer").GetComponent<Text> ().text = CooldownTimes [GetSpellIndex(spell)].ToString("0.0");

		PlayAnimation (m_Character + spell, true);
	}

	Boolean SpellOnCooldown(String spell) 
	{
		return (CurrentCooldowns [GetSpellIndex(spell)] > 0);
	}

    void PlayAnimation(String a_Animation, bool a_Block)
    {
        if (!m_AnimationBlocking)
        {
            m_AnimationPlaying = a_Animation;
            GetComponent<Animation>().CrossFade(a_Animation);
        }
        
        if (a_Block && (!m_AnimationBlocking || !GetComponent<Animation>().IsPlaying(m_AnimationPlaying)))
        {
            m_AnimationBlocking = true;
        }
    }

    bool CanWalk()
    {
        if (m_Character == "Renekton" && IsCasting('W'))
            return false;

        if (IsDashing())
            return false;

        return true;
    }

    bool IsDashing()
    {
        if(m_Character == "Renekton")
            return IsCasting('E');

        return false;
    }

    bool IsWalking()
    {
        Vector3 t_Direction = m_WalkingDestination - transform.position;

        if (t_Direction.sqrMagnitude > 0.0001f)
            return true;

        return false;
    }

    float GetSpeed()
    {
        if (IsDashing())
            return m_DashSpeed;

        return m_MovementSpeed;
    }
    
    bool IsCasting(char Key)
    {
        if(Key == '0')
        {
            if (GetComponent<Animation>().IsPlaying(m_Character + 'Q') ||
                GetComponent<Animation>().IsPlaying(m_Character + 'R') ||
                GetComponent<Animation>().IsPlaying(m_Character + 'E') ||
                GetComponent<Animation>().IsPlaying(m_Character + 'W'))
                return true;
            else
				return false;
        }

        return GetComponent<Animation>().IsPlaying(m_Character + Key);
    }
}
