using UnityEngine;
using System.Collections;

public class HideStuff : MonoBehaviour {

	// Use this for initialization
	void Awake () 
	{
		//Hide all walls
		foreach(Transform child in GameObject.Find("Walls").transform)
		{
			child.GetComponent<MeshRenderer>().enabled = false;
		}
		//Hide all waypoints
		foreach(Transform child in GameObject.Find("Waypoints").transform)
		{
			child.GetComponent<MeshRenderer>().enabled = false;
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
