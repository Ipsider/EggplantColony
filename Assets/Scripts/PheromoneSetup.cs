using UnityEngine;
using System.Collections;

public class PheromoneSetup : MonoBehaviour {

	// The location of the food.
	public Vector3 foodLocation;

	// General setup.
	private Setup setup;

	// Use this for initialization.
	void Start () {
	
		// Get general information.
		setup = GameObject.FindGameObjectWithTag ("Setup").GetComponent<Setup>();
		
		// Evaporate pheromone after pheromoneEvaporationTime seconds.
		Destroy (gameObject, setup.pheromoneEvaporationTime);
	}
}
