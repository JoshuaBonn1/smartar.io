﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

	public List<GameObject> visibleObjects;
	public float sightRange;
	public RaycastHit2D[] hits;
	public float[] inputs;

	// Use this for initialization
	void Start () {
		inputs = new float[360];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//int counter = 0;
		for (int counter = 0; counter < 360; counter++) {
		//for (float angle = 0; angle < 2 * Mathf.PI; angle+=Mathf.PI/180.0f) {
			// Convert angle to vector
			float angle = (float)counter * Mathf.PI / 180.0f;
			Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

			//TODO: Switch to Physics2D.Raycast and ignore myself (if possible)
			//IDEA: Only test for lines outside of self. Use a different mode to check for consumption
			//IDEA2: If colliding with something, try to overlap myself with their circle. If I am within it, send message
			RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, direction, sightRange);
			if (hits.Length == 1)
				inputs [counter] = 0;
			else {
				inputs [counter] = 1;
				//print (angle);
				//gameObject.GetComponent<Bot> ().setRotation (counter);

				Debug.DrawRay (transform.position, hits[1].collider.gameObject.transform.position - transform.position, Color.black);
				//break;
			}
			counter++;
		}
	}

	void OnTriggerEnter(Collider other) {
		print ("bla");
	}

	void OnTriggerExit(Collider other) {
		print ("bla2");
	}

}