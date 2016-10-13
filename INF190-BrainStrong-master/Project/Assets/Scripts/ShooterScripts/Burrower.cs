using UnityEngine;
using System.Collections;

public class Burrower : BasicEnemyBehavior {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void TakeDamage(float damage){

		currentHealth -= damage;
		//print (currentHealth);
		if (currentHealth <= 0) {
			if (!dead){
				GameManager.burrowerCount--;
			}
			dead = true;
			Destroy(this.gameObject);
		}
	}
}
