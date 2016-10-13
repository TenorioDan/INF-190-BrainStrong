using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResearchMenuButton : MonoBehaviour {
    public GameObject ResearchPanel;
	// Use this for initialization
	public void ToggleResearch()
    {
        if( ResearchPanel.activeSelf )
        {
            ResearchPanel.SetActive( false );
        }
        else
        {
            ResearchPanel.SetActive( true );
        }
    }
}
