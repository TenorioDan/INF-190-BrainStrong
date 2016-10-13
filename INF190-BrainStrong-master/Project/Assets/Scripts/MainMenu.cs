using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
	public GameObject bg; // inside the body background
	public Image fg;
	public float fadeTime;

	void Start()
	{
		GameObject[] meeple = GameObject.FindGameObjectsWithTag ("Person");
		foreach (GameObject obj in meeple) {
			GameObject.Destroy(obj);
		}

	}

	public void ToInstr() {
		Application.LoadLevel("Instructions");
	}

	public void ToMenu() {
		Application.LoadLevel("MainMenu");
	}

	public void PlayGame()
	{
		StartCoroutine(LevelTransition());
	}

	IEnumerator LevelTransition() {
		fg.gameObject.SetActive(true);
		float totalTime = 0f;
		float startTime = Time.time;
		float alpha = 0f; // alpha
		while(totalTime <= fadeTime) {
			totalTime = Time.time - startTime;
			Vector3 tempScale =	bg.transform.localScale;
			tempScale.x += 2;
			tempScale.y += 2;
			bg.transform.localScale = tempScale;
			alpha = Mathf.Clamp01(totalTime/fadeTime);
			fg.color = new Color(0f, 0f, 0f, alpha);
			yield return new WaitForEndOfFrame();
		}
		Destroy(GameObject.Find("Music"));
		Application.LoadLevel ("NavMesh Test");
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
