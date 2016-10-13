using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	private float originalPos;
	
	private GameObject ground;
	private float groundMaxX;
	private float groundMaxZ;
	private float groundMinX;
	private float groundMinZ;

	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
		originalPos = cam.orthographicSize;

		//Since ground is on xz plane, get z bounds instead of y bounds.
		ground = GameObject.FindGameObjectWithTag("Ground");
		
		groundMaxX = ground.transform.position.x + ground.GetComponent<Renderer>().bounds.extents.x - 2; // -2 is for a little fine-tuning max x because of map position
		groundMaxZ = ground.transform.position.z + ground.GetComponent<Renderer>().bounds.extents.z - 4; // ^^see above
		groundMinX = ground.transform.position.x - 2f; // added -2 because of reason above
		groundMinZ = ground.transform.position.z;
	}
	
	// Update is called once per frame
	void OnGUI () {
		//If the user is double-clicking,
		if(Event.current.type == EventType.ScrollWheel)
		{
			if(Event.current.delta.y < 0)
			{
				if(cam.orthographicSize - .6f >= originalPos - 5f) {
					cam.orthographicSize -= .6f;
					SetPosition(new Vector2(0f, 0f)); //make sure camera never goes beyond map border
				}
			}
			else
			{
				if(cam.orthographicSize + .6f <= originalPos) {
					cam.orthographicSize += .6f;
					SetPosition(new Vector2(0f, 0f)); //make sure camera never goes beyond map border
				}
			}
		}
		else if(Event.current.type == EventType.MouseDrag)
		{
				Vector2 deltaPos = Event.current.delta / 50; //Lessen the amount the camera gets moved
				SetPosition(deltaPos);
		}
	}

	void SetPosition(Vector2 deltaPos) {
		float screenWidth = cam.orthographicSize * 2 - cam.aspect;
		float screenHeight =  cam.orthographicSize * 2;
		transform.position = new Vector3(Mathf.Clamp(transform.position.x - deltaPos.x, groundMinX - (groundMaxX - screenWidth), groundMaxX - screenWidth/2),
			transform.position.y, Mathf.Clamp(transform.position.z + deltaPos.y, groundMinZ - (groundMaxZ - screenHeight)/2, groundMinZ + (groundMaxZ - screenHeight)/2));
	}
}
