using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class UpgradeScreen : MonoBehaviour 
{
	private string filepath;
	XmlDocument xmlDoc;

	public List<UpgradeStruct> upgradeList;

	GameObject upgradeCanvas;
	public int protienFrag;
	Text fragmentUI;

	GameObject[] meeples;

	void Awake()
	{
		upgradeCanvas = GameObject.Find ("Canvas");
		upgradeList = new List<UpgradeStruct> ();
		LoadXML ();
		for (int idx = 0; idx < upgradeList.Count; idx++) 
		{
			GameObject instance = Instantiate (Resources.Load ("Upgrade", typeof(GameObject))) as GameObject;
			instance.transform.SetParent (upgradeCanvas.transform, false);
			instance.GetComponent<Upgrade> ().Init (upgradeList [idx]);
		}

		meeples = GameObject.FindGameObjectsWithTag ("Person");
		foreach(GameObject meeple in meeples)
		{
			meeple.SetActive(false);
		}
		fragmentUI = GameObject.Find("FragmentUI").GetComponent<Text>();
		if(protienFrag < 10) {
			fragmentUI.text = "0" + protienFrag.ToString();
		}
		else {
			fragmentUI.text = protienFrag.ToString();
		}
	}

	void OnGUI()
	{
		if(protienFrag < 10) {
			fragmentUI.text = "0" + protienFrag.ToString();
		}
		else {
			fragmentUI.text = protienFrag.ToString();
		}
	}

	void OnDestroy() 
	{
		if(xmlDoc != null)
		{
			XmlNodeList currentNode = xmlDoc.GetElementsByTagName("PlayerEcon");
			currentNode.Item(0).Attributes["BEPF"].Value = protienFrag.ToString();
			xmlDoc.Save(filepath);
		}
	}
	
	void OnApplicationQuit()
	{
		if(xmlDoc != null)
		{
			XmlNodeList currentNode = xmlDoc.GetElementsByTagName("PlayerEcon");
			currentNode.Item(0).Attributes["BEPF"].Value = protienFrag.ToString();
			
			xmlDoc.Save(filepath);
		}
	}

	void LoadXML()
	{
		filepath = Application.streamingAssetsPath + @"/XML/PlayerInfo.xml";
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

			XmlNodeList currentNode = xmlDoc.GetElementsByTagName ("PlayerEcon");
			
			protienFrag = XmlConvert.ToInt16(currentNode.Item(0).Attributes["BEPF"].Value);
			if(PlayerPrefs.HasKey("Frags"))
			{
				protienFrag += PlayerPrefs.GetInt("Frags");
				PlayerPrefs.DeleteKey("Frags");
				PlayerPrefs.Save();
			}

			currentNode = xmlDoc.GetElementsByTagName ("Upgrade");
			
			foreach (XmlNode upgrade in currentNode) 
			{
				UpgradeStruct s = new UpgradeStruct();
				s.desc = upgrade.InnerText;
				s.idx = XmlConvert.ToInt32(upgrade.Attributes["indexNum"].Value);
				s.name = upgrade.Attributes["Name"].Value;
				s.enableImg = upgrade.Attributes["enableImage"].Value;
				s.disableImg = upgrade.Attributes["disableImage"].Value;
				s.cost = XmlConvert.ToInt32(upgrade.Attributes["cost"].Value);
				s.unlocked = XmlConvert.ToBoolean(upgrade.Attributes["unlocked"].Value);
				
				upgradeList.Add(s);
			}
		}
	}

	public void SaveUpgrade(int idxNum)
	{
		XmlNodeList currentNode = xmlDoc.GetElementsByTagName ("Upgrade");
		currentNode.Item(idxNum).Attributes["unlocked"].Value = "1";
		xmlDoc.Save(filepath);
	}

	public void Exit()
	{
		foreach(GameObject meeple in meeples)
		{
			meeple.SetActive(true);
			if(meeple.GetComponent<Profile>().beingInvaded)
				meeple.GetComponent<Profile>().SetImmune();
			meeple.GetComponent<NavMeshWalk>().init();
			meeple.GetComponent<Profile>().beingInvaded = false;
		}

		Application.LoadLevel("NavMesh Test");
	}

}

public struct UpgradeStruct
{
	public string name, desc, enableImg, disableImg;
	public int cost, idx;
	public bool unlocked;
}
