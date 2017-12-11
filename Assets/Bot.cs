﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {
	float f = 0.0f;
	public float maxSpeed = 1f;

	// Ground to stay in bounds on, area is in the scale of the ground
	public GameObject ground;


	// Use this for initialization
	void Start () {
		f = Random.value * 360.0f;
		setRotation (f);
		gameObject.SetActive (true);
	}

	void Update () {
		checkBotOverlap ();
		if (getSize() < 0.3f)
			kill ();
	}

	// Update is called once every physics update
	void FixedUpdate () {
		// Move in the direction it is facing
		// TODO: Add acceleration and max speeds
		transform.Translate(Vector3.right * maxSpeed * Time.deltaTime);
		stayInBounds ();
		setSize (getSize () - 0.005f);
	}

	private void checkBotOverlap () {
		Collider2D[] potentialBots = Physics2D.OverlapCircleAll (transform.position, getSize());
		foreach (Collider2D bot in potentialBots) {
			if (bot.gameObject == gameObject)
				continue;
			float distance = Vector2.Distance (transform.position, bot.transform.position);
			if (distance + bot.gameObject.transform.localScale.x / 2.0f <= getSize () / 2.0f) {
				bot.SendMessage ("kill");
				setSize (getSize () + 0.1f);
			}
		}
	}

	private void kill () {
		print ("KILLED");
		setPosition (new Vector3 (
			Random.value * 50 - 50 / 2.0f, 
			Random.value * 50 - 50 / 2.0f, 
			0));
		setSize (1.0f);
	}

	// Keeps bot inbounds, happens every fixedUpdate
	private void stayInBounds () {
		float max_x = ground.transform.localScale.x / 2;
		float max_y = ground.transform.localScale.y / 2;
		float radius = getSize () / 2;
		Vector3 newPosition = transform.position;
		if (transform.position.x + radius > max_x) {
			newPosition.Set (max_x - radius, newPosition.y, newPosition.z);
		} else if (transform.position.x - radius < -max_x) {
			newPosition.Set (-max_x + radius, newPosition.y, newPosition.z);
		}
		if (transform.position.y + radius > max_y) {
			newPosition.Set (newPosition.x, max_y - radius, newPosition.z);
		} else if (transform.position.y - radius < -max_y) {
			newPosition.Set (newPosition.x, -max_y + radius, newPosition.z);
		}
		setPosition (newPosition);
	}

	// Get size
	public float getSize () {
		return transform.localScale.x;
	}

	// Set size, always equal sizes on x and y
	public void setSize (float newSize) {
		Vector3 newScale = new Vector3 (newSize, newSize, 1f);
		transform.localScale = newScale;
	}

	// Return rotation as a Quaternion
	public Quaternion getRotation () {
		return transform.rotation;
	}

	// Set rotation as a degree from 0 to 359. Handle overflow in this function
	public void setRotation (float newRotation) {
		transform.rotation = Quaternion.Euler(0, 0, newRotation);
	}

	public Vector3 getPosition () {
		return transform.position;
	}

	public void setPosition (Vector3 newPosition) {
		transform.position = newPosition;
	}
}
