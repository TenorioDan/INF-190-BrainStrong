using UnityEngine;
using System.Collections;
using System.Xml;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour 
{
	public int money;
	public int BEPF; //Bloody Eye Protein Fragments
	public int PR;
	//public int nanoMachines;
	//public int nanoVaccines;
	
	private string filepath;
	private XmlDocument xmlDoc;
	private Text moneyLbl, BEPFLbl, PRLbl, nanoMLbl, nanoVLbl;
	
	
	// Use this for initialization
	void Start () 
	{
		LoadXML();
		//moneyLbl = GameObject.Find ("MoneyLabel").GetComponent<Text>();
		BEPFLbl = GameObject.Find ("BEPFLabel").GetComponent<Text>(); 
		//PRLbl = GameObject.Find ("PRLabel").GetComponent<Text>(); 
		//nanoMLbl = GameObject.Find ("NanomachinesLabel").GetComponent<Text>();
		//moneyLbl.text = "$: " + money.ToString();
		if(BEPF < 10) {
			BEPFLbl.text = "0" + BEPF.ToString();
		}
		else {
			BEPFLbl.text = BEPF.ToString();
		}
		//PRLbl.text = "PR: " + PR.ToString();
		//nanoMLbl.text = "Nano Machines: " + nanoMachines.ToString();
		//nanoVLbl= GameObject.Find ("Nano").GetComponent<Text>();
	}
	
	private void LoadXML()
	{
		filepath = Application.streamingAssetsPath + @"/XML/PlayerInfo.xml";
		if(File.Exists (filepath))
		{
			xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(filepath);
			}
			catch (FileNotFoundException)
			{
				Debug.Log("The file for loading the XML was not found");
				return;
			}
			
			XmlNodeList currentNode = xmlDoc.GetElementsByTagName("PlayerEcon");
			
			//money = XmlConvert.ToInt16(currentNode.Item(0).Attributes["money"].Value);
			BEPF = XmlConvert.ToInt16(currentNode.Item(0).Attributes["BEPF"].Value);
			//PR = XmlConvert.ToInt16(currentNode.Item(0).Attributes["PR"].Value);
			//nanoMachines = XmlConvert.ToInt16(currentNode.Item(0).Attributes["nanoMach"].Value);
			//nanoVaccines = XmlConvert.ToInt16(currentNode.Item(0).Attributes["nanoVac"].Value);
		}
	}
	
	void OnGUI()
	{
		//moneyLbl.text = "$: " + money.ToString();
		if(BEPF < 10) {
			BEPFLbl.text = "0" + BEPF.ToString();
		}
		else {
			BEPFLbl.text = BEPF.ToString();
		}
		//PRLbl.text = "PR: " + PR.ToString();
		//nanoMLbl.text = "Nano Machines: " + nanoMachines.ToString();
	}
	
	void OnDestroy() 
	{
		if(xmlDoc != null)
		{
			XmlNodeList currentNode = xmlDoc.GetElementsByTagName("PlayerEcon");
			//currentNode.Item(0).Attributes["money"].Value = money.ToString();
			currentNode.Item(0).Attributes["BEPF"].Value = BEPF.ToString();
			xmlDoc.Save(filepath);
		}
	}
	
	void OnApplicationQuit()
	{
		if(xmlDoc != null)
		{
			XmlNodeList currentNode = xmlDoc.GetElementsByTagName("PlayerEcon");
			//currentNode.Item(0).Attributes["money"].Value = money.ToString();
			currentNode.Item(0).Attributes["BEPF"].Value = BEPF.ToString();
			
			xmlDoc.Save(filepath);
		}
	}
}