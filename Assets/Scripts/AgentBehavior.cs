using UnityEngine;
using System.Collections;

public class AgentBehavior : MonoBehaviour {

	// The conditions of the agents current behavior.
	private bool foundFood;			// food is found at a specific place.
	private Vector3 foodLocation;	// the location the food is found.
	private bool transportingFood;	// a bit of the chunk is collected by the ant.
	private bool foundPheromones;	// a pheromone trail is found.

	// The distance the agent moves in one iteration.
	private int moveRadius;
	
	// The behavior states in which the agent is in.
	private enum State
	{
		EXPLORE,
		FOLLOW_PHEROMONES,
		TRANSPORT_FOOD
	}

	// The active behavior at the moment.
	private State activeState;
	
	// Component of the agent for rendering purposes.
	private Renderer renderer;
	// Component for the navigation.
	private NavMeshAgent navMeshAgent;
	// Component for the trail renderer.
	private TrailRenderer trailRenderer;

	// Helper field for the exploring behavior.
	private float timestamp;
	
	// The initialization.
	void Start () {

		// No food and no pheromones.
		foundFood = false;
		foundPheromones = false;
		moveRadius = 50;
		activeState = State.EXPLORE;

		// Get the renderer component and set the color of the agent.
		renderer = GetComponent<Renderer> ();
		renderer.material.color = new Color (0.75f, 0.25f, 0.25f);

		// Get the navigation component.
		navMeshAgent = GetComponent<NavMeshAgent> ();

		// Get the trail renderer component.
		trailRenderer = this.gameObject.transform.GetChild (0).GetComponent<TrailRenderer> ();
	}

	// Update is called once per frame
	void Update () {
	
		// A switch statement to change between the three behavior states.
		switch (activeState)
		{

		// The exploration behavior.
		case State.EXPLORE:

			explore();

			// If exploring and food is found, switch to transport food behavior.
			if (foundFood) {
				activeState = State.TRANSPORT_FOOD;
			}

			// If exploring and a pheromone trail is found, switch to following a pheromone trail.
			if (foundPheromones) {
				activeState = State.FOLLOW_PHEROMONES;
			}

			break;

		// The behavior for following a pheromone trail.
		case State.FOLLOW_PHEROMONES:

			Debug.Log("Following pheromone trail.");

			// If exploring and food is found, switch to transport food behavior.
			if (foundFood) {
				activeState = State.TRANSPORT_FOOD;
			}

			// If a pheromone trail is lost, switch back to exploring.
			if (!foundPheromones) {
				activeState = State.EXPLORE;
			}

			break;

		// The food transportation behavior.
		case State.TRANSPORT_FOOD:

			transportFood();

			//Debug.Log("Transporting food.");
			
			// If found food and there are no chunks of food left, deliver the last bit and start exploring again.
			if (!foundFood && !transportingFood) {
				activeState = State.FOLLOW_PHEROMONES;
			}

			break;
		}
	}
	
	// Explore. Finds a new destination in the immediate surrounding of the agent every one to two seconds.
	void explore () {

		// Reorientate every 1 to 2 seconds.
		if(timestamp < Time.time) {
			
			/**
			 * 
			 * Find a new destination to move to:
			 * 
			 **/

			// Generate random vector within a unit sphere.
			Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
			// Add agents current position to this vector.
			randomDirection += transform.position;
			NavMeshHit navMeshHit;
			NavMesh.SamplePosition (randomDirection, out navMeshHit, moveRadius, 1);
			Vector3 finalPosition = navMeshHit.position;
			navMeshAgent.destination = finalPosition;
			
			// Reset the timer (to one to two seconds).
			timestamp += Random.Range(1f, 2f);
		}

		// TODO: Change color of pheromone?
		trailRenderer.time = 5f;
	}

	void transportFood () {

		// if the ant is in fact transporting food, go back to the nest to deliver it.
		if (transportingFood) {
			navMeshAgent.destination = GameObject.FindGameObjectWithTag ("Nest").transform.position;
		}

		// if the ant has delivered the food, go back to the location to pick up the next chunk.
		else if (!transportingFood) {
			navMeshAgent.destination = foodLocation;
		}

		// TODO: Set the pheromone particle parameters.
		trailRenderer.time = 25f;
	}

	// If the agent touches a gameobject
	void OnTriggerEnter(Collider other) {
		
		// Check if the object is food.
		if (other.gameObject.tag == "Food") {

			// collect the food.
			foundFood = true;
			foodLocation = other.transform.position;
			transportingFood = true;

			// diminish the amount of food left.
			Debug.Log ("Ant: Food collected!");
		}

		// Check if the object is the nest.
		if (other.gameObject.tag == "Nest" && foundFood) {

			// Deliver the food.
			foundFood = true;

			transportingFood = false;
			Debug.Log ("Ant: Food delivered!");
		}
	}
}
