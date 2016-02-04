using UnityEngine;
using System.Collections;

public class AntBehavior : MonoBehaviour
{
	// The conditions to trigger the behaviors.
	public bool foundFood;
	public Vector3 foodLocation;	// the location the food is found.
	public bool transportingFood;	// a bit of the chunk is collected by the ant.
	private bool foundPheromone;	// a pheromone trail is found.

	// The behavior states in which the agent can be in.
	private enum State
	{
		EXPLORE,
		FOLLOW_TRAIL,
		TRANSPORT_FOOD
	}

	// The active behavior at the moment.
	private State activeState;

	// The location of the nest.
	private Vector3 nestLocation;

	// Component for the food chunk renderer.
	public GameObject foodChunk;

	// Components of the agent for various purposes.
	private Renderer myRenderer;			// for rendering
	private NavMeshAgent navMeshAgent;		// for pathfinding
	private TrailRenderer trailRenderer;	// for pheromone rendering
	private Setup setup;					// the parameters of the algorithm

	// Helper variable
	private float timestamp;

	// Use this for initialization
	void Start () {
	
		// Set default behavior.
		activeState = State.EXPLORE;

		// Set the location of the nest.
		nestLocation = GameObject.FindGameObjectWithTag ("Nest").transform.position;

		// Get the other components.
		myRenderer = GetComponent<Renderer> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
		trailRenderer = this.gameObject.transform.GetChild (0).GetComponent<TrailRenderer> ();
		setup = GameObject.FindGameObjectWithTag ("Setup").GetComponent<Setup> ();

		// Set color of object.
		myRenderer.material.color = new Color (0.75f, 0.25f, 0.25f);
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("Active state: " + activeState);
		//Debug.Log ("Food location" + foodLocation);
		//Debug.Log ("Found pheromone?" + foundPheromone);
		//Debug.Log ("Ant perspective: " + transportingFood);
		
		// A switch statement to change between the three behavior states.
		switch (activeState) {
			
			// The exploration behavior.
			case State.EXPLORE:
			
				explore ();

				// The transition conditions to another state.
				if (transportingFood) {
					activeState = State.TRANSPORT_FOOD;
				}
				if (foundPheromone) {
					activeState = State.FOLLOW_TRAIL;
				}

				break;
			
			// The behavior for following a pheromone trail.
			case State.FOLLOW_TRAIL:
			
				followTrail ();

				// The transition conditions to another state.
				if (transportingFood) {

					activeState = State.TRANSPORT_FOOD;
				}

				if (!foundFood) {
					activeState = State.EXPLORE;
				}

				break;
			
			// The food transportation behavior.
			case State.TRANSPORT_FOOD:
			
				transportFood ();

				// The transition conditions to another state.
				if (!transportingFood) {

					if (foodLocation != Vector3.zero) {
						activeState = State.FOLLOW_TRAIL;
					} else {
						activeState = State.EXPLORE;
					}
				}

				break;
		}
	}

	// Explore. Finds a new destination in the immediate surrounding of the agent every one to two seconds.
	void explore ()
	{
		
		// Reorientate every 1 to 2 seconds.
		if (timestamp < Time.time) {
			
			// Generate random vector within a unit sphere.
			Vector3 randomDirection = Random.insideUnitSphere * setup.moveRadius;
			// Add agents current position to this vector.
			randomDirection += transform.position;
			NavMeshHit navMeshHit;
			NavMesh.SamplePosition (randomDirection, out navMeshHit, setup.moveRadius, 1);
			Vector3 finalPosition = navMeshHit.position;
			navMeshAgent.destination = finalPosition;
			
			// Reset the timer (to one to two seconds).
			timestamp += Random.Range (1f, 2f);
		}

		// Cange the intensity (length) of the pheromones smoothly.
		trailRenderer.time = Mathf.Lerp(trailRenderer.time, 0f, 0.01f);
	}

	// Follow a trail to the food location.
	void followTrail ()
	{

		navMeshAgent.destination = foodLocation;
		trailRenderer.time = Mathf.Lerp(trailRenderer.time, 0f, 0.01f);
	}

	// Transport food back to the nest.
	void transportFood ()
	{

		navMeshAgent.destination = nestLocation;
		trailRenderer.enabled = true;
		trailRenderer.time = setup.pheromoneEvaporationTime;
	}

	// If the agent touches a gameobject...
	void OnTriggerEnter (Collider other)
	{
		
		// Check if the object is food.
		if (other.gameObject.tag == "Food") {

			foundPheromone = false;

			if (other.transform.parent.GetComponent<FoodSetup> ().foodLeft) {

				foundFood = true;
				foodLocation = other.transform.position;
				transportingFood = true;
				foodChunk.SetActive (true);
			} else {
			
				foundFood = false;
				foundPheromone = false;
				foodLocation = new Vector3 (0f, 0f, 0f);
			}
		}
		
		// Check if the object is the nest.
		if (other.gameObject.tag == "Nest") {

			if (transportingFood) {
				other.gameObject.GetComponent<NestSetup>().score += setup.carryingCapacity;
			}

			transportingFood = false;
			foodChunk.SetActive (false);
		}
		
		// Check if the object is the nest.
		if (other.gameObject.tag == "Pheromone") {

			foundPheromone = true;
			foodLocation = other.gameObject.GetComponent<PheromoneSetup> ().foodLocation;
		}
	}

	// Quick and dirty work around.
	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Food") {
			if (!other.transform.parent.GetComponent<FoodSetup> ().foodLeft) {
				foundPheromone = false;
			}
		}
	}
}
