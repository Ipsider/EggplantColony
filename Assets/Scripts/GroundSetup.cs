using UnityEngine;
using System.Collections;

public class GroundSetup : MonoBehaviour {

	// Component of the agent for rendering purposes.
	private Renderer myRenderer;

	// Use this for initialization
	void Start () {
	
		// Get the renderer component and set the color of the agent.
		myRenderer = GetComponent<Renderer> ();
		myRenderer.material.color = new Color (0.9f, 0.95f, 0.7f);
	}
}
