using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class WinScreen : MonoBehaviour 
{
	Text msg;
	Image background;

	void Awake()
	{
		msg = GameObject.Find("GameOverMsg").GetComponent<Text>();
		background = GameObject.Find ("Background").GetComponent<Image>();
		if(PlayerPrefs.HasKey("WinFlag"))
		{
			switch(PlayerPrefs.GetInt("WinFlag"))
			{
			//Lose
			case 0:
				msg.text = "SORRY YOU LOSE!";
				background.sprite = Resources.Load<Sprite>("Sprites/TempNickLoseScreen");
				break;
			//Win
			case 1:
				msg.text = "YOU WIN!";
				background.sprite = Resources.Load<Sprite>("Sprites/TempNickWinScreen");
				break;
			}
		}
		else 
		{
			//Error
			Debug.Log ("Something went wrong");
		}

	}

	public void Exit()
	{
		Application.LoadLevel("MainMenu");
	}
}
