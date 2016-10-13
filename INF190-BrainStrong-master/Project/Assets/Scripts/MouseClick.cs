using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour {
	
	public GameObject ui;
	
	private Transform selected;
	
	private GameObject infiltrateMsg;
	private float secsToDisplay = 2f;
	private float lastDisplayed = 0f;

	private Color previousColor;
	private int previousZ;

	
	// Use this for initialization
	void Start () 
	{
		infiltrateMsg = GameObject.Find("InfiltrateMessage");
		infiltrateMsg.SetActive(false);
		ui = GameObject.Find("PersonClickCanvas");
		for (int i = 1; i <= gameObject.GetComponent<StratLayer>().symptonList.Count; i++)
		{
			string s = "Symptom" + i.ToString();
			ui.transform.Find(s).FindChild("Label").GetComponent<Text>().text = 
				gameObject.GetComponent<StratLayer>().symptonList[i-1].name;
		}
		ui.SetActive(false);
		selected = null;
		previousColor = Color.white;
		
		
	}
	/*
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "MeepleCollider")
		{
			UpdateProfile(other.transform);
			selected.GetComponent<Profile>().beingInvaded = true;
		}
		else
		{
			if(selected != null) 
			{
				selected.GetComponent<Profile>().beingInvaded = false;
				selected = null;
			}
			ui.SetActive(false);
		}
	}
	*/
	
	// Update is called once per frame
	void Update () 
	{
		if(infiltrateMsg.activeSelf == true)
		{
			float currentTime = Time.time;
			if(currentTime - lastDisplayed > secsToDisplay)
			{
				infiltrateMsg.SetActive(false);
			}
		}
		
		if (Input.GetMouseButtonDown(0))
		{
			if(EventSystem.current.IsPointerOverGameObject())
			{
				//Had to disable Camera Control so that player could use slider without moving camera
				GameObject.Find("Main Camera").GetComponent<CameraControl>().enabled = false;
				return;
			}
			GameObject.Find("Main Camera").GetComponent<CameraControl>().enabled = true;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			RaycastHit[] hits = Physics.RaycastAll(ray);
			
//			Debug.DrawRay(ray.origin, ray.direction*1000);
			
			if(hits.Length != 0)
			{
				//RaycastHit hit = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				// if(hit.collider != null)
				//{
				bool hitMeeple = false;
				for(int i = 0; i < hits.Length; i++) {
					if(hits[i].collider != null && hits[i].collider.gameObject.transform.tag == "MeepleCollider") {
						if(selected != null) {
							ResetMeepleColor();
						}
						UpdateProfile(hits[i].transform);
						previousColor = selected.GetComponent<Renderer>().material.color;
						previousZ = selected.GetComponent<Renderer>().material.renderQueue;
						selected.GetComponent<Renderer>().material.renderQueue = 4000;
						hits[i].transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
						hitMeeple = true;
						break;
					}
				}

				if (!hitMeeple)
				{
					if(selected != null) {
						selected.GetComponent<Profile>().beingInvaded = false;
						ResetMeepleColor();
						selected = null;
					}
					ui.SetActive(false);
				}				
			}
			else
			{
				ui.SetActive(false);
				if(selected != null) 
				{
					selected.GetComponent<Profile>().beingInvaded = false;
					ResetMeepleColor();
					selected = null;
				}
			}
		}
	}

	public void ResetMeepleColor() {
		if(selected != null) 
		{
			selected.GetComponent<Renderer>().material.color = previousColor;
			selected.GetComponent<Renderer>().material.renderQueue = previousZ;
		}
	}

	public void Invading() {
		if(selected != null) 
		{
			selected.GetComponent<Profile>().beingInvaded = true;
		}
	}
	
	public void UpdateProfile(Transform meeple) {
		ui.SetActive(true);
		selected = meeple;
		ui.transform.Find("Name").GetComponent<Text>().text = selected.GetComponent<Profile>().name;
		ui.transform.Find ("ProfilePicture").GetComponent<Image>().color = meeple.gameObject.GetComponent<MeshRenderer>().material.color;
		int temp = selected.GetComponent<Profile>().infectionStage;
		/*
		if(selected.gameObject.GetComponent<Profile>().immune)
		{
			ui.transform.Find("Button").gameObject.SetActive(false);
			for (int i = 1; i <= gameObject.GetComponent<StratLayer>().symptonList.Count; i++)
			{
				ui.transform.Find("Symptom" + i.ToString()).FindChild("Background").FindChild("Checkmark").GetComponent<Image>().enabled = false;
				ui.transform.Find("Symptom" + i.ToString()).FindChild("Background").GetComponent<Image>().enabled = false;
				ui.transform.Find("Symptom" + i.ToString()).FindChild("Label").GetComponent<Text>().enabled = false;
			}
		}
		*/
		/*
		else
		{
			*/
			for (int i = 1; i <= gameObject.GetComponent<StratLayer>().symptonList.Count; i++)
			{
				if(i <= temp+1)
					ui.transform.Find("Symptom" + i.ToString()).FindChild("Background").FindChild("Checkmark").GetComponent<Image>().enabled = true;
				else
					ui.transform.Find("Symptom" + i.ToString()).FindChild("Background").FindChild("Checkmark").GetComponent<Image>().enabled = false;
			}
			/*
		}				
		*/
		ui.transform.Find("Health").GetComponent<Text>().text = selected.GetComponent<Profile>().immunityStr.ToString();
		
	}
	
	public void CheckMeepleInfection()
	{
		if((!selected.GetComponent<Profile>().infected) && (!selected.GetComponent<Profile>().immune))
		{
			selected.GetComponent<Profile>().beingInvaded = true;
			GameObject.Find("Main Camera").GetComponent<StratLayer>().ChangeToInner();
		}
		else
		{
			//Let player know they messed up + make it obvious that the park is speeding up.
			infiltrateMsg.SetActive(true);
			lastDisplayed = Time.time;
			GameObject.Find("Main Camera").GetComponent<StratLayer>().timeToSpeed = Time.time;
			Time.timeScale = 3;
		}
	}
	
	public void SetMeepleColor()
	{
		int value = (int)ui.transform.Find("MeepleColorSlider").GetComponent<Slider>().value;

		if(selected.gameObject.GetComponent<MeshRenderer>().material.color == Color.yellow && !selected.gameObject.GetComponent<Profile>().immune) {
			Material mat = (Material)Resources.Load("meeple", typeof(Material));
			switch(value)
			{
				case 1:
					mat = (Material)Resources.Load("meeple", typeof(Material));
					break;
				case 2:
					mat = (Material)Resources.Load("PossibleSick", typeof(Material));
					break;
				case 3:
					mat = (Material)Resources.Load("DefinitelySick", typeof(Material));
					break;
				case 4:
					mat = (Material)Resources.Load("Immune", typeof(Material));
					break;
			}
			previousColor = mat.color;	
			ui.transform.Find ("ProfilePicture").GetComponent<Image>().color = previousColor;
		}
		else if(!selected.gameObject.GetComponent<Profile>().immune) {
			switch(value)
			{
				case 1:
					selected.gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("meeple", typeof(Material));
					break;
				case 2:
					selected.gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("PossibleSick", typeof(Material));
					break;
				case 3:
					selected.gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("DefinitelySick", typeof(Material));
					break;
				case 4:
					selected.gameObject.GetComponent<Renderer>().material = (Material)Resources.Load("Immune", typeof(Material));
					break;
			}
			ui.transform.Find ("ProfilePicture").GetComponent<Image>().color = selected.gameObject.GetComponent<MeshRenderer>().material.color;
			previousColor = selected.gameObject.GetComponent<MeshRenderer>().material.color;
		}
	}
}
