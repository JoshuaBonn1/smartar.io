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
		inputs = new float[360];
		nn = gameObject.GetComponent<NeuralNetwork> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		updateInputs ();
		//nn.feedForward (inputs);
		//gameObject.GetComponent<Bot> ().setRotation (nn.output[0]);
	}

	private void updateInputs () {
		for (int counter = 0; counter < 360; counter++) {
			// Convert counter to angle to vector
			float angle = (float)counter * Mathf.PI / 180.0f;
			Vector2 direction = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));
			Vector2 castVector = (Vector2)transform.position + direction * (transform.localScale.x + 0.02f) / 2.0f;
			RaycastHit2D hit = Physics2D.Raycast(castVector, direction, sightRange);

			if (hit) {
				float edible = -1.0f;
				float enemySize = hit.collider.transform.localScale.x;
				if (enemySize < transform.localScale.x)
					edible = 1.0f;
				inputs [counter] = (edible) * (enemySize) / (hit.distance);
				//print (inputs[counter]);
				Debug.DrawRay (castVector, direction * hit.distance, Color.black,0.1f);
			} else
				inputs [counter] = 0.0f;
			counter++;
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
