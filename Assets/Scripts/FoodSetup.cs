using UnityEngine;
using System.Collections;

public class FoodSetup : MonoBehaviour {

	// Are there any chunks of food left at the spot?
	public bool foodLeft;

	// The amount of food in this spot.
	private float amountOfFood;

	// Components for rendering and other purposes.
	private Renderer myRenderer;
	private Setup setup;

	// Use this for initialization
	void Start () {

		amountOfFood = this.transform.localScale.x * this.transform.localScale.y * this.transform.localScale.z;
		foodLeft = true;

		// Get the renderer component and set the color of the agent.
		myRenderer = GetComponent<Renderer> ();
		myRenderer.material.color = new Color (1f, 1f, 0.5f);

		// Get general setup.
		setup = GameObject.FindGameObjectWithTag ("Setup").GetComponent<Setup>();
	}
	
	// Update is called once per frame
	void Update () {

		if (amountOfFood <= 0f) {
			foodLeft = false;
			this.gameObject.GetComponent<MeshRenderer>().enabled = false;
			this.transform.localScale = new Vector3(1f,1f,1f);
			this.transform.GetChild(0).GetComponent<SphereCollider>().radius = 3;
		}
	}
	
	// If any gameobject touches the food...
	void OnTriggerEnter(Collider other) {

		// Check if the object is an ant.
		if (other.gameObject.tag == "Ant") {
			if (foodLeft && other.gameObject.GetComponent<AntBehavior>().transportingFood) {

				amountOfFood = amountOfFood - setup.carryingCapacity;
				Vector3 newSize = new Vector3(Mathf.Pow(amountOfFood, 1f/3f), Mathf.Pow(amountOfFood, 1f/3f), Mathf.Pow(amountOfFood, 1f/3f));
				this.transform.localScale = newSize;
				this.transform.position = new Vector3(this.transform.position.x, this.transform.localScale.y/2, this.transform.position.z);
			}
		}
	}
}
