using UnityEngine;
using System.Collections;

public class LaserBlastScript : MonoBehaviour {

	public float movementSpeed = 75.0f;
	public float destroyWaitTime = 2.0f;
	public float damage = 12.5f;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, destroyWaitTime);
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 perpedicular = Vector3.Cross (transform.forward, transform.up);
		transform.RotateAround(GameManager.sphereCenter, -perpedicular, movementSpeed * Time.deltaTime);
	
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Enemy") {
			col.gameObject.GetComponent<BasicEnemyBehavior>().TakeDamage(damage);
			Destroy(this.gameObject);
		}
	}
}
