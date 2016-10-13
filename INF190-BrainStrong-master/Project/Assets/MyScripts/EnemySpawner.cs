using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject basicEnemy;
	public float timeToSpawn = 5.0f;
	public float previousTime;
	public float currentTime;
	private float spawnRadius = 38.75f;

	public static Vector3 sphereCenter;

	public float timer = 2;

	GameObject[] meeples;

	void Awake()
	{
		meeples = GameObject.FindGameObjectsWithTag ("Person");
		foreach(GameObject meeple in meeples)
		{
			meeple.SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
		sphereCenter = this.transform.position;
		currentTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - currentTime > timeToSpawn) {
			currentTime = Time.time;			
			SpawnEnemy ();
		}
		timer -= Time.deltaTime;
		if (timer < 0) {
			Application.LoadLevel("NavMesh Test");

			foreach(GameObject meeple in meeples)
			{
				meeple.SetActive(true);
				meeple.GetComponent<NavMeshWalk>().init();
			}
		}

	}

	void SpawnEnemy(){
		Vector3 pointToSpawn = Random.onUnitSphere * spawnRadius;
		Vector3 normal = ( pointToSpawn - sphereCenter ).normalized;

		GameObject spawned = (GameObject) GameObject.Instantiate (basicEnemy, pointToSpawn, new Quaternion());
		spawned.transform.up = normal;
	}

}
