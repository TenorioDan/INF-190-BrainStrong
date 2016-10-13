using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject drill;
	public GameObject basicEnemy;
	public GameObject burrower;
	public float timeToSpawn = 2.5f;
	public float previousTime;
	public float currentTime;
	public Font timerFont;
	public int TimeLeft = 10;
	public static int burrowerCount = 0;
	public static bool gameStarted = false;
	public static Vector3 sphereCenter;	
//	public static Vector3 sphereCenter;
	private Vector2 hotSpot = Vector2.zero;
	private float inbetweenTime;
	private float spawnRadius = 38.75f;
	private float timeToStart = 3.0f;
	public int numFragments;

	
//
	GameObject[] meeples;
	
	void Awake()
	{
		meeples = GameObject.FindGameObjectsWithTag ("Person");
		foreach(GameObject meeple in meeples)
		{
			meeple.SetActive(false);
		}
		numFragments = 0;
	}

	// Use this for initialization
	void Start () {
		//Cursor.visible = false;
		sphereCenter = this.transform.position;
		currentTime = Time.time;
		inbetweenTime = 0f;

	}

	void OnDestroy()
	{
		PlayerPrefs.SetInt("Frags", numFragments);
		PlayerPrefs.Save();
	}
	
	// Update is called once per frame
	void Update () {
		//change the cursor position to follow the mouse

		if (gameStarted) {


			//print (burrowerCount);
			if (inbetweenTime > 1 && burrowerCount == 0) {
				TimeLeft--;
				inbetweenTime = 0f;
			}

			if (Time.time - currentTime > timeToSpawn) {
				currentTime = Time.time;			
				SpawnEnemy ();
			}

			if (burrowerCount == 0) {
				drill.transform.RotateAround (drill.transform.position, drill.transform.up, 1000.0f * Time.deltaTime);
			}

			if (TimeLeft < 0) {
				gameStarted = false;
				Application.LoadLevel ("UpgradeScene");
			
				foreach (GameObject meeple in meeples) {
					meeple.SetActive (true);
					Debug.Log(meeple.GetComponent<Profile>().beingInvaded);
					if (meeple.GetComponent<Profile> ().beingInvaded) {
						Debug.Log("Setting immune");
						meeple.GetComponent<Profile> ().SetImmune ();
					}
					meeple.GetComponent<NavMeshWalk> ().init ();
					meeple.GetComponent<Profile> ().beingInvaded = false;
				}
			}
			inbetweenTime += Time.deltaTime;
		} 
		else {
			if (Time.time - currentTime >= 1.0f){
				currentTime = Time.time;
				timeToStart--;

				if (timeToStart <= 0){
					gameStarted = true;
					//Cursor.visible = true;
					//Cursor.lockState = CursorLockMode.None;
					//System.Drawing.Point mousePoint = new System.Drawing.Point(Screen.width/2, 250);
					//GameObject player = GameObject.Find ("PlayerSphere");
					//player.GetComponent<CharacterInputHandler>().initializeRotation(mousePoint.X, mousePoint.Y);
				}
			}
		}
	}

	//the gui for drawing the timer, player health and drill health
	void OnGUI(){
		GUIStyle timerStyle = new GUIStyle ();
		timerStyle.font = timerFont;
		timerStyle.fontSize = 40;

		if (gameStarted) {
			GUI.Label (new Rect (Screen.width / 2 - 30, 5, 200, 50), TimeLeft.ToString (), timerStyle);
			} 
		else {
			GUI.Label (new Rect (Screen.width / 2 - 30, 5, 200, 50), timeToStart.ToString (), timerStyle);
			}
	}

	void SpawnEnemy(){
		Vector3 pointToSpawn = Random.onUnitSphere * spawnRadius;
		Vector3 normal = ( pointToSpawn - sphereCenter ).normalized;
		Debug.Log(normal);

		float rand = Random.Range (0.0f, 1.0f);

		GameObject objectToSpawn = basicEnemy;

		if (rand > .10f) {
			if (rand > 0.90f) {
				objectToSpawn = burrower;
				burrowerCount++;
			}
			GameObject spawned = (GameObject) GameObject.Instantiate (objectToSpawn, pointToSpawn, new Quaternion());
			spawned.transform.up = normal;
		}
	}

	
	
	
}
