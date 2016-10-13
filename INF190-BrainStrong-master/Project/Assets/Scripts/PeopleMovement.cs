using UnityEngine;
using System.Collections;

public class PeopleMovement: MonoBehaviour 
{
    public float timeTillChange;
    public Vector2 currentDirection;
    public int lowerTimeBound, upperTimeBound;

	// Use this for initialization
	void Start () 
    {
        
        Random.seed = 0;
		currentDirection = new Vector3(Mathf.Sin(Random.Range(-3, 4)), Mathf.Sin(Random.Range(-3, 4)), 0);
        timeTillChange = Random.Range(lowerTimeBound, upperTimeBound);
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(currentDirection * Time.deltaTime);
        timeTillChange -= Time.deltaTime;
        if (timeTillChange <= 0)
        {
            currentDirection = new Vector3(Mathf.Sin(Random.Range(-3, 4)), Mathf.Sin(Random.Range(-3, 4)), 0);
            timeTillChange = Random.Range(lowerTimeBound, upperTimeBound);
        }
	}
}
