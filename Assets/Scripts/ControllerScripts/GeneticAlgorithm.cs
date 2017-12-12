using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour {
	// Use this entire program for the GA
	// Also use it for initial spawning...
	// Use this for initialization
	public GameObject groundToSpawnOn;
	public GameObject individual;
	public int populationSize;
	public int tournamentSize;
	[Range(0.0f, 1.0f)]
	public float mutationRate;

	public GameObject[] population;

	void Start () {
		population = new GameObject[populationSize];
		initialSpawn ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void initialSpawn () {
		float width = groundToSpawnOn.transform.localScale.x - 1.0f;
		float height = groundToSpawnOn.transform.localScale.y - 1.0f;
		for (int spawnCount = 0; spawnCount < populationSize; spawnCount++) {
			GameObject thing = (GameObject)Instantiate (individual, this.transform);
			thing.SendMessage ("Start");
			thing.GetComponent<Bot> ().setPosition(new Vector3(
				Random.value * width - width / 2.0f, 
				Random.value * height - height / 2.0f, 
				0));
			thing.GetComponent<Bot> ().ground = groundToSpawnOn;
			thing.name = "Bot";
			population [spawnCount] = thing;
		}
	}

	public float[] getWeights() {
		// Select 5 random bots (Not including current one?)
		List<int> randomValues = new List<int>();
		while (randomValues.Count < tournamentSize) {
			int newValue = Random.Range (0, populationSize);
			if (!randomValues.Contains(newValue))
				randomValues.Add(newValue);
		}
		// Pick best
		int numOfWeights = population[0].GetComponent<NeuralNetwork>().weights.Length;
		float[] parent1 = new float[numOfWeights];
		float bestFitness = -1.0f;
		foreach (int value in randomValues) {
			float fitness = population [value].GetComponent<Bot> ().getFitness ();
			if (bestFitness < fitness) {
				bestFitness = fitness;
				parent1 = population [value].GetComponent<NeuralNetwork> ().weights;
			}
		}
		// Select 5 random bots
		randomValues.Clear();
		while (randomValues.Count < tournamentSize) {
			int newValue = Random.Range (0, populationSize);
			if (!randomValues.Contains(newValue))
				randomValues.Add(newValue);
		}
		// Pick best
		float[] parent2 = new float[numOfWeights];
		bestFitness = -1.0f;
		foreach (int value in randomValues) {
			float fitness = population [value].GetComponent<Bot> ().getFitness ();
			if (bestFitness < fitness) {
				bestFitness = fitness;
				parent2 = population [value].GetComponent<NeuralNetwork> ().weights;
			}
		}
		// Crossover
		float[] child = new float[numOfWeights];
		int crossoverPoint = Random.Range (0, numOfWeights);
		for (int i = 0; i < numOfWeights; i++) {
			if (i < crossoverPoint)
				child [i] = parent1 [i];
			else
				child [i] = parent2 [i];
			if (Random.value < mutationRate)
				child [i] += Random.value * 0.5f - 0.25f;
			if (child [i] > 5.0f)
				child [i] = 5.0f;
			if (child [i] < -5.0f)
				child [i] = -5.0f;
		}
		// Mutation
		return child;
	}
}
