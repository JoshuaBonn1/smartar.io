using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBots : MonoBehaviour {
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
		float width = groundToSpawnOn.transform.localScale.x - 1.0f;
		float height = groundToSpawnOn.transform.localScale.y - 1.0f;
		for (int spawnCount = 0; spawnCount < numberToSpawn; spawnCount++) {
			GameObject thing = (GameObject)Instantiate (thingToSpawn, this.transform);
			thing.GetComponent<Bot> ().setPosition(new Vector3(
				Random.value * width - width / 2.0f, 
				Random.value * height - height / 2.0f, 
				0));
			thing.GetComponent<Bot> ().ground = groundToSpawnOn;
			thing.name = "Bot" + spawnCount;
		}
	}
}
