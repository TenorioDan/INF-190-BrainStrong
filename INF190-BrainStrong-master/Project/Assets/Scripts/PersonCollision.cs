using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonCollision : MonoBehaviour 
{
	private int infectionStrength;
	private int oldInfectionStrength;
	private bool needChange;
	private int personNum;
	
	private List<GameObject> currentlyInfecting;
	private Profile p;
	
	void Awake()
	{
		p = gameObject.GetComponentInParent<Profile>();
		infectionStrength = p.GetInfectionStrength();
		needChange = false;
		personNum = p.personNum;
		currentlyInfecting = new List<GameObject>();
	}
	
	void OnTriggerEnter(Collider other)
	{
		Profile tempProf = other.GetComponent<Profile>();
		if((other.tag == "Person") && (!tempProf.infected) && (!tempProf.immune))
		{
			if(tempProf.AddToInfectors(personNum))
			{
				if(currentlyInfecting == null)
					currentlyInfecting = new List<GameObject>();
				currentlyInfecting.Add(other.gameObject);
				tempProf.AddInfection(infectionStrength);
			}
		}
	}
	/*
	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Person" && (!other.GetComponent<Profile> ().infected))
		{
			if(needChange) 
			{
				other.GetComponent<Profile> ().AddInfection (-oldInfectionStrength);
				int temp = other.GetComponent<Profile> ().nextInfectionAmount;
				int temp2 = gameObject.GetComponentInParent<Profile>().GetInfectionStrength();
				if((other.GetComponent<Profile> ().infectorList.Count == 1) && 
				   (other.GetComponent<Profile> ().nextInfectionAmount > 0))
				{
					Debug.Log ("This is an error");
				}
				oldInfectionStrength = infectionStrength;
				other.GetComponent<Profile> ().AddInfection (infectionStrength);
				needChange = false;
			}
			else if (!other.GetComponent<Profile> ().infectorList.Contains (personNum)) 
			{
				other.GetComponent<Profile> ().AddToInfectors(personNum);
				other.GetComponent<Profile> ().AddInfection (infectionStrength);
			}
		} 
	}
	*/
	
	void OnTriggerExit(Collider other)
	{
		Profile tempProf = other.GetComponent<Profile>();
		if((other.tag == "Person") && (!tempProf.infected) && (!tempProf.immune))
		{
			//other.GetComponent<Profile>().AddInfection(-infectionStrength);
			tempProf.infectorList.Remove(personNum);
			currentlyInfecting.Remove(other.gameObject);
			tempProf.AddInfection(-infectionStrength);
			
			//Debugging
			int temp = p.GetInfectionStrength();
			if((tempProf.nextInfectionAmount < 0))
			{
				Debug.Log ("This is an error");
				tempProf.nextInfectionAmount = 0;
				int i = 0;
			}

			if((tempProf.infectorList.Count <= 0) && (tempProf.nextInfectionAmount > 0))
			{
				Debug.Log ("This is an error");
			}
		}
	}
	
	public void UpdateCurrentlyInfecting(int newInfection)
	{
		foreach (GameObject g in currentlyInfecting)
		{
			g.GetComponent<Profile>().AddInfection(-infectionStrength);
			g.GetComponent<Profile>().AddInfection(newInfection);
			
		}
		infectionStrength = newInfection;
	}

	public void SetInfected()
	{
		infectionStrength = p.GetInfectionStrength();
	}
}
