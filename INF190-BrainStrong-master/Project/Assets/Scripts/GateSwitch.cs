using UnityEngine;
using System.Collections;

public class GateSwitch : MonoBehaviour {
	private GameObject[] gates;

	// Use this for initialization
	void Start () {
		gates = new GameObject[4];
		gates[0] = GameObject.Find("Gate1");
		gates[1] = GameObject.Find("Gate2");
		gates[2] = GameObject.Find("Gate3");
		gates[3] = GameObject.Find("Gate4");
		
		foreach (GameObject gate in gates)
		{
			gate.GetComponent<MeshRenderer>().enabled = false;
			gate.GetComponent<NavMeshObstacle>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ToggleGate1()
	{ 
		gates[0].GetComponent<MeshRenderer>().enabled = !gates[0].GetComponent<MeshRenderer>().enabled;
		gates[0].GetComponent<NavMeshObstacle>().enabled = !gates[0].GetComponent<NavMeshObstacle>().enabled;
	}
	
	public void ToggleGate2()
	{ 
		gates[1].GetComponent<MeshRenderer>().enabled = !gates[1].GetComponent<MeshRenderer>().enabled;
		gates[1].GetComponent<NavMeshObstacle>().enabled = !gates[1].GetComponent<NavMeshObstacle>().enabled;
	}
	
	public void ToggleGate3()
	{ 
		gates[2].GetComponent<MeshRenderer>().enabled = !gates[2].GetComponent<MeshRenderer>().enabled;
		gates[2].GetComponent<NavMeshObstacle>().enabled = !gates[2].GetComponent<NavMeshObstacle>().enabled;
	}
	
	public void ToggleGate4()
	{ 
		gates[3].GetComponent<MeshRenderer>().enabled = !gates[3].GetComponent<MeshRenderer>().enabled;
		gates[3].GetComponent<NavMeshObstacle>().enabled = !gates[3].GetComponent<NavMeshObstacle>().enabled;
	}
}
