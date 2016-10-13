using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering;

public class SusceptibleSweep : MonoBehaviour {
	private int sweepCost = 1;
	private GameObject sweepPanel;
	public GameObject sweepHighlighter;
	private bool choosing;
	private Collider[] meeplesToScan;
	private float secsToDisplay = 2f;
	private float lastDisplayed = 0f;
	private float lastSweep = 0f;
	private float cooldown = 0f;
	private EventSystem e;
	private Color defaultTextColor;
	private GameObject dailyUI;
	
	private Ray ray;
	private RaycastHit hit;
	
	private MouseClick mouseClickScript;
	private CameraControl cameraControlScript;
	
	private float haloSecsToDisplay = 5f;
	private float haloLastDisplayed = 0f;
	
	// Use this for initialization
	void Awake () 
	{
		sweepPanel = GameObject.Find("SweepPanel");
		sweepPanel.GetComponentInChildren<Text>().text = "";
		sweepPanel.SetActive(false);
		sweepHighlighter = GameObject.Find("SweepHighlighter");
		sweepHighlighter.GetComponent<MeshRenderer>().enabled = false;
		choosing = false;
		e = GameObject.Find("EventSystem").GetComponent<EventSystem>();
		mouseClickScript = GameObject.Find("Main Camera").GetComponent<MouseClick>();
		cameraControlScript = GameObject.Find("Main Camera").GetComponent<CameraControl>();
		meeplesToScan = new Collider[0];
		sweepHighlighter.GetComponent<Renderer>().material.renderQueue = 5000;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float currentTime = Time.time;
		if(sweepPanel.activeSelf == true)
		{
			if(currentTime - lastDisplayed > secsToDisplay)
			{
				sweepPanel.SetActive(false);
			}
		}
		
		if(meeplesToScan.Length > 0)
		{
			if(currentTime - haloLastDisplayed > haloSecsToDisplay)
			{
				foreach (Collider meeple in meeplesToScan)
				{
					if(meeple.gameObject.tag == "Person")
					{
						//Turn off halo
						Behaviour halo = (Behaviour)meeple.gameObject.GetComponent("Halo");
						halo.enabled = false;
					}
				}
			}
		}
		
		if(choosing)
		{
//			//Move the highlighter to the mouse
//			Vector3 pos = Input.mousePosition;
//			pos.z = 14f;
//			sweepHighlighter.transform.position = Camera.main.ScreenToWorldPoint(pos);
			
			//Disable other scripts affected by clicking
			mouseClickScript.enabled = false;
			cameraControlScript.enabled = false;
			
			//If mouse hovers over a meeple, show highlighter around it.
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.transform.tag == "Person")
				{
					if(!sweepHighlighter.GetComponent<MeshRenderer>().enabled)
					{
						sweepHighlighter.GetComponent<MeshRenderer>().enabled = true;
					}
					sweepHighlighter.transform.position = hit.collider.gameObject.transform.position;
				}
			}
			
			//Once the player has clicked on an area,
			if(Input.GetMouseButtonDown(0) && !e.IsPointerOverGameObject())
			{
				DoSweep();
			}
		}
	}
	
	public void ToggleSweep() 
	{
		if(!choosing)
		{
			AttemptSweep();
		}
		else
		{
//			Debug.Log ("Don't sweep!");
			choosing = false;
			sweepHighlighter.GetComponent<MeshRenderer>().enabled = false;
			gameObject.GetComponentInChildren<Text>().text = "Susceptible Sweep";
			gameObject.GetComponent<Button>().image.color = Color.white;
			gameObject.GetComponentInChildren<Text>().color = defaultTextColor;
			//Enable other scripts affected by clicking
			mouseClickScript.enabled = true;
			cameraControlScript.enabled = true;
		}
	}
	
	public void AttemptSweep () 
	{
		float currentTime = Time.time;
		if(currentTime - lastSweep >= cooldown)
		{
			//Check if player has enough money
			if(GameObject.Find("Main Camera").GetComponent<PlayerInfo>().BEPF >= sweepCost)
			{
				//If player has enough money, show map highlighter + allow them to click on an area
//				sweepHighlighter.GetComponent<MeshRenderer>().enabled = true;
				choosing = true;
				//Change button text & color so that players know they can cancel
				gameObject.GetComponent<Button>().image.color = Color.red;
			}
			else
			{
				//Otherwise, pop up notification
				NotificationMessage("Not enough money!");
			}
		}
		else
		{
			NotificationMessage("Please wait...");
		}
	}
	
	public void DoSweep()
	{
		//Scan the meeples
		//What is colliding with the highlighter cylinder?
		meeplesToScan = Physics.OverlapSphere(sweepHighlighter.transform.position, sweepHighlighter.GetComponent<Collider>().bounds.extents.x);
		int count = 0;
		foreach (Collider meeple in meeplesToScan)
		{
			if(meeple.gameObject.tag == "Person")
			{
				//Turn on halo
				Behaviour halo = (Behaviour)meeple.gameObject.GetComponent("Halo");
				halo.enabled = true;
				
				//If susceptible,
				if(meeple.gameObject.GetComponent<Profile>().infected == false 
				   && meeple.gameObject.GetComponent<Profile>().immune == false)
				{
					count++;
				}
			}
		}
		
		//Set halo display start time
		if(meeplesToScan.Length > 0)
		{
			haloLastDisplayed = Time.time;
		}
		//Display count
		NotificationMessage(count.ToString() + " susceptible");
		//Subtract money
		GameObject.Find("Main Camera").GetComponent<PlayerInfo>().BEPF -= sweepCost;
		//Keep track of last time of successful sweep
		lastSweep = Time.time;
		//Get rid of highlighter
		choosing = false;
		sweepHighlighter.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<Button>().image.color = Color.white;
		//Enable other scripts affected by clicking
		mouseClickScript.enabled = true;
		cameraControlScript.enabled = true;
	}
	
	public void NotificationMessage(string s)
	{
		sweepPanel.SetActive(true);
		sweepPanel.GetComponentInChildren<Text>().text = s;
		lastDisplayed = Time.time;
	}
}
