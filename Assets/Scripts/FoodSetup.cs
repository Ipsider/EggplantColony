using UnityEngine;
using System.Collections;

public class FoodSetup : MonoBehaviour {

	// The amount of food in this chunk.
	private int amountOfFood;

	// Component of the agent for rendering purposes.
	private Renderer renderer;

	// Use this for initialization
	void Start () {

		// TODO: set amount of food.
	
		// Get the renderer component and set the color of the agent.
		renderer = GetComponent<Renderer> ();
		renderer.material.color = new Color (1f, 1f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
		// TODO: update size of food according to the amount of food.
	}

	// If any gameobject touches the food...
	void OnTriggerEnter(Collider other) {

		// Check if the object is an ant.
		if (other.gameObject.tag == "Ant") {

			// If so, diminish the amount of food.
			Debug.Log ("Food: Food collected!");
		}
	}
}
