using UnityEngine;
using System.Collections;

public class WaypointWalk: MonoBehaviour 
{
	public float timeTillChange;
	public Vector2 currentDirection;
	public int lowerTimeBound, upperTimeBound;
	
	private GameObject[] waypoints;
	public GameObject targetWaypoint;
	public float speed; 
	
	// Use this for initialization
	void Start () 
	{
		//Set up waypoints array
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		
		//Choose random waypoint
		int randomIndex = Random.Range(0, waypoints.Length - 1);
		targetWaypoint = waypoints[randomIndex];
		
		//Set default values
		lowerTimeBound = 3;
		upperTimeBound = 5;
		speed = 1.0f;
		
		//Set up timeTillChange for when NPC will randomly go to another waypoint
		timeTillChange = Random.Range(lowerTimeBound, upperTimeBound);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//While waypoint has not been reached, move to that waypoint.
		if(Vector3.Distance(transform.position, targetWaypoint.transform.position) > 0.05f)
		{ 
			transform.LookAt(targetWaypoint.transform);
			//Vector3.MoveTowards(transform.position, targetWaypoint.transform.position, Time.deltaTime * speed);
			transform.position += transform.forward * Time.deltaTime;
		}
		else	
		{	//When waypoint has been reached, choose next waypoint, 
			//wait until timeTillChange to start moving to next waypoint
			timeTillChange -= Time.deltaTime;
			if (timeTillChange <= 0)
			{
				//Choose random waypoint
				int randomIndex = Random.Range(0, waypoints.Length - 1);
				targetWaypoint = waypoints[randomIndex];
				timeTillChange = Random.Range(lowerTimeBound, upperTimeBound);
			}
		}
	}
}		