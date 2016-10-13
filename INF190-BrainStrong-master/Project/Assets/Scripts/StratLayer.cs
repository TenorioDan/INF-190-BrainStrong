using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

public class StratLayer : MonoBehaviour 
{
	private string filepath;
	XmlDocument xmlDoc;
	
	public float spawnTimer;
	public int infectionThreshold;
	public List<string> names;
	public List<Symptom> symptonList;
	public int numOfPeople, infectedPeople;
	
	public double secLeftInDay, secInDay;
	public double timeToSpeed;
	
	private float timer = 0.0f;
	private int infectionRate;
	private int tillNextInfect;

	//private int endOfDayMoney;
	private int maxPeople;
	
	//private GameObject collectDailyMoneyButton;
	private GameObject cashLbl, PRLbl, PplLbl;
	
	private int currentDay;
	private bool goingToInner;

	private GameObject[] meeple;
	
	void Awake()
	{
		symptonList = new List<Symptom>();
		names = new List<string>();
		Random.seed = (int)System.DateTime.Now.Ticks;
		LoadXML();
		
		
		cashLbl = GameObject.Find("CashNum");
		PRLbl = GameObject.Find("PRNum"); 
		PplLbl = GameObject.Find("PplNum");
		
		//endOfDayMoney = 0;
		
		if (PlayerPrefs.HasKey ("DayNum"))
			currentDay = PlayerPrefs.GetInt("DayNum");
		else
			currentDay = 1;
		if (PlayerPrefs.HasKey ("NumOfPpl"))
			numOfPeople = PlayerPrefs.GetInt ("NumOfPpl");
		else {
			numOfPeople = 0;
		}
		goingToInner = false;
	}
	
	void Start()
	{
		LoadLevel ();
		//Time.timeScale = 3f;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		
		/*
		//If the day is over, have the user collect the money by clicking on a button
		if(secLeftInDay <= 0)
		{
			//collectDailyMoneyButton.GetComponent<Button>().interactable = true;
			CheckParkStatus();
			secLeftInDay = secInDay;
			Time.timeScale = 0;
		}
		*/
		
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.LoadLevelAsync("MainMenu");
		}
		/*
		GameObject[] people = GameObject.FindGameObjectsWithTag("Person");
		if(people.Length > 0) 
		{
			Debug.Log("Pause");
		}
		*/
		
		if (timeToSpeed > -1 && Time.time - timeToSpeed > 5) {
			timeToSpeed = -1;
			Time.timeScale = 1;
		}
	}
	
	[ExecuteInEditMode]
	void OnDestroy()
	{
		//Delete some of the prefferences if you are actually leaving the game
		if (!goingToInner) {
			PlayerPrefs.DeleteKey ("DayNum");
			PlayerPrefs.DeleteKey ("NumOfPpl");
			PlayerPrefs.DeleteKey("Timer");
		}
		
	}
	
	private void ResetPark()
	{
		numOfPeople = 0;
		GameObject[] people = GameObject.FindGameObjectsWithTag("Person");
		foreach(GameObject person in people)
		{
			GameObject.Destroy(person);
		}
		
		timer = 0;
		GameObject.Find ("SparkFeed").GetComponent<ParkSpark>().Reset();
		Time.timeScale = 1;
		
		//Delete some of the play prefs whena  new day is happening
		PlayerPrefs.DeleteKey("Timer");
		PlayerPrefs.DeleteKey("NumOfPpl");
		PlayerPrefs.SetInt ("DayNum", ++currentDay);
		PlayerPrefs.Save ();
		
		LoadLevel ();
		tillNextInfect = infectionRate;
	}
	
	public void ChangeToInner()
	{
		gameObject.GetComponent<MouseClick>().ResetMeepleColor();
		gameObject.GetComponent<MouseClick>().Invading();
		Application.LoadLevel ("TopDownShooterScene");
		//Save out the current timer for the level we are leaving and the current day that it is
		PlayerPrefs.SetFloat("Timer", (float)secLeftInDay);
		PlayerPrefs.SetInt ("DayNum", currentDay);
		PlayerPrefs.SetInt("NumOfPpl", numOfPeople);
		//PlayerPrefs.SetInt("Infected", infectedPeople);
		PlayerPrefs.Save ();
		goingToInner = true;
	}
	
	public void CollectDailyMoney()
	{
		//gameObject.GetComponent<PlayerInfo>().money += endOfDayMoney;
		if(currentDay < 5)
			ResetPark();
		else {
			ResetGame();
			Application.LoadLevelAsync("MainMenu");
		}
	}
	
	void ResetGame()
	{
		numOfPeople = 0;
		GameObject[] people = GameObject.FindGameObjectsWithTag("Person");
		foreach(GameObject person in people)
		{
			GameObject.Destroy(person);
		}
		
		timer = 0;
		GameObject.Find ("SparkFeed").GetComponent<ParkSpark>().Reset();
		Time.timeScale = 1;
		
		//Delete some of the play prefs whena  new day is happening
		PlayerPrefs.DeleteKey("Timer");
		PlayerPrefs.DeleteKey("NumOfPpl");
		PlayerPrefs.SetInt ("DayNum", 0);
		PlayerPrefs.Save ();
	}
	
	void LoadLevel()
	{
		Level l = gameObject.GetComponent<LevelLoader> ().GetLevel ();
		spawnTimer = l.spawnRate;
		infectionRate = l.infectionRate;
		tillNextInfect = infectionRate;
		if(PlayerPrefs.HasKey("Timer"))
		{
			secInDay = 0;
			secLeftInDay = PlayerPrefs.GetFloat("Timer");
		}
		/*else
		{
			secInDay = l.secPerDay;
			secLeftInDay = secInDay;
		}
		*/
		infectionThreshold = l.infectionThreshold;
		maxPeople = l.maxPopulation;
		
		//timer += Time.deltaTime;
		if (GameObject.FindGameObjectsWithTag ("Person").Length == 0) {
			for (numOfPeople = 0; numOfPeople < maxPeople; numOfPeople++) {
				tillNextInfect--;
				GameObject instance = Instantiate (Resources.Load ("Meeple", typeof(GameObject)), GameObject.FindGameObjectWithTag ("Respawn").transform.position, Quaternion.identity) as GameObject;
//				Debug.Log ("Right after the position is set: " + instance.transform.position);
//				Debug.Log ("Respawn's position: " + GameObject.FindGameObjectWithTag ("Respawn").transform.position);
				instance.transform.GetComponent<Profile> ().personNum = numOfPeople;
				//timer = 0.0f;
				if (tillNextInfect == 0) {
					tillNextInfect = infectionRate;
					instance.GetComponent<Profile> ().SetInfected ();
				}
				//numOfPeople++;
				//Debug.Log ("End of the spawning subroutine: " + instance.transform.position);
			}
		}
		Debug.Log(infectedPeople);
		//secLeftInDay -= Time.deltaTime;
	}
	
	private void LoadXML()
	{
		filepath = Application.streamingAssetsPath + @"/XML/General.xml";
		//Debug.Log("Loading XML for levels");
		if (File.Exists (filepath)) {
			//Debug.Log("Found XML file");
			xmlDoc = new XmlDocument ();
			try {
				xmlDoc.Load (filepath);
			} catch (FileNotFoundException) {
				Debug.Log ("The file for loading the XML was not found");
				return;
			}
			
			XmlNodeList currentNode = xmlDoc.GetElementsByTagName("symptom");
			
			foreach (XmlNode symptom in currentNode)
			{
				Symptom s = new Symptom();
				s.name = symptom.InnerText;
				s.infectStr = XmlConvert.ToInt16(symptom.Attributes["infectStr"].Value);
				s.timeTill = XmlConvert.ToInt16(symptom.Attributes["timeTill"].Value);
				
				symptonList.Add(s);
			}
			
			currentNode = xmlDoc.GetElementsByTagName("name");
			foreach (XmlNode n in currentNode)
			{
				names.Add(n.InnerText);
			}
			
			//currentNode = xmlDoc.GetElementsByTagName("settings");
		}
	}

	public void CheckParkStatus(bool death = false)
	{
		if (death) {
			numOfPeople -= 1;
			infectedPeople -= 1;
		}
		if (maxPeople - numOfPeople >= maxPeople * 0.25) {
			PlayerPrefs.SetInt("WinFlag", 0);
			PlayerPrefs.Save();
			meeple = GameObject.FindGameObjectsWithTag("Person");
			foreach(GameObject obj in meeple)
			{
				GameObject.Destroy(obj);
			}
			Application.LoadLevel("GameOverScene");
			Debug.Log ("Player Lost");
		}
		else if (infectedPeople == 0) {
			PlayerPrefs.SetInt("WinFlag", 1);
			PlayerPrefs.Save();
			meeple = GameObject.FindGameObjectsWithTag("Person");
			foreach(GameObject obj in meeple)
			{
				GameObject.Destroy(obj);
			}
			Application.LoadLevel("GameOverScene");
			Debug.Log("Player Wins");
		}
	}
}


public struct Symptom
{
	public string name;
	public int infectStr;
	public int timeTill;
	
	public void Print()
	{
		Debug.Log("Name: " + name + '\n' + "Infection Strength: " + infectStr.ToString());
	}
}