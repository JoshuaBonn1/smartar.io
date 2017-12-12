using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
	public GameObject ground;

	void Update() {}

	public void kill () {
		float width = ground.transform.localScale.x - 1.0f;
		float height = ground.transform.localScale.y - 1.0f;
		Vector3 newPosition;
		int tries = 5;
		do { 
			newPosition = new Vector3 (
				Random.value * width - width / 2.0f, 
				Random.value * height - height / 2.0f, 
				0);
			tries--;
		} while (Physics2D.OverlapCircle (newPosition, transform.localScale.x) && tries != 0);
		transform.position = newPosition;
	}
		
}
