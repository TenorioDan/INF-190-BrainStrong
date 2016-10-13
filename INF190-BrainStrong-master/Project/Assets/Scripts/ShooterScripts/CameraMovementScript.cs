using UnityEngine;
using System.Collections;

public class CameraMovementScript : MonoBehaviour {

	public GameObject player;
	private Vector3 cameraPosition;
	private float distanceToMoveCamera = 5.0f;
	private float distanceToMoveCamera_Z = 5.0f;
	private float cameraSpeed = 0.0725f;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		Vector3 cameraTranslation = new Vector3(0, 0, 0);

		if (player.transform.position.x - transform.position.x > distanceToMoveCamera) {
			cameraTranslation.x = cameraSpeed;
		} 
		else if (player.transform.position.x - transform.position.x < -distanceToMoveCamera) {
			cameraTranslation.x = -cameraSpeed;
		}

		if (player.transform.position.z - transform.position.z > distanceToMoveCamera_Z) {
			cameraTranslation.y = cameraSpeed;
		} 
		else if (player.transform.position.z - transform.position.z < -distanceToMoveCamera_Z) {
			cameraTranslation.y = -cameraSpeed;
		}

		this.transform.Translate (cameraTranslation);


	}
}
