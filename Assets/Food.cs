using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
	public GameObject ground;

	void Update() {}

	public void kill () {
		//print ("Food Eaten");
		float width = ground.transform.localScale.x - 1.0f;
		float height = ground.transform.localScale.y - 1.0f;
		transform.position = new Vector3 (
			Random.value * width - width / 2.0f, 
			Random.value * height - height / 2.0f, 
			0);
	}
		
}
