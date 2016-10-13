using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class UpgradeDesc : MonoBehaviour 
{
	public GameObject m_title, m_desc;

	public void SetTitle(string title)
	{
		m_title.GetComponent<Text>().text = title;
	}

	public void SetDescription(string description)
	{
		m_desc.GetComponent<Text>().text = description;
	}
}
