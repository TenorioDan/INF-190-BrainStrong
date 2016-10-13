using UnityEngine;
using System.Collections;

public class ProtienFrag : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if(other.GetComponent<CharacterInputHandler>().PickUpFragment())
				GameObject.Destroy(this.gameObject);
		}
	}
}
