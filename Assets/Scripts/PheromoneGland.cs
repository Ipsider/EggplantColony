using UnityEngine;
using System.Collections;
public class PheromoneGland : MonoBehaviour
{
	public GameObject Pheromone;
	private GameObject pheromones;

	// Component of the AgentBehavior.
	private AntBehavior antBehavior;

	// Helper field for the pheromone segregation.
	private float timestamp;

	// General setup.
	private Setup setup;
	
	// The initialization.
	void Start() {

		// Get the pheromones folder.
		pheromones = GameObject.Find ("Pheromones");

		// Get general information.
		setup = GameObject.FindGameObjectWithTag ("Setup").GetComponent<Setup> ();
		
		// Get the agentBehavior script.
		antBehavior = this.transform.parent.GetComponent<AntBehavior> ();
	}

	void Update()
	{
		// Spawn pheromone every n frames.
		// TODO: change to time based rate?
		// TODO: change this if the speed of the ants is changed.
		timestamp++;

		if (antBehavior.foundFood) {

			if (timestamp % setup.pheromoneRate == 0) { 

				// Instantiate new pheromone object from the prefab.
				GameObject newPheromone = Instantiate(Pheromone, this.transform.position, Quaternion.identity) as GameObject;

				// Store all pheromones in folder pheromones.
				newPheromone.transform.parent = pheromones.transform;

				// Commit food location information of agent to pheromone object
				newPheromone.GetComponent<PheromoneSetup>().foodLocation = antBehavior.foodLocation;
			}
		}
	}
}