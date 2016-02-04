using UnityEngine;
using System.Collections;

public class Setup : MonoBehaviour {

	public int pheromoneRate;				// Segregate pheromone every n frams TODO: change to seconds.
	public int pheromoneEvaporationTime;	// Time until a pheromone evaporates.
	public float antSpeed;					// The speed of the ants.
	public float carryingCapacity;			// The size of the food chunks the ants are able to carry.
	public float moveRadius;				// The vector for finding random destinations to head to.


	void Start () {
	
		pheromoneRate = 15;
		pheromoneEvaporationTime = 20;
		moveRadius = 50f;

		// TODO change the size of the visual representation of the food chunk as well.
		carryingCapacity = 0.25f;
	}
}
