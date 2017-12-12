using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnFood : MonoBehaviour {
	public GameObject thingToSpawn;
	public int numberToSpawn;

	// The size of this ground is determined by it's scale
	public GameObject groundToSpawnOn;

	// Use this for initialization
	void Start () {
		spawn ();
	}

	// Spawn the bots randomly on the available ground
	private void spawn () {
		// TODO: Remove any overlapping upon spawning
		float width = groundToSpawnOn.transform.localScale.x - 1.0f;
		float height = groundToSpawnOn.transform.localScale.y - 1.0f;
		for (int spawnCount = 0; spawnCount < numberToSpawn; spawnCount++) {
			GameObject thing = (GameObject)Instantiate (thingToSpawn);
			thing.GetComponent<Food> ().kill ();
			thing.name = "Food" + spawnCount;
		}
	}
}
