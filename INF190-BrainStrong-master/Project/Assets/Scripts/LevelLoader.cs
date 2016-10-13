using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

public class LevelLoader : MonoBehaviour 
{
	private string filepath;
	private XmlDocument xmlDoc;
	
	private Dictionary<int, Level> levels;
	
	// Use this for initialization
	void Awake () 
	{
		levels = new Dictionary<int, Level> ();
		LoadXML();
	}
	
	public Level GetLevel()
	{
		Level test;
		if (levels.TryGetValue (1, out test))
			return test;
		return new Level (); //Throw exception 
	}
	
	void LoadXML()
	{
		filepath = Application.streamingAssetsPath + @"/XML/Levels.xml";
		//Debug.Log("Loading XML for levels");
		if (File.Exists (filepath)) {
			//Debug.Log("Foudn XML file");
			xmlDoc = new XmlDocument ();
			try {
				xmlDoc.Load (filepath);
			} catch (FileNotFoundException) {
				Debug.Log ("The file for loading the XML was not found");
				return;
			}
			
			XmlNodeList currentNode = xmlDoc.GetElementsByTagName ("Level");
			
			foreach(XmlNode n in currentNode)
			{
				Level l = new Level();
				//l.name = n.Attributes["name"].Value;
				l.maxPopulation = XmlConvert.ToInt16(n.Attributes["maxPopulation"].Value);
				//l.spawnRate = (float)XmlConvert.ToDouble(n.Attributes["spawnRate"].Value);
				l.infectionRate = XmlConvert.ToInt16(n.Attributes["infectionRate"].Value);
				l.infectionThreshold = XmlConvert.ToInt16(n.Attributes["infectionThreshold"].Value);
				l.maxImmunityStr = XmlConvert.ToInt16(n.Attributes["maxImmunityStr"].Value);
				int temp = XmlConvert.ToInt16(n.Attributes["levelNum"].Value);
				levels.Add(temp, l);
				//Debug.Log("Name: " + l.name + " secPerDay: " + l.secPerDay.ToString());
			}
		}
	}
}

public struct Level
{
	public string name;
	public int maxPopulation;
	public float spawnRate;
	public int infectionRate;
	public int infectionThreshold;
	public int maxImmunityStr;
}
