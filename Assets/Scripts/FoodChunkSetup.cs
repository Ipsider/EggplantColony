using UnityEngine;
using System.Collections;

public class FoodChunkSetup : MonoBehaviour {

	// Component of the agent for rendering purposes.
	private Renderer myRenderer;

	// Use this for initialization
	void Start () {

		// Deactivate the gameObject at first.
		this.gameObject.SetActive (false);

		// Get the renderer component and set the color of the agent.
		myRenderer = GetComponent<Renderer> ();
		myRenderer.material.color = new Color (1f, 1f, 0.5f);
	}
}
