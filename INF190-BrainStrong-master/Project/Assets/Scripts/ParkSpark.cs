using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

public class ParkSpark : MonoBehaviour {

	private string filepath;
	private XmlDocument xmlDoc;

	private float sparkInterval;
	private List<string> r_spark_texts; // Random spark text list
	private List<string> s_spark_texts; // Storyline spark text list
	private List<string> b_spark_texts; // Bloody eye infected spark text list
	private int sparkCount;
	private List<GameObject> sparkList;
	public int sparkMax;


	public GameObject feed;

	private float timer = 0.0f;
	
	void Awake()
	{
		r_spark_texts = new List<string>();
		s_spark_texts = new List<string>();
		b_spark_texts = new List<string>();
		sparkInterval = Random.Range(5f, 7f);
		sparkList = new List<GameObject>();
		LoadXML();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		if(timer > sparkInterval)
		{
			GameObject instance = Instantiate(Resources.Load("Spark", typeof(GameObject))) as GameObject;
			Vector3 originalPos = instance.transform.localPosition;
			RectTransform instanceRect = (RectTransform) instance.transform;


			// Places the new spark at the top of the feed and moves the rest down
			float height = instanceRect.sizeDelta.y;
			instanceRect.SetParent(transform.Find("Sparks").transform, true);
			instanceRect.SetAsFirstSibling();
			if(sparkCount >= sparkMax) {
				Destroy(sparkList[0]);
				sparkList.RemoveAt(0);
				OffsetFeed(-25);
			}

			// Resets position and size of spark
			instanceRect.localPosition = new Vector3(originalPos.x, originalPos.y, 0);
			instanceRect.localScale = new Vector3(1f, 1f, 1f);
			instanceRect.localRotation = Quaternion.identity;

			OffsetFeed(height + 25); 

			// Gives the Spark a meeple and message
			instance.GetComponent<Spark>().meeple = GetRandMeeple();
			string message = "";
			if (instance.GetComponent<Spark>().meeple.GetComponent<Profile>() != null) {
				message = instance.GetComponent<Spark>().meeple.GetComponent<Profile>().name + ": ";
				if(instance.GetComponent<Spark>().meeple.GetComponent<Profile>().infected) {
					message += b_spark_texts[Random.Range(0, b_spark_texts.Count)];
				}
				else {
					message += r_spark_texts[Random.Range(0, r_spark_texts.Count)];
				}
			}
			else {
				message += r_spark_texts[Random.Range(0, r_spark_texts.Count)];
			}
			instanceRect.GetComponent<Spark>().SetMessage(message); 


			sparkList.Add(instance);
			sparkCount++;
			timer = 0f;
			sparkInterval = Random.Range(2f, 3f);
		}
	}

	public void Reset()
	{
		for(int i = 0; i < sparkList.Count; i++) {
			Destroy(sparkList[i]);
			OffsetFeed(-25);
		}
		sparkList = new List<GameObject>(); 
		timer = 0;
		sparkInterval = Random.Range(2f, 3f);
		sparkCount = 0;
	}

	private GameObject GetRandMeeple() {
		GameObject[] people = GameObject.FindGameObjectsWithTag("Person");
		return people[Random.Range(0, people.Length)];
	}

	private void OffsetFeed(float height) {
		Vector3 originalPos = feed.transform.localPosition;
		feed.transform.localPosition = new Vector3(originalPos.x, originalPos.y - height, originalPos.z);
	}

	private void LoadXML()
	{
		
		filepath = Application.streamingAssetsPath + @"/XML/Tweets.xml";
		//Debug.Log("Loading XML for levels");
		if (File.Exists (filepath)) 
		{
			//Debug.Log("Foun XML file");
			xmlDoc = new XmlDocument ();
			try {
				xmlDoc.Load (filepath);
			} catch (FileNotFoundException) {
				Debug.Log ("The file for loading the XML was not found");
				return;
			}


			List<string> element_names = new List<string>() {"r_spark", "s_spark", "b_spark"};

			// Loops through each Spark type in the xml file
			for(int i = 0; i < element_names.Count; i++) {

				XmlNodeList sparks = xmlDoc.GetElementsByTagName(element_names[i]);

				// Adds each Spark text to the appropriate list
				foreach(XmlNode spark in sparks) {
					if(element_names[i] == "r_spark") {
						r_spark_texts.Add(spark.InnerText);
					}
					else if(element_names[i] == "s_spark") {
						// TO DO
					}
					else if(element_names[i] == "b_spark") {
						b_spark_texts.Add(spark.InnerText);
					}
				}
			}
		}
	}
}