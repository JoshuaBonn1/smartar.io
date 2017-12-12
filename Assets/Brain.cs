using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

	public List<GameObject> visibleObjects;
	public float sightRange;
	public float[] inputs;

	public NeuralNetwork nn;

	// Use this for initialization
	void Start () {
		nn = gameObject.GetComponent<NeuralNetwork> ();
		inputs = new float[nn.numOfInputs];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		updateInputs ();
		nn.feedForward (inputs);
		gameObject.GetComponent<Bot> ().setRotation (nn.output[0]);
	}

	private void updateInputs () {
		int increment = (int) (360.0f / (float) inputs.Length);
		for (int counter = 0; counter < 360; counter+=increment) {
			// Convert counter to angle to vector
			float angle = (float)counter * Mathf.PI / 180.0f;
			Vector2 direction = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
			Vector2 castVector = (Vector2)transform.position + direction * (transform.localScale.x + 0.02f) / 2.0f;
			RaycastHit2D hit = Physics2D.Raycast(castVector, direction, sightRange);
			//Debug.DrawRay (castVector, direction * sightRange, Color.black);

			if (hit) {
				float edible = -1.0f;
				float enemySize = hit.collider.transform.localScale.x;
				if (enemySize < transform.localScale.x)
					edible = 1.0f;
				float distanceToEnemy = hit.distance;
				if (distanceToEnemy < 0.001f)
					distanceToEnemy = 0.001f;
				inputs [counter / increment] = (edible) * (enemySize) / (distanceToEnemy);
				//print (inputs[counter]);
				Debug.DrawRay (castVector, direction * hit.distance, Color.black);

			} else
				inputs [counter / increment] = 0.0f;
		}

		// Temporary code to follow best individual
		float maxInput = 0.0f;
		int maxIndex = 0;
		for (int i = 0; i < inputs.Length; i++) {
			if (inputs [i] > maxInput) {
				maxInput = inputs [i];
				maxIndex = i;
			}
		}
		gameObject.GetComponent<Bot> ().setRotation (maxIndex);
	}

}
