using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {
	float f = 0.0f;
	public float maxSpeed = 1f;

	// Ground to stay in bounds on, area is in the scale of the ground
	public GameObject ground;
	public float sizeReduction;

	private int thingsEaten = 0;
	public float thingsEatenWeight;
	private float maxSize = 1.0f;
	public float maxSizeWeight;
	private long lifespan = 0;
	public float lifespanWeight;

	public float fitness;


	// Use this for initialization
	void Start () {
		f = Random.value * 360.0f;
		setRotation (f);
		gameObject.SetActive (true);
	}

	void Update () {
		checkBotOverlap ();
		if (getSize () < 0.3f || getSize() > 50.0f)
			kill ();
		fitness = getFitness ();
	}

	// Update is called once every physics update
	void FixedUpdate () {
		// Move in the direction it is facing
		// TODO: Add acceleration and max speeds
		transform.Translate (Vector3.right * maxSpeed * Mathf.Sqrt(getSize()) * Time.deltaTime);
		stayInBounds ();
		setSize (getSize () * sizeReduction);
		lifespan++;
	}

	private void checkBotOverlap () {
		Collider2D[] potentialBots = Physics2D.OverlapCircleAll (transform.position, getSize());
		foreach (Collider2D bot in potentialBots) {
			if (bot.gameObject == gameObject)
				continue;
			float distance = Vector2.Distance (transform.position, bot.transform.position);
			// Eat food on collision
			if (bot.name.Contains ("Food") && distance <= getSize() / 2.0f + bot.gameObject.transform.localScale.x / 2.0f) {
				bot.SendMessage ("kill");
				thingsEaten++;
				setSize (getSize () + 0.2f * bot.gameObject.transform.localScale.x);
				if (getSize () > maxSize)
					maxSize = getSize ();
				continue;
			}
			// Eat bots on overlap
			if (distance + bot.gameObject.transform.localScale.x / 2.0f <= getSize () / 2.0f) {
				bot.SendMessage ("kill");
				thingsEaten++;
				setSize (getSize () + 0.2f * bot.gameObject.transform.localScale.x);
				if (getSize () > maxSize)
					maxSize = getSize ();
			}
		}
	}

	private void kill () {
		GetComponent<NeuralNetwork> ().weights = GetComponentInParent<GeneticAlgorithm> ().getWeights ();
		setSize (1.0f);
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
		thingsEaten = 0;
		maxSize = getSize ();
		lifespan = 0;
		//print(GetComponentInParent<GeneticAlgorithm> ().getWeights ());
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

	public float getFitness () {
		float thingsEatenPart = thingsEaten * thingsEatenWeight;
		float maxSizePart = maxSize * maxSizeWeight;
		float lifespanPart = lifespan * lifespanWeight;
		return thingsEatenPart + maxSizePart + lifespanPart;
	}

}
