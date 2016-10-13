using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spark : MonoBehaviour {

	public string user_name;
	public string message;
	public Text text_box;
	public GameObject meeple;

	private bool firstClick;
	private float timeStart;
	private float timeClickMax = 0.5f;

	private GameObject cam;


	// Use this for initialization
	void Start () {
		user_name = "Name";
		message = "";
		firstClick = false;
		timeStart = 0f;
		cam = GameObject.Find("Main Camera");
		GetComponent<Button>().onClick.AddListener(() => SparkClicked());
	}
	
	// Update is called once per frame
	void Update () {
	}

	void SparkClicked() {
		cam.GetComponent<MouseClick>().UpdateProfile(meeple.transform);
	}

	public void SetMessage(string m) {
		message = m;
		text_box.horizontalOverflow = HorizontalWrapMode.Wrap;
		text_box.verticalOverflow = VerticalWrapMode.Overflow;
		text_box.text = message;

	}
}
