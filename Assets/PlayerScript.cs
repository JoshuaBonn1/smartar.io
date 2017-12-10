using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Raycast out to hit enemy
		RaycastHit2D[] hit = Physics2D.RaycastAll (
			                      transform.position, Vector2.right, 100000000000.0f);
		Debug.Log (hit[0], this);
		//Debug.Log(hit.collider.gameObject.name);

		Debug.DrawRay (transform.position, Vector2.right * 100.0f);
	}
}
