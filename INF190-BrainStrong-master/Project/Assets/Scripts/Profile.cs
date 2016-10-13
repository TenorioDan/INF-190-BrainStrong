using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Profile : MonoBehaviour 
{
    public string name;
	public string userName;
	public int personNum = 0;
	public int immunityStr;
	public int infectionThreshold;
	public bool infected = false;
	public bool immune = false;
	public bool beingInvaded = false;
	public int infectionStage;
	public List<int> infectorList;
	
	private float timer;
	//testing
	public int infectionLevel;
	public int nextInfectionAmount;
	public GameObject xmlInfo;
	
	private StratLayer sL;
	private GameObject infectionColider;
	
	void Awake()
	{
		GameObject.DontDestroyOnLoad (this);
		//Random.seed = (int)System.DateTime.Now.Ticks;
		xmlInfo = GameObject.Find("Main Camera");
		sL = xmlInfo.GetComponent<StratLayer>();
		//Get a random name
		name = sL.names[Random.Range (0, sL.names.Count)];
		sL.names.Remove(name);
		
		infectionThreshold = sL.infectionThreshold;
		//RNG for what each persons immunity level is
		int temp = xmlInfo.GetComponent<LevelLoader> ().GetLevel ().maxImmunityStr;
		immunityStr = Random.Range (0, temp);
		
		foreach(Transform child in transform)
		{
			if(child.tag != "MeepleCollider")
			{
				infectionColider = child.gameObject;
				infectionColider.SetActive(false);
			}
		}
		
		infectionStage = -1;
		infectionLevel = 0;
		timer = 0;
		infectorList = new List<int> ();
		//Debug.Log ("End of the awake: " + transform.position);
	}
	
	
	void FixedUpdate() 
	{
		if(xmlInfo == null)
			xmlInfo = GameObject.Find("Main Camera");
		
		if (infected) 
		{
			timer += Time.deltaTime;
			if (timer > sL.symptonList[infectionStage].timeTill )
			{
			    if( infectionStage < sL.symptonList.Count-1)
				{
					timer = 0;
					infectionStage++;
					gameObject.GetComponentInChildren<PersonCollision>().UpdateCurrentlyInfecting(GetInfectionStrength());
				}
				else if( infectionStage == sL.symptonList.Count-1)
				{
					Die();
				}
			}
		}
		else 
		{
			if(nextInfectionAmount - immunityStr > 0)
			{
				infectionLevel += (nextInfectionAmount - immunityStr);
			}
			if (infectionLevel >= infectionThreshold)
				SetInfected();
		}
	}

	void Die()
	{
		GameObject s = GameObject.Find ("Main Camera");
		s.GetComponent<StratLayer> ().CheckParkStatus (true);
		gameObject.SetActive (false);
		//infectionColider.SetActive(true);
		//GameObject.Destroy(this);
	}
	
	public void AddInfection(int infectionAmount)
	{
		nextInfectionAmount += infectionAmount;
	}
	
	public int GetInfectionStrength()
	{
		if(infectionStage >= 0)
			return sL.symptonList[infectionStage].infectStr;
		return 0;
	}
	
	public void SetInfected()
	{
		infectionStage = 0;
		infected = true;
		infectionColider.SetActive(true);
		gameObject.GetComponentInChildren<PersonCollision>().SetInfected();
		GameObject.Find ("Main Camera").GetComponent<StratLayer> ().infectedPeople++;
	}

	public void SetImmune()
	{
		Debug.Log("HELLPPPP");
		if(infected) {
			gameObject.GetComponentInChildren<SphereCollider>().enabled = false;
			gameObject.GetComponentInChildren<PersonCollision>().enabled = false;
		}
		infectionLevel = 0;
		infectionStage = -1;
		infected = false;
		immune = true;
		//After being invaded + cured, meeple should be green
		gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("Immune", typeof(Material));
		Debug.Log(gameObject.GetComponent<Renderer>().material.color);
	}
	
	public bool AddToInfectors(int person)
	{
		if (person == personNum) {
			return false;
		} 
		else 
		{
			infectorList.Add(person);
			return true;
		}
	}
	
	public int GetDailyPoints()
	{
		if(infectionStage == -1)
			return 0;
		int temp = (infectionThreshold/sL.symptonList.Count) * infectionStage+1;
		return temp;
	}
}
