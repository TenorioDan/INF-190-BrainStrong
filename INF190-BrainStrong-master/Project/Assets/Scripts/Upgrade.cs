using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.Text;

public class Upgrade : MonoBehaviour 
{

	public GameObject desc;
	public Text name, cost;
	private int idx;

	void Awake()
	{
	}

	public void Init(UpgradeStruct obj)
	{
		name.GetComponent<Text> ().text = obj.name;
		cost.GetComponent<Text> ().text = obj.cost.ToString();
		if(obj.unlocked)
		{
			gameObject.GetComponent<Button>().interactable = false;
			cost.GetComponent<Text> ().text = "";
		}
		desc.GetComponent<UpgradeDesc> ().SetTitle (obj.name);
		desc.GetComponent<UpgradeDesc> ().SetDescription (obj.desc);
		idx = obj.idx;
	
		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + obj.enableImg);
		Sprite temp = Resources.Load<Sprite>("Sprites/" + obj.disableImg);
		SpriteState st = new SpriteState();
		st.disabledSprite = temp;
		gameObject.GetComponent<Button> ().spriteState = st;
		desc.SetActive (false);
	}

	public void ShowDescUI()
	{
		desc.SetActive(true);
	}

	public void HideDescUI()
	{
		desc.SetActive (false);
	}

	public void Buy()
	{
		UpgradeScreen screenManagaer = GameObject.Find("Main Camera").GetComponent<UpgradeScreen>();
		if(screenManagaer.protienFrag >= int.Parse(cost.text))
		{
			screenManagaer.protienFrag -= int.Parse(cost.text);
			screenManagaer.SaveUpgrade(idx);
			gameObject.GetComponent<Button>().interactable = false;
			cost.GetComponent<Text> ().text = "";
		}
	}
}
