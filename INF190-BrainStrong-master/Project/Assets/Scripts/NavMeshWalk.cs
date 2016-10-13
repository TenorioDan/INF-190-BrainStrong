using UnityEngine;
using System.Collections;

public class NavMeshWalk : MonoBehaviour {

	public float timeTillChange;
	public int lowerTimeBound, upperTimeBound;
	
	private GameObject[] waypoints;
	public GameObject targetWaypoint;
	public float speed; 
	
	private NavMeshAgent agent;
	
	// Use this for initialization
	void Start () 
	{
		//Debug.Log ("Beginning of start in Navmesh walk: " + transform.position);
		//Set up waypoints array
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		
		//Choose random waypoint
		int randomIndex = Random.Range(0, waypoints.Length - 1);
		targetWaypoint = waypoints[randomIndex];
		
		//Set default values
		lowerTimeBound = 3;
		upperTimeBound = 5;
		speed = Random.Range(0.3f, 1.0f);
		
		//Set up timeTillChange for when NPC will randomly go to another waypoint
		timeTillChange = Random.Range(lowerTimeBound, upperTimeBound);
		
		//Initialize NavMesh Agent
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		//transform.LookAt(targetWaypoint.transform);
		agent.destination = targetWaypoint.transform.position;
		//Debug.Log ("End of start in Navmesh walk: " + transform.position);
		init ();
	}

	public void init()
	{
		timeTillChange = 0;
	}
	
	private void newWaypoint()
	{
		bool check = false; //Check if there is a path
		while(!check)
		{
			//Choose random waypoint
			int randomIndex = Random.Range(0, waypoints.Length - 1);
			targetWaypoint = waypoints[randomIndex];
			timeTillChange = Random.Range(lowerTimeBound, upperTimeBound);
			//Set the agent to go to it.
			check = agent.SetDestination(targetWaypoint.transform.position);
			speed = Random.Range(0.3f, 1.0f);
		}
	}
	
	// Update is called once per frame
	//Don't need to manually step agent to target; all of that is handled as long as you set
	//agent destination.
	void Update () 
	{	
		if(waypoints.Length > 0)
		{
			if(waypoints[0] == null)
				waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
			//Don't decrement timeTillChange until agent has reached the target
			if(agent.pathStatus==NavMeshPathStatus.PathComplete && agent.remainingDistance==0)	
			{	
				timeTillChange -= Time.deltaTime;
				if (timeTillChange <= 0)
				{
					newWaypoint();
				}
			}
			
			//If agent's path is blocked by a gate, automatically assign a new waypoint
			/*
			if(!agent.hasPath)
			{
				Vector3 oldDestination = agent.destination;
				newWaypoint();
				int i = 0;
				while(oldDestination == agent.destination)
				{
					i++;
					newWaypoint();
				}
			}
			*/
		}
	}
}		