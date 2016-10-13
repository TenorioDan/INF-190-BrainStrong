using UnityEngine;
using System.Collections;

public class BasicEnemyBehavior : MonoBehaviour {

	public float maxHealth = 100.0f;
	public float currentHealth = 100.0f;
	public float movementSpeed = 0.20f;
	private GameObject player;
	protected bool dead;
	// Use this for initialization
	void Start () {
		dead = false;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	public virtual void TakeDamage(float damage){
		currentHealth -= damage;
		//print (currentHealth);
		if (currentHealth <= 0 && !dead) {
			dead = true;
			if(Random.value <= .15f) {
				GameObject instance = Instantiate(Resources.Load("ProtienFragment", typeof(GameObject))) as GameObject;
				instance.transform.position = this.transform.position;
			}
			Destroy(this.gameObject);
		}
	}

	private void Move(){
		player = GameObject.Find ("PlayerSphere");
		transform.position = Vector3.Slerp (transform.position, player.transform.position, Time.deltaTime * movementSpeed);
		transform.up = (transform.position - GameManager.sphereCenter).normalized;
		//Vector3 perpedicular = Vector3.Cross (transform.forward, transform.up);
		//transform.RotateAround (Vector3.zero, -perpedicular, movementSpeed * Time.deltaTime);

		//perpedicular = Vector3.Cross (transform.right, transform.up);
		//transform.RotateAround (Vector3.zero, -perpedicular, movementSpeed * Time.deltaTime);
	}


}
