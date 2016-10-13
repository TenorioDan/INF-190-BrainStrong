using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if(GameObject.FindGameObjectsWithTag("Music").Length > 1) {
			Debug.Log(GameObject.FindGameObjectsWithTag("Music").Length);
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
