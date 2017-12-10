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
	private void spawn() {
		// TODO: Remove any overlapping upon spawning
		float width = groundToSpawnOn.transform.localScale.x;
		float height = groundToSpawnOn.transform.localScale.y;
		for (int spawnCount = 0; spawnCount < numberToSpawn; spawnCount++) {
			GameObject thing = (GameObject)Instantiate (thingToSpawn);
			thing.GetComponent<Bot> ().setPosition(new Vector3(
				Random.value * width - width / 2.0f, 
				Random.value * height - height / 2.0f, 
				0));
			thing.name = "Bot" + spawnCount;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
